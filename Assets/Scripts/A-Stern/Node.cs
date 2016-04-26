using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[ExecuteInEditMode]
public class Node : MonoBehaviour {

	public bool ducking_spot;
	public float degree;

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
		transform.localRotation = Quaternion.AngleAxis (degree,Vector3.forward);
		degree = degree - 360 * Mathf.Floor (degree / 360);
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
	}
}
