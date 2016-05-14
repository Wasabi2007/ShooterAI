using UnityEngine;
using System;

public class search_and_go_to_cover_task:Task
{
	public search_and_go_to_cover_task ()
	{
	}

	public override void Activate (GameObject go)
	{
		base.Activate (go);

		Debug.Log ("search_and_go_to_cover_task Activ");


		CharController cc = go.GetComponent<CharController>();
		cc.claim_node (null);
		cc.changestate (new NPCWalking ());

		Node start_node = cc.nav_path.get_nearest_node (go.transform.position);

		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Node end_node = cc.nav_path.get_nearest_cover_node (go.transform.position,player.transform.position);

		cc.path.Clear ();

		if (end_node != null) {
			cc.claim_node (end_node);
			var path = cc.nav_path.get_path (start_node, end_node);
			foreach (var n in path)
				cc.path.Enqueue (n);

			//cc.walktarget (cc.path.Peek ().transform.position);
		}

		if (cc.path.Count <= 0){
			Debug.Log ("search_and_go_to_cover_task Terminate false");
			parentNode.ChildTerminated (go,this, false);
		}
	}

	public override void Update(float dt, GameObject go){
		base.Update (dt, go);

		CharController cc = go.GetComponent<CharController>();

		if (cc.claimend_node && cc.path.Count <= 0) {
			Debug.Log ("search_and_go_to_cover_task Terminate true");
			parentNode.ChildTerminated (go,this, true);
			return;
		}



		cc.movedirection (cc.path.Peek().transform.position-cc.transform.position);
		cc.target (cc.path.Peek ().transform.position);

		if (Vector3.Distance (go.transform.position, cc.path.Peek().transform.position) < cc.speed*Time.deltaTime) {
			cc.movedirection (cc.path.Peek().transform.position-cc.transform.position,Vector3.Distance (go.transform.position, cc.path.Peek().transform.position));
			cc.path.Dequeue ();
			//cc.walktarget (cc.path.Peek ().transform.position);
		} 

	}
}


