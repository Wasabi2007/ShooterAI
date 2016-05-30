using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class GenerateWayPoints : MonoBehaviour {

	public float x_grid_size = 1;
	public float y_grid_size = 1;

	private BoxCollider2D sprite_collider;
	public Node node_prefab;
	public LayerMask castLayer;

	private List<Node> childs = new List<Node>();
	private Vector3 old_pos = Vector3.zero;
	// Use this for initialization
	void Start () {
		generateWayPoints();
	}

	
	// Update is called once per frame
	void generateWayPoints() {
		sprite_collider = GetComponent<BoxCollider2D> ();

		foreach (Node n in childs) {
			if(n != null)
				GameObject.DestroyImmediate (n.gameObject);
		}
		childs.Clear();

		var bounds = sprite_collider.bounds;
		var castdistanze = x_grid_size / 4.0f;

		for (float x = Mathf.RoundToInt(bounds.min.x/x_grid_size)*x_grid_size; x < Mathf.RoundToInt(bounds.max.x/x_grid_size)*x_grid_size+x_grid_size; x += x_grid_size) {
			for (float y = Mathf.RoundToInt(bounds.min.y/y_grid_size)*y_grid_size; y < Mathf.RoundToInt(bounds.max.y/y_grid_size)*y_grid_size+y_grid_size; y += y_grid_size) {
				Vector3 origin = new Vector2 (x, y);
				var circle_hit = Physics2D.OverlapCircle (origin, castdistanze,castLayer.value); // can we place a Waypoint there?
				if (!circle_hit) {
					Node n = GameObject.Instantiate (node_prefab.gameObject).GetComponent<Node>();
					childs.Add (n);
					n.transform.position = origin;
					n.transform.parent = transform;

				}
			}
		}
		
	}
}
