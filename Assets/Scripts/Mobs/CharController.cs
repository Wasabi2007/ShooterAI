﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharController : MonoBehaviour {

	public enum CharType{
		Player,
		NPC,
		NPC_Melee
	}

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

	public float health = 100;
	public float health_max = 100;

	public int clips{
		get{ return (weapons.Length>0?weapons [selected_weapon].clips:0); }
	}
	public int ammo{
		get{ return (weapons.Length>0?weapons [selected_weapon].ammo:0); }
	}
	public int ammo_max{
		get{ return (weapons.Length>0?weapons [selected_weapon].ammo_max:0); }
	}

	public SingleUnityLayer BulletLayer;
	public LayerMask CoverLayer;

	private Weapon[] weapons = new Weapon[0];
	private int selected_weapon = 0;

	public float speed;

	public CharType char_type;
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

	void setup_normal_ki(){
		root = new UtilFail ();
		{
			var selector00 = new Selector ();
			{
				var parallel3 = new ParralelNode ();
				{

					var enough_health = new has_health_condition (health_max * 0.3f);
					var selector = new Selector ();
					{
						var until_fail = new UtilFail (false);
						{
							var sequence = new Sequence ();
							{
								var isnearnode = new is_in_range (follow_range, "Player");
								var iscoverdnode = new is_cover_blown_condition ("Player");
								var until_fail2 = new UtilFail (true);
								var cover = new cover_task ();
								var enough_ammo = new has_ammo_condition (1);

								sequence.AddChild (isnearnode);
								sequence.AddChild (iscoverdnode);
								sequence.AddChild (until_fail2);
								sequence.AddChild (cover);
								sequence.AddChild (enough_ammo);
								{
									var sequence2 = new Sequence ();
									{
										var dangernode = new shoot_on_me_condition ();
										isnearnode = new is_in_range (follow_range, "Player");
										var shoottargetnode = new shoot_on_target_task ("Player");
										enough_ammo = new has_ammo_condition (1);

										sequence2.AddChild (dangernode);
										sequence2.AddChild (isnearnode);
										sequence2.AddChild (shoottargetnode);
										sequence2.AddChild (enough_ammo);
									}
									until_fail2.AddChild (sequence2);
								}

							}
							until_fail.AddChild (sequence);

						}
						selector.AddChild (until_fail);
					}
					{
						var sequence1 = new Sequence ();
						{
							var selector2 = new Selector ();
							{
								var enough_ammo3 = new has_ammo_condition (1);
								var sequence2 = new Sequence ();
								{
									var find_ammo = new find_ammo_task ();
									var until_fail2 = new UtilFail (true);
									{
										var next_waypoint = new go_to_next_waypoint_task ();
										until_fail2.AddChild (next_waypoint);
									}
									sequence2.AddChild (find_ammo);
									sequence2.AddChild (until_fail2);

								}
								selector2.AddChild (enough_ammo3);
								selector2.AddChild (sequence2);
							}
							var sequence3 = new Sequence ();
							{
								var sgtask = new search_and_go_to_cover_task (0.6f);
								var until_fail3 = new UtilFail (true);
								{
									var parralel = new ParralelNode ();
									{
										var sequence4 = new Sequence ();
										{
											var is_way_good = new way_still_good_condition (0.6f);
											var next_waypoint = new go_to_next_waypoint_task ();
											sequence4.AddChild (is_way_good);
											sequence4.AddChild (next_waypoint);
										}
										var selector3 = new Selector ();
										{
											var sequence5 = new Sequence ();
											{
												var isnearnode = new is_in_range (follow_range, "Player");
												var shoottargetnode = new shoot_on_target_task ("Player");
												sequence5.AddChild (isnearnode);
												sequence5.AddChild (shoottargetnode);
											}
											var enough_ammo4 = new has_ammo_condition (1);
											selector3.AddChild (sequence5);
											selector3.AddChild (enough_ammo4);
										}
										parralel.AddChild (sequence4);
										parralel.AddChild (selector3);
									}
									until_fail3.AddChild (parralel);
								}
								sequence3.AddChild (sgtask);
								sequence3.AddChild (until_fail3);
							}
							sequence1.AddChild (selector2);
							sequence1.AddChild (sequence3);
						}
						selector.AddChild (sequence1);
					}
					parallel3.AddChild (enough_health);
					parallel3.AddChild (selector);
				}
				var sequence6 = new Sequence ();
				{
					var find_health = new find_heathpack_task ();
					var until_fail2 = new UtilFail (true);
					{
						var next_waypoint = new go_to_next_waypoint_task ();
						until_fail2.AddChild (next_waypoint);
					}
					sequence6.AddChild (find_health);
					sequence6.AddChild (until_fail2);

				}
				selector00.AddChild (parallel3);
				selector00.AddChild (sequence6);
			}
			root.AddChild (selector00);
		}
	}

	void setup_melee_ki(){
		root = new UtilFail ();
		{
			var parallel = new ParralelNode ();
			{
				var unitlfail4 = new UtilFail(true);
				{
					var sequence01 = new Sequence ();
					{
						var player_find = new find_player_task ();
						var unitlfail = new UtilFail (true);
						{
							var sequence03 = new Sequence ();
							{
								var selector01 = new Selector ();
								{
									var player_moved = new target_moved_condition ();
									var see_player = new has_target_in_eyesigth_contiditon ("Player");
									var next_waypoint = new  go_to_next_waypoint_task ();
									selector01.AddChild (player_moved);
									selector01.AddChild (see_player);
									selector01.AddChild (next_waypoint);
								}
								var not_node = new Not ();
								{
									var see_player2 = new has_target_in_eyesigth_contiditon ("Player");
									not_node.AddChild (see_player2);
								}

								sequence03.AddChild (selector01);
								sequence03.AddChild (not_node);
							}
							unitlfail.AddChild (sequence03);
						}
						var unitlfail2 = new UtilFail (true);
						{
							var sequence02 = new Sequence ();
							{
								var see_player = new has_target_in_eyesigth_contiditon ("Player");
								var charge = new  charge_target_task ("Player");
								sequence02.AddChild (see_player);
								sequence02.AddChild (charge);
							}
							unitlfail2.AddChild (sequence02);
						}
						sequence01.AddChild (unitlfail2);
						sequence01.AddChild (player_find);
						sequence01.AddChild (unitlfail);
					}
					unitlfail4.AddChild (sequence01);
				}
				var unitlfail3 = new UtilFail(true);
				{
					var attack = new shoot_on_target_task("Player");
					unitlfail3.AddChild (attack);
				}
				parallel.AddChild (unitlfail4);
				parallel.AddChild (unitlfail3);
			}
			root.AddChild (parallel);
		}
	}

	// Use this for initialization
	void Start () {
		weapons = GetComponentsInChildren<Weapon> ();
		foreach(var w in weapons){
			w.gameObject.SetActive (false);
		}
		weapons[selected_weapon].gameObject.SetActive (true);

		if(char_type == CharType.NPC){
			setup_normal_ki();
		}
		if(char_type == CharType.NPC_Melee){
			setup_melee_ki();
		}



		rigid = GetComponent<Rigidbody2D> ();
		render = GetComponent<SpriteRenderer> ();
		if (char_type == CharType.NPC|| char_type == CharType.NPC_Melee) {
			if (npc_in_cover)
				current_state = new NPCInCover ();
			else
				current_state = new NPCStanding();
			root.Activate (gameObject);
		} else {
			current_state = new PlayerStanding ();
		}

	}

	public bool is_moveing(){
		return move_direction.sqrMagnitude >= 0.00001 || current_state.state == States.Walking || rigid.velocity.sqrMagnitude >= 0.00001 ;
	}

	public bool is_incover(){
		return current_state.state == States.InCover;
	}

	public States currentstate(){
		return current_state.state;
	}

	public void change_color(Color state){
		render.color = state;
	}

	public void change_state(CharState state){
		current_state = state;
	} 
	
	// Update is called once per frame
	void LateUpdate () {
		current_state.update (this);
		if (char_type == CharType.NPC|| char_type == CharType.NPC_Melee) {
			//Debug.Log (root.get_path ());
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

		float axis_input = Input.GetAxis ("Mouse ScrollWheel");

		if (Mathf.Abs(axis_input) > 0) {
			weapons [selected_weapon].gameObject.SetActive (false);
			selected_weapon = (selected_weapon+(int)Mathf.Sign (axis_input)+weapons.Length)%weapons.Length;
			weapons [selected_weapon].gameObject.SetActive (true);

		}
	}

	public bool is_behind_cover (Vector3 target_position){
		var hit = Physics2D.Linecast (rigid.position, target_position,CoverLayer);

		return !hit;
	}

	public bool has_free_shoot (Vector3 target_position){
		var rel_pos = (Vector2)target_position-rigid.position;
		var hit = Physics2D.Raycast (rigid.position, rel_pos.normalized,rel_pos.magnitude,CoverLayer);
		return !hit;
	}

	public void position(Vector3 pos){
		rigid.position = pos;
		move_direction = Vector2.zero;
	}

	public void movedirection(Vector3 dir,float speed_mod = 1){
		move_direction = Vector3.Normalize(dir)*speed_mod;
	}
		
	public void target (Vector3 target_position){
		var rel_pos = (Vector2)target_position-rigid.position;
		rigid.MoveRotation (Mathf.Atan2(rel_pos.y,rel_pos.x)*Mathf.Rad2Deg);
		rigid.angularVelocity = 0;
	}

	public void walk_to_target(Vector3 target_position){
		rigid.MoveRotation (Vector2.Dot(Vector2.up,(Vector2)target_position-rigid.position));

		walk_progress = 0;
		
		walk_target = target_position;
		start_position = transform.position;
		length = Vector2.Distance (walk_target,start_position);
	}

	public bool shoot(){
		return weapons[selected_weapon].shoot(BulletLayer);
	}

	public void add_clips(int clips){
		foreach(var w in weapons){
			w.add_clips (clips);
		}
	}

	public void heal(float health_boost){
		if (health + health_boost < health_max) {
			health += health_boost;
		} else {
			health = health_max;
		}
	}


	public void apply_damage(float damage){
        if (god_mode) return;
		health -= damage;
		if (health < 0) {
			if(char_type == CharType.NPC|| char_type == CharType.NPC_Melee)
				GameObject.Destroy (gameObject);

			rigid.constraints = RigidbodyConstraints2D.FreezeAll;
			change_state(new PlayerDead());
		}
		danger ();
	}

	public void apply_directed_damage(System.Object[] info){
		float damage = (float)info [0];
		Vector2 dir = ((Vector2)info [1])*-1;
		if (!Physics2D.Raycast (rigid.position, dir, 1.0f, CoverLayer) || current_state.state != States.InCover) {
			apply_damage(damage);
			GameObject.Destroy ((Object)info [2]);
		}
	}

	public void danger(Vector3 pos = new Vector3()){
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

	void OnDestroy(){
		claim_node (null);
	}
}
