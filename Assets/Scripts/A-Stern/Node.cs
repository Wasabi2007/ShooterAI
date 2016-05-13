using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[ExecuteInEditMode]
public class Node : MonoBehaviour {

	public enum direction{
		NOTRH = 90,
		SOUTH = 270,
		EAST = 0,
		WEST = 180,
	}

	public bool ducking_spot;
	public direction duck_direction;

	[HideInInspector]
	public bool in_use;

	[HideInInspector]
	public List<Node> Connection;

	[HideInInspector]
	public int value = 0;

	[HideInInspector]
	public Node parrent = null;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		transform.localRotation = Quaternion.AngleAxis ((float)duck_direction,Vector3.forward);
		duck_direction = duck_direction;
	}

	public void clear(){
		Connection.Clear ();
	}

	public void add_node(Node n){
		if(!Connection.Contains(n))
			Connection.Add(n);
	}

	void OnDrawGizmos(){
		foreach (Node n in Connection) {
			if (!n)
				continue;
			Gizmos.DrawLine (transform.position, n.transform.position);
		}

		if (ducking_spot) {
			Gizmos.color = Color.magenta;
		}

		if (in_use)
			Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, 0.25f);
	}
}
