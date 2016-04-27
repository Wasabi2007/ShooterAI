using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharController : MonoBehaviour {


	public float Health = 100;
	public float MaxHealth = 100;

	public Bullet bullet;
	public SingleUnityLayer BulletLayer;
	public LayerMask DuckingLayer;

	public int Ammo;
	public int AmmoMax;
	public int Clips;

	public float speed;

	public bool NPC;
	public bool npc_ducking = false;//Debug

	private CharState current_state;

	private Vector3 walk_target;
	private Vector3 start_position;
	private float walk_progress = 1;
	private float length = 1;

	private Vector3 move_direction = Vector3.zero;

	private Rigidbody2D rigid;
	private SpriteRenderer render;


	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		render = GetComponent<SpriteRenderer> ();
		//rigid.constraints = RigidbodyConstraints2D.FreezeAll;
		if (NPC) {
			if (npc_ducking)
				current_state = new NPCDucking ();
			else
				current_state = new NPCStanding();
		} else {
			current_state = new PlayerStanding ();
		}
	}

	public bool ismoveing(){
		return move_direction.sqrMagnitude >= 0.00001 || current_state.state == States.Walking || rigid.velocity.sqrMagnitude >= 0.00001 ;
	}

	public bool isducking(){
		return current_state.state == States.Ducking;
	}

	public States currentstate(){
		return current_state.state;
	}

	public void changecolor(Color state){
		render.color = state;
	}

	public void changestate(CharState state){
		current_state = state;
	} 
	
	// Update is called once per frame
	void Update () {
		current_state.update (this);
		rigid.MovePosition (rigid.position+(Vector2)(move_direction*speed*Time.deltaTime));

		if (walk_progress < 1.0f) {
			walk_progress += (Time.deltaTime * speed)/length;
			rigid.MovePosition(Vector3.Slerp(start_position,walk_target,Mathf.Clamp01(walk_progress)));
		}

	}

	public bool is_behind_duckspot (Vector3 target_position){
		var rel_pos = (Vector2)target_position-rigid.position;
		var hit = Physics2D.Raycast (rigid.position, rel_pos.normalized,float.MaxValue,DuckingLayer);
		return !hit;
	}

	public bool has_free_shoot (Vector3 target_position){
		var rel_pos = (Vector2)target_position-rigid.position;
		var hit = Physics2D.Raycast (rigid.position, rel_pos.normalized);
		return !hit;
	}

	public void movedirection(Vector3 dir){
		move_direction = Vector3.Normalize(dir);
	}
		
	public void target (Vector3 target_position){
		var rel_pos = (Vector2)target_position-rigid.position;
		rigid.MoveRotation (Mathf.Atan2(rel_pos.y,rel_pos.x)*Mathf.Rad2Deg);
	}

	public void walktarget(Vector3 target_position){
		rigid.MoveRotation (Vector2.Dot(Vector2.up,(Vector2)target_position-rigid.position));
		walk_progress = 0;
		walk_target = target_position;
		start_position = transform.position;
		length = Vector2.Distance (walk_target,start_position);
	}

	public void shoot(){
		GameObject go =	GameObject.Instantiate<GameObject> (bullet.gameObject);
		go.layer = BulletLayer.LayerIndex;
		go.transform.position = transform.position;
		var bul = go.GetComponent<Bullet> ();
		bul.damage = 10;
		bul.speed = 10;
		bul.dir = Quaternion.AngleAxis (rigid.rotation, Vector3.forward)*Vector2.right;
	}

	public void applydamage(float damage){
		Health -= damage;
	}

	public void applydirecteddamage(System.Object[] info){
		float damage = (float)info [0];
		Vector2 dir = ((Vector2)info [1])*-1;
		if (!Physics2D.Raycast (rigid.position, dir, float.MaxValue, DuckingLayer) || current_state.state != States.Ducking) {
			Health -= damage;
			GameObject.Destroy ((Object)info [2]);
		}
	}
}
