using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

[ExecuteInEditMode]
public class AStern : MonoBehaviour {


	private Node[] nodes;

	// Use this for initialization
	void Start () {
		nodes = GetComponentsInChildren<Node> ();
	}

	// Update is called once per frame
	void Update () {
	
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
