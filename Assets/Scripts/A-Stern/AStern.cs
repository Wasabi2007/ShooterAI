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

	public List<Node> get_path(Node start,Node end){
		
		SimplePriorityQueue<Node> openlist = new SimplePriorityQueue<> ();
		List<Node> closedlist = new List<>();

		start.value = 0;
		openlist.Enqueue (start, 0);

		while (openlist.Count > 0) {
			Node current_node = openlist.Dequeue ();
			if (current_node == end) {
				break;
			}
			closedlist.Add (closedlist);

			foreach (Node n in current_node.Connection) {
				if( openlist.Contains(n))n.value=0;
				if( closedlist.Contains(n))continue;
				var tentative_g = current_node.value + Vector3.Distance(current_node.transform.position, n.transform.position);

				if(openlist.Contains(n) && tentative_g >= n.value)continue;

				n.parrent = current_node;
				n.value = tentative_g;
				var f = tentative_g + Vector3.Distance(end.transform.position, n.transform.position);
				if (openlist.Contains (n))
					openlist.UpdatePriority (n, f);
				else
					openlist.Enqueue (n, f);
			}
		}
	}
}
