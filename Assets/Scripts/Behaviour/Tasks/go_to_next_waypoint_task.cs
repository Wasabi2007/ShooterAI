using System;
using UnityEngine;

public class go_to_next_waypoint_task:Task
{
	public go_to_next_waypoint_task ()
	{
	}

	public override void Update(float dt, GameObject go){
		base.Update (dt, go);

		CharController cc = go.GetComponent<CharController>();
		if(!cc.is_moveing()&&cc.currentstate() != States.Shooting)
			cc.change_state (new NPCWalking ());
		else if(cc.currentstate() == States.Walking)
			cc.target (cc.Path.Peek ().transform.position);

		cc.movedirection (cc.Path.Peek().transform.position-cc.transform.position);
		//cc.target (cc.Path.Peek ().transform.position);

		if (Vector3.Distance (go.transform.position, cc.Path.Peek().transform.position) < cc.speed*Time.deltaTime) {
			cc.position (cc.Path.Peek ().transform.position);
			//cc.movedirection (cc.Path.Peek().transform.position-cc.transform.position,Vector3.Distance (go.transform.position, cc.Path.Peek().transform.position));
			cc.Path.Dequeue ();
			//cc.walktarget (cc.path.Peek ().transform.position);
		} 
			
		if (cc.Path.Count > 0){
			Debug.Log ("Hey da ist noch weg.");
			parentNode.ChildTerminated (go,this, true);
		} else {
			Debug.Log ("Warum ist der Weg weg.");
			parentNode.ChildTerminated (go,this, false);
		}

	}
}


