using UnityEngine;
using System.Collections;

public class charge_target_task : Task
{

	public string tag_;
	public charge_target_task (string tag)
	{
		tag_ = tag;
	}

	public override void Update (float dt, GameObject go)
	{
		var cc = go.GetComponent<CharController> ();
		var target = GameObject.FindGameObjectWithTag (tag_); //replace this with the Objects function for multiplayer
		cc.walk_to_target(target.transform.position);

		parentNode.ChildTerminated (go, this, true);

	}
}

