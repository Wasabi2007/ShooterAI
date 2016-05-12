﻿using UnityEngine;
using System;

public class search_and_go_to_cover_task:Task
{
	public search_and_go_to_cover_task ()
	{
	}

	public override void Activate (GameObject go)
	{
		base.Activate (go);
	}

	public override void Update(float dt, GameObject go){
		CharController cc = go.GetComponent<CharController>();
		if(cc.currentstate() != States.Walking)
			cc.changestate (new NPCWalking ());
		Node start_node = cc.nav_path.get_nearest_node (go.transform.position);

		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Node end_node = cc.nav_path.get_nearest_cover_node (go.transform.position,player.transform.position);

		if (cc.claimend_node && Vector3.Distance (go.transform.position, cc.claimend_node.transform.position) < 0.001) {
			ChildTerminated (go,this, true);
			return;
		}
			

		if (cc.claimend_node != end_node) {
			cc.claim_node (end_node);
			cc.path.Clear ();
			var path = cc.nav_path.get_path (start_node, end_node);
			//foreach(var n in path)
				//cc.path.Enqueue(n);
		}
		if (cc.path.Count <= 0)
			return;	

		if (Vector3.Distance (go.transform.position, cc.path.Peek().transform.position) < 0.001) {
			Debug.Log ("Peak distance: "+Vector3.Distance (go.transform.position, cc.path.Peek().transform.position));
			cc.path.Dequeue ();
		} 
		cc.walktarget (cc.path.Peek().transform.position);
	}
}


