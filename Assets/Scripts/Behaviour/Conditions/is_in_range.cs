using UnityEngine;
using System;


public class is_in_range : Task
{
	private float range_;
	private string tag_;

	public is_in_range (float range, string tag)
	{
		range_ = range;
		tag_ = tag;
	}

	public override void Update (float dt, GameObject go)
	{
		base.Update (dt, go);

		var player = GameObject.FindGameObjectWithTag (tag_);
		if (Vector3.Distance (go.transform.position, player.transform.position) < range_) {
			parentNode.ChildTerminated (go, this, true);
		} else {
			parentNode.ChildTerminated (go, this, false);
		}

	}
}

