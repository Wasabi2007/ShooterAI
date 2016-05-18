using UnityEngine;
using System;
 
public class cover_task : Task
{
	public cover_task ()
	{
	}

	public override void Update (float dt, GameObject go)
	{
		var cc = go.GetComponent<CharController> ();

		if(!cc.isincover())
			cc.changestate (new NPCInCover ());

		parentNode.ChildTerminated (go, this, true);
	}
}


