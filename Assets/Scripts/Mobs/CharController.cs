using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharController : MonoBehaviour {

    //Debug
    public bool god_mode;
    public void set_godmode(bool value)
    {
        god_mode = value;
    }

    public bool show_path = false;
    public void set_show_path(bool value)
    {
        show_path = value;
    }

    public string current_behavoiur()
    {
        return root.get_path();
    }

	public float Health = 100;
	public float MaxHealth = 100;

	public Bullet bullet;
	public SingleUnityLayer BulletLayer;
	public LayerMask CoverLayer;


	//todo move to WeaponComponent
	public int Ammo;
	public int AmmoMax;
	public int Clips;
	public float bullet_speed = 10;
	public float bullet_firerate = 0.1f;
	public float reload_speed = 1;
	private float last_shoot;
	private float reload_time;

	public float speed;

	public bool NPC;
	public bool npc_in_cover = false;//Debug
	public AStern nav_path;
	public float follow_range = 100;

	private Vector3 danger_pos_;
	public Vector3 danger_pos{
		get{ return danger_pos_; }
	}

	private float danger_time_;
	public float danger_time{
		get{ return danger_time_; }
	}

	[HideInInspector]
	public Node claimend_node;

	private Queue<Node> path = new Queue<Node>();
	Node[] path_ = new Node[0];
	public Queue<Node> Path{
		set{ path = value;
			 path_ = this.path.ToArray();}
		get{ return path; }
	}

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
		var sgtask = new search_and_go_to_cover_task (0.6f);

		var until_fail = new UtilFail ();

		var isnearnode = new is_in_range (follow_range, "Player");
		var iscoverdnode = new is_cover_blown_condition ("Player");
		var until_fail2 = new UtilFail (true);
		var cover = new cover_task ();

		var sequence2 = new Sequence ();
		var dangernode = new shoot_on_me_condition ();
		var isnearnode2 = new is_in_range (follow_range, "Player");
		var shoottargetnode = new shoot_on_target_task ("Player");

		sequence2.AddChild (dangernode);
		sequence2.AddChild (isnearnode2);
		sequence2.AddChild (shoottargetnode);

		until_fail2.AddChild (sequence2);

		sequence.AddChild (isnearnode);
		sequence.AddChild (iscoverdnode);
		sequence.AddChild (until_fail2);
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
		if (last_shoot + bullet_firerate < Time.time && Ammo > 0) {
			last_shoot = Time.time;
			GameObject go =	GameObject.Instantiate<GameObject> (bullet.gameObject);
			go.layer = BulletLayer.LayerIndex;
			go.transform.position = transform.position;
			var bul = go.GetComponent<Bullet> ();
			bul.damage = 10;
			bul.speed = bullet_speed;
			bul.dir = Quaternion.AngleAxis (rigid.rotation, Vector3.forward) * Vector2.right;
			Ammo--;
			if(Ammo == 0)
				reload_time = Time.time + reload_speed;
		}

		if (Ammo == 0 && reload_time < Time.time) {
			Ammo = AmmoMax;
		}
	}

	public void applydamage(float damage){
        if (god_mode) return;
		Health -= damage;
		if (Health < 0) {
			if(NPC)
				GameObject.Destroy (gameObject);

			rigid.constraints = RigidbodyConstraints2D.FreezeAll;
			changestate(new PlayerDead());
		}
	}

	public void applydirecteddamage(System.Object[] info){
		float damage = (float)info [0];
		Vector2 dir = ((Vector2)info [1])*-1;
		if (!Physics2D.Raycast (rigid.position, dir, 1.0f, CoverLayer) || current_state.state != States.InCover) {
			applydamage(damage);
			GameObject.Destroy ((Object)info [2]);
		}
	}

	public void danger(Vector3 pos){
		danger_pos_ = pos;
		danger_time_ = Time.time;
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawLine (transform.position, transform.position+move_direction);

        if (path_.Length > 0 && show_path)
        {
			Gizmos.color = Color.red;
			Node old_n = path_ [0];
			foreach (Node n in path_) {
				Gizmos.DrawLine (old_n.transform.position, n.transform.position);
                old_n = n;
			}
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawLine (transform.position, walk_target);

	}
}
