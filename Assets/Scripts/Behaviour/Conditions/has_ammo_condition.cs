using System;
using UnityEngine;

public class has_ammo_condition: Task
{
	int not_less_than;

	public has_ammo_condition (int not_less_than)
	{
		this.not_less_than = not_less_than;
	}

	public override void Update (float dt, GameObject go)
	{
		base.Update (dt, go);

		var cc = go.GetComponent<CharController> ();

		parentNode.ChildTerminated (go, this, cc.Clips >= not_less_than);
	}
}


