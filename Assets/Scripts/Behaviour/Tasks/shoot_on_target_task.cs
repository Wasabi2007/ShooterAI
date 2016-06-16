using UnityEngine;
using System;


public class shoot_on_target_task : Task
{
	private string target_tag_;

	public shoot_on_target_task (string target_tag)
	{
		target_tag_ = target_tag;
	}

	public override void Update (float dt, GameObject go)
	{
		base.Update (dt, go);

		var cc = go.GetComponent<CharController> ();
		cc.movedirection (Vector3.zero);

		cc.changestate (new NPCShooting ());

		var target = GameObject.FindGameObjectWithTag (target_tag_);
		cc.target (target.transform.position);

		parentNode.ChildTerminated (go, this, cc.shoot ());
	}
}


