using UnityEngine;
using System;


public class shoot_on_me_condition : Task
{

	public float time;

	public shoot_on_me_condition ()
	{
		time = 1;
	}

	public override void Update (float dt, GameObject go)
	{
		base.Update (dt, go);

		var cc = go.GetComponent<CharController> ();

		var result = Time.time - cc.danger_time > time;

		parentNode.ChildTerminated (go, this, result);
	}
}


