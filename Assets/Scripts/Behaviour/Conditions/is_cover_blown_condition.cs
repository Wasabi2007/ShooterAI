using UnityEngine;
using System;

public class is_cover_blown_condition : Task
{
	public string tag_;
	public is_cover_blown_condition (string tag)
	{
		tag_ = tag;
	}

	public override void Update (float dt, GameObject go)
	{
		var cc = go.GetComponent<CharController> ();
		var target = GameObject.FindGameObjectWithTag (tag_); //replace this with the Objects function for multiplayer
		var result = cc.is_behind_cover(target.transform.position);

		parentNode.ChildTerminated (go, this, !result);

	}
}


