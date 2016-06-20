using UnityEngine;
using System;
using System.Collections.Generic;

public class find_ammo_task : Task
{
	public find_ammo_task ()
	{
	}

	public override void Activate (GameObject go)
	{
		base.Activate (go);

		CharController cc = go.GetComponent<CharController>();
		cc.claim_node (null);
		cc.changestate (new NPCWalking ());
		GameObject player = GameObject.FindGameObjectWithTag ("Player");


		Node start_node = cc.nav_path.get_nearest_node (go.transform.position);

		Node end_node = cc.nav_path.get_nearest_ammo_node (go.transform.position);

		cc.Path.Clear ();
		Queue<Node> path_ = new Queue<Node>();

		if (end_node != null) {
			cc.claim_node (end_node);
			var path = cc.nav_path.get_path (start_node, end_node,player.transform.position,1000);
			foreach (var n in path)
				path_.Enqueue (n);

			//cc.walktarget (cc.path.Peek ().transform.position);
		}
		cc.Path = path_;
	}

	public override void Update(float dt, GameObject go){
		base.Update (dt, go);

		CharController cc = go.GetComponent<CharController>();

		if (!cc.claimend_node && cc.Path.Count <= 0){
			Debug.Log ("find_ammo_task Terminate false");
			parentNode.ChildTerminated (go,this, false);
			return;
		}

		if (cc.claimend_node && cc.Path.Count <= 0) {
			Debug.Log ("find_ammo_task Terminate true");
			parentNode.ChildTerminated (go,this, true);
			return;
		}



		cc.movedirection (cc.Path.Peek().transform.position-cc.transform.position);
		cc.target (cc.Path.Peek ().transform.position);

		if (Vector3.Distance (go.transform.position, cc.Path.Peek().transform.position) < cc.speed*Time.deltaTime) {
			cc.movedirection (cc.Path.Peek().transform.position-cc.transform.position,Vector3.Distance (go.transform.position, cc.Path.Peek().transform.position));
			cc.Path.Dequeue ();
			//cc.walktarget (cc.path.Peek ().transform.position);
		} 

	}
}


