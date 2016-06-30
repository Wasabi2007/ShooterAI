using System;
using UnityEngine;

public class has_health_condition: Task
{
	float not_less_than;
	public has_health_condition (float not_less_than)
	{
		this.not_less_than = not_less_than;
	}

	public override void Update (float dt, GameObject go)
	{
		base.Update (dt, go);

		var cc = go.GetComponent<CharController> ();

		parentNode.ChildTerminated (go, this, cc.health >= not_less_than);
	}
}


