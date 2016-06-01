using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

[ExecuteInEditMode]
public class AStern : MonoBehaviour {

	public LayerMask hit_mask;
	public bool searchgrid = false;
	public float raycast_thicknes = 0.2f;
	private Node[] nodes = null;
	private HashSet<Node> cover_nodes = new HashSet<Node>();

	// Use this for initialization
	void Start () {
		find_connections ();
	}

	// Update is called once per frame
	void Update () {
		if (searchgrid) {
			searchgrid = false;
			find_connections ();
		}
	}

	public Node get_nearest_node(Vector2 position){
		if(nodes == null)
			find_connections ();
		float current_min_dist = float.MaxValue;
		Node current_min_node = null;
		foreach (Node n in nodes) {
			float dist = Vector3.Distance (position, n.transform.position);
			if (dist < current_min_dist) {
				current_min_dist = dist;
				current_min_node = n;
			}
		}
		return current_min_node;
	}

	public Node get_nearest_cover_node(Vector2 position,Vector3 target_position){
		if(cover_nodes.Count == 0)
			find_connections ();
		float current_min_dist = float.MaxValue;
		Node current_min_node = null;
		foreach (Node n in cover_nodes) {
			Vector3 rel_pos = target_position - n.transform.position;
			rel_pos.Normalize ();
			float angle = Mathf.Atan2(rel_pos.y,rel_pos.x)*Mathf.Rad2Deg;
			if (angle < 0) {
				angle = 360 + angle;
			}

			if (!n.in_use && Mathf.Abs((int)n.duck_direction-angle)<45) { //Warning doesn't work with angles > 360 or negativ angles
				float dist = Vector3.Distance (position, n.transform.position); // maybe replace with a better Distance function? one that takes the path into account
				if (dist < current_min_dist) {
					current_min_dist = dist;
					current_min_node = n;
				}
			}
		}
		return current_min_node;
	}

	void find_connections(){
		nodes = transform.GetComponentsInChildren<Node> ();
		foreach (Node n in nodes) {
			n.clear ();
		}
		cover_nodes.Clear ();

		for (int i = 0; i < nodes.Length;++i) {
			Node n = nodes [i];
			if (n.ducking_spot) {
				cover_nodes.Add (n);
			}
			for (int j = i+1; j < nodes.Length;++j) {
				Node n2 = nodes [j];
				Vector2 ray = (n2.transform.position - n.transform.position).normalized;
				Vector3 normal = Vector3.zero;
				normal.x = -ray.y;
				normal.y = ray.x;

				bool add_connection = true;

				RaycastHit2D hit = Physics2D.Raycast (n.transform.position, ray,1000000,hit_mask);
				add_connection &= !hit;

				hit = Physics2D.Raycast (n.transform.position+normal*raycast_thicknes, ray,1000000,hit_mask);
				add_connection &= !hit;

				hit = Physics2D.Raycast (n.transform.position-normal*raycast_thicknes, ray,1000000,hit_mask);
				add_connection &= !hit;

				if (add_connection) {
					n.add_node (n2);
					n2.add_node (n);
				}
			}
		}
	}

	void reset(){
		foreach (Node n in nodes) {
			n.value = 0;
			n.parrent = null;
		}
	}

	public List<Node> get_path(Node start,Node end){
		
		SimplePriorityQueue<Node> openlist = new SimplePriorityQueue<Node> ();
		List<Node> closedlist = new List<Node>();

		start.value = 0;
		openlist.Enqueue (start, 0);

		while (openlist.Count > 0) {
			Node current_node = openlist.Dequeue ();
			if (current_node == end) {
				break;
			}

			closedlist.Add (current_node);

			foreach (Node n in current_node.Connection) {
				if( closedlist.Contains(n))continue;
				int tentative_g = current_node.value + Mathf.RoundToInt(Vector3.Distance(current_node.transform.position, n.transform.position));

				if(openlist.Contains(n) && tentative_g >= n.value)continue;

				n.parrent = current_node;
				n.value = tentative_g;
				int f = tentative_g + Mathf.RoundToInt(Vector3.Distance(end.transform.position, n.transform.position));
				if (openlist.Contains (n))
					openlist.UpdatePriority (n, f);
				else
					openlist.Enqueue (n, f);
			}
		}

		List<Node> way = new List<Node>();

		for(Node prev = end; prev != null; prev = prev.parrent){
			way.Add (prev);
		}
		way.Reverse ();

		reset ();
		return way;
	}
}
