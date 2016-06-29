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
		cover_fullcover,
		ammo
	}

	public way_point_type way_point_type_;
	public List<direction> cover_direction = new List<direction>();
	public List<direction> full_cover_direction = new List<direction>();

	public Sprite cover_img;
	public Sprite normal_img;
	public Sprite ammo_img;

	public static bool show_all_connections = false;
	public static int show_percent_connections = 0;

	public LayerMask cover_cast;
	public float cover_range;
	public LayerMask full_cover_cast;

	public int base_value = 1000;
	[HideInInspector]
	public int real_value = 0;


	[HideInInspector]
	public bool in_use;

	[HideInInspector]
	public List<Node> Connection;

	[HideInInspector]
	public int value = 0;

	[HideInInspector]
	public Node parrent = null;

	private int cover_value = 10;
	private int full_cover_value = 100;

	// Use this for initialization
	void Start () {
		//GameObject.DestroyImmediate (GetComponent<CircleCollider2D> ());

		real_value = base_value;
		//ist it a usable cover
		if (way_point_type_ == way_point_type.normal) {
			var hit = Physics2D.Raycast (transform.position, Vector2.down, cover_range, cover_cast.value);
			if (hit) {
				way_point_type_ = way_point_type.cover;
				cover_direction.Add(Node.direction.SOUTH);
				real_value -= cover_value;
			}

			hit = Physics2D.Raycast (transform.position, Vector2.up, cover_range, cover_cast.value);
			if (hit) {
				way_point_type_ = way_point_type.cover;
				cover_direction.Add(Node.direction.NOTRH);
				real_value -= cover_value;
			}

			hit = Physics2D.Raycast (transform.position, Vector2.left, cover_range, cover_cast.value);
			if (hit) {
				way_point_type_ = way_point_type.cover;
				cover_direction.Add(Node.direction.WEST);
				real_value -= cover_value;
			}

			hit = Physics2D.Raycast (transform.position, Vector2.right, cover_range, cover_cast.value);
			if (hit) {
				way_point_type_ = way_point_type.cover;
				cover_direction.Add(Node.direction.EAST);
				real_value -= cover_value;
			}



			hit = Physics2D.Raycast (transform.position, Vector2.down, cover_range, full_cover_cast.value);
			if (hit) {
				way_point_type_ = (way_point_type_ == way_point_type.cover|| way_point_type_ ==  way_point_type.cover_fullcover?way_point_type.cover_fullcover:way_point_type.fullcover);
				full_cover_direction.Add(Node.direction.SOUTH);
				real_value -= full_cover_value;
			

			}

			hit = Physics2D.Raycast (transform.position, Vector2.up, cover_range, full_cover_cast.value);
			if (hit) {
				way_point_type_ = (way_point_type_ == way_point_type.cover|| way_point_type_ ==  way_point_type.cover_fullcover?way_point_type.cover_fullcover:way_point_type.fullcover);
				full_cover_direction.Add(Node.direction.NOTRH);
				real_value -= full_cover_value;

			}

			hit = Physics2D.Raycast (transform.position, Vector2.left, cover_range, full_cover_cast.value);
			if (hit) {
				way_point_type_ = (way_point_type_ == way_point_type.cover|| way_point_type_ ==  way_point_type.cover_fullcover?way_point_type.cover_fullcover:way_point_type.fullcover);
				full_cover_direction.Add(Node.direction.WEST);
				real_value -= full_cover_value;

			}

			hit = Physics2D.Raycast (transform.position, Vector2.right, cover_range, full_cover_cast.value);
			if (hit) {
				way_point_type_ = (way_point_type_ == way_point_type.cover|| way_point_type_ ==  way_point_type.cover_fullcover?way_point_type.cover_fullcover:way_point_type.fullcover);
				full_cover_direction.Add(Node.direction.EAST);
				real_value -= full_cover_value;

			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (cover_direction.Count > 0) {
			transform.localRotation = Quaternion.AngleAxis ((float)cover_direction [0], Vector3.forward);
			cover_direction = cover_direction;
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
		case way_point_type.fullcover:{sr.sprite = cover_img;Gizmos.color = Color.yellow;}break;
		case way_point_type.cover_fullcover:{sr.sprite = cover_img;Gizmos.color = Color.cyan;}break;
		case way_point_type.ammo:{sr.sprite = ammo_img;}break;
		}


		if (in_use)
			Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, 0.25f);
	}
}
