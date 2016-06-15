using UnityEngine;
using System.Collections;

public class FillWithWayPoints : MonoBehaviour {

	public Bounds level_bounderies = new Bounds(Vector3.zero,Vector3.one);
	public GameObject way_point;
	public Vector2 grid_cell_size = Vector2.one;
	public int spacing_x = 1;
	public int spacing_y = 1;
	public LayerMask castLayer;


	// Use this for initialization
	void Start () {
		if (way_point) {
			SpriteRenderer sprite_collider = way_point.GetComponent<SpriteRenderer> ();

			for (float x = level_bounderies.min.x; x < level_bounderies.max.x; x += grid_cell_size.x * spacing_x)
				for (float y = level_bounderies.min.y; y < level_bounderies.max.y; y += grid_cell_size.y * spacing_y) {
					var circle_hit = Physics2D.OverlapCircle (new Vector2 (x, y), sprite_collider.bounds.size.magnitude,castLayer.value);
					if (circle_hit)
						continue;
					
					GameObject point = GameObject.Instantiate (way_point);
					point.transform.position = new Vector3 (x, y);
					point.transform.parent = transform;
				}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos(){
		if (way_point) {
			SpriteRenderer sprite_collider = way_point.GetComponent<SpriteRenderer> ();


			Gizmos.DrawWireCube (level_bounderies.center, level_bounderies.size);
			for (float x = level_bounderies.min.x; x < level_bounderies.max.x; x += grid_cell_size.x * spacing_x)
				for (float y = level_bounderies.min.y; y < level_bounderies.max.y; y += grid_cell_size.y * spacing_y) {
					var circle_hit = Physics2D.OverlapCircle (new Vector2 (x, y), sprite_collider.bounds.size.magnitude,castLayer.value);
					if (circle_hit)
						continue;
				
					Gizmos.DrawCube (new Vector3 (x, y), sprite_collider.bounds.size);
				}
		}
	}
}
