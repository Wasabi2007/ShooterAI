using UnityEngine;
using System.Collections;

public class has_target_in_eyesigth_contiditon : Task
{

	public string tag_;
	public has_target_in_eyesigth_contiditon (string tag)
	{
		tag_ = tag;
	}

	public override void Update (float dt, GameObject go)
	{
		var cc = go.GetComponent<CharController> ();
		var target = GameObject.FindGameObjectWithTag (tag_); //replace this with the Objects function for multiplayer
		var result = cc.has_free_shoot(target.transform.position);

		parentNode.ChildTerminated (go, this, !result);

	}
}

