using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

[ExecuteInEditMode]
public class AStern : MonoBehaviour {

	public LayerMask hit_mask;
	public bool searchgrid = false;
	private Node[] nodes;
	private HashSet<Node> ducking_nodes;

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

	Node get_nearest_duck_node(Vector2 position,float angle){
		float current_min_dist = float.MaxValue;
		Node current_min_node = null;
		foreach (Node n in ducking_nodes) {
			if (!n.in_use && Mathf.Abs((float)n.duck_direction-angle)<45) { //Warning doesn't work with angles > 360 or negativ angles
				float dist = Vector3.Distance (position, n.transform.position);
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
		ducking_nodes.Clear ();

		for (int i = 0; i < nodes.Length;++i) {
			Node n = nodes [i];
			if (n.ducking_spot) {
				ducking_nodes.Add (n);
			}
			for (int j = i+1; j < nodes.Length;++j) {
				Node n2 = nodes [j];
				RaycastHit2D hit = Physics2D.Raycast (n.transform.position, (n2.transform.position - n.transform.position).normalized,1000000,hit_mask);
				if (!hit) {
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
