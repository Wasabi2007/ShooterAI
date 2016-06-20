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

	public enum way_point_type{
		normal,
		cover,
		fullcover,
		ammo
	}

	public way_point_type way_point_type_;
	public List<direction> duck_direction = new List<direction>();

	public Sprite cover_img;
	public Sprite normal_img;
	public Sprite ammo_img;

	public static bool show_all_connections = false;
	public static int show_percent_connections = 0;

	public LayerMask cover_cast;
	public float cover_range;

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
		//ist it a usable cover
		if (way_point_type_ == way_point_type.normal) {
			var hit = Physics2D.Raycast (transform.position, Vector2.down, cover_range, cover_cast.value);
			if (hit) {
				way_point_type_ = way_point_type.cover;
				duck_direction.Add(Node.direction.SOUTH);
			}

			hit = Physics2D.Raycast (transform.position, Vector2.up, cover_range, cover_cast.value);
			if (hit) {
				way_point_type_ = way_point_type.cover;
				duck_direction.Add(Node.direction.NOTRH);
			}

			hit = Physics2D.Raycast (transform.position, Vector2.left, cover_range, cover_cast.value);
			if (hit) {
				way_point_type_ = way_point_type.cover;
				duck_direction.Add(Node.direction.WEST);
			}

			hit = Physics2D.Raycast (transform.position, Vector2.right, cover_range, cover_cast.value);
			if (hit) {
				way_point_type_ = way_point_type.cover;
				duck_direction.Add(Node.direction.EAST);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (duck_direction.Count > 0) {
			transform.localRotation = Quaternion.AngleAxis ((float)duck_direction [0], Vector3.forward);
			duck_direction = duck_direction;
		}
	}

	public void clear(){
		Connection.Clear ();
	}

	public void add_node(Node n){
		if(!Connection.Contains(n))
			Connection.Add(n);
	}

	void OnDrawGizmos(){

		Random.seed = 42;
		foreach (Node n in Connection) {
			if (!n)
				continue;
			if (!show_all_connections && Random.Range (0, 100) > show_percent_connections)
				continue;

			Gizmos.DrawLine (transform.position, n.transform.position);
		}

		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer> ();

		switch (way_point_type_) {
		case way_point_type.normal:{sr.sprite = normal_img;}break;
		case way_point_type.cover:{sr.sprite = cover_img;Gizmos.color = Color.magenta;}break;
		case way_point_type.ammo:{sr.sprite = ammo_img;}break;
		}


		if (in_use)
			Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, 0.25f);
	}
}
