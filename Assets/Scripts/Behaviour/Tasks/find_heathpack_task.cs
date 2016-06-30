using UnityEngine;
using System;
using System.Collections.Generic;


public class find_heathpack_task: Task
{
	public find_heathpack_task ()
	{
	}

	public override void Activate (GameObject go)
	{
		base.Activate (go);
	}

	public override void Update(float dt, GameObject go){
		base.Update (dt, go);

		CharController cc = go.GetComponent<CharController>();
		cc.claim_node (null);
		GameObject player = GameObject.FindGameObjectWithTag ("Player");


		Node start_node = cc.nav_path.get_nearest_node (go.transform.position);

		Node end_node = cc.nav_path.get_nearest_heath_spawn_node (go.transform.position);

		cc.Path.Clear ();
		Queue<Node> path_ = new Queue<Node>();

		if (end_node != null) {
			cc.claim_node (end_node);
			var path = cc.nav_path.get_path (start_node, end_node,player.transform.position,100000);
			foreach (var n in path)
				path_.Enqueue (n);

			//cc.walktarget (cc.path.Peek ().transform.position);
		}
		cc.Path = path_;


		if (!end_node){
			//Debug.Log ("search_and_go_to_cover_task Terminate false");
			parentNode.ChildTerminated (go,this, false);
		}else {
			//Debug.Log ("search_and_go_to_cover_task Terminate true");
			parentNode.ChildTerminated (go,this, true);
		}

	}
}


