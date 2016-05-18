using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharController : MonoBehaviour {


	public float Health = 100;
	public float MaxHealth = 100;

	public Bullet bullet;
	public SingleUnityLayer BulletLayer;
	public LayerMask CoverLayer;

	public int Ammo;
	public int AmmoMax;
	public int Clips;

	public float speed;

	public bool NPC;
	public bool npc_in_cover = false;//Debug
	public AStern nav_path;
	public float follow_range = 100;

	[HideInInspector]
	public Node claimend_node;
	[HideInInspector]
	public Queue<Node> path = new Queue<Node>();

	private CharState current_state;

	private Vector3 walk_target;
	private Vector3 start_position;
	private float walk_progress = 1;
	private float length = 1;

	private Vector3 move_direction = Vector3.zero;

	private Rigidbody2D rigid;
	private SpriteRenderer render;

	private BehaviourNode root;

	public void claim_node(Node node){
		if (claimend_node)
			claimend_node.in_use = false;
		if(node)
			node.in_use = true;
		claimend_node = node;
	}

	// Use this for initialization
	void Start () {
		root = new UtilFail ();
		var selector = new Selector ();
		var sequence = new Sequence ();
		var sgtask = new search_and_go_to_cover_task ();

		var until_fail = new UtilFail ();

		var isnearnode = new is_in_range (follow_range, "Player");
		var iscoverdnode = new is_cover_blown_condition ("Player");
		var cover = new cover_task ();

		sequence.AddChild (isnearnode);
		sequence.AddChild (iscoverdnode);
		sequence.AddChild (cover);

		until_fail.AddChild (sequence);

		selector.AddChild (until_fail);
		selector.AddChild (sgtask);
		root.AddChild (selector);


		rigid = GetComponent<Rigidbody2D> ();
		render = GetComponent<SpriteRenderer> ();
		//rigid.constraints = RigidbodyConstraints2D.FreezeAll;
		if (NPC) {
			if (npc_in_cover)
				current_state = new NPCInCover ();
			else
				current_state = new NPCStanding();

			root.Activate (gameObject);
		} else {
			current_state = new PlayerStanding ();
		}

	}

	public bool ismoveing(){
		return move_direction.sqrMagnitude >= 0.00001 || current_state.state == States.Walking || rigid.velocity.sqrMagnitude >= 0.00001 ;
	}

	public bool isincover(){
		return current_state.state == States.InCover;
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
		if (NPC) {
			if (!root.IsActive)
				root.Activate (gameObject);
			root.Update (0, gameObject);
		}
		
		rigid.MovePosition (rigid.position+(Vector2)(move_direction*speed*Time.deltaTime));

		if (walk_progress < 1.0f) {
			walk_progress += (Time.deltaTime * speed)/length;
			rigid.MovePosition(Vector3.Lerp(start_position,walk_target,Mathf.Clamp01(walk_progress)));
			target (walk_target);
		}

	}

	public bool is_behind_cover (Vector3 target_position){
		var rel_pos = (Vector2)target_position-rigid.position;
		var hit = Physics2D.Raycast (rigid.position, rel_pos.normalized,float.MaxValue,CoverLayer);

		return !hit;
	}

	public bool has_free_shoot (Vector3 target_position){
		var rel_pos = (Vector2)target_position-rigid.position;
		var hit = Physics2D.Raycast (rigid.position, rel_pos.normalized);
		return !hit;
	}

	public void movedirection(Vector3 dir,float speed_mod = 1){
		move_direction = Vector3.Normalize(dir)*speed_mod;
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
		if (!Physics2D.Raycast (rigid.position, dir, float.MaxValue, CoverLayer) || current_state.state != States.InCover) {
			Health -= damage;
			GameObject.Destroy ((Object)info [2]);
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, transform.position+move_direction);

		Gizmos.color = Color.blue;

		Gizmos.DrawLine (transform.position, walk_target);

		int count = this.path.Count;
		Node[] path_ = new Node[count];
		this.path.CopyTo (path_,0);

		if (path_.Length <= 0)
			return;


		Node old_n = path_[0];
		foreach (Node n in path_) {
			Gizmos.DrawLine (old_n.transform.position, n.transform.position);
		}

	}
}
