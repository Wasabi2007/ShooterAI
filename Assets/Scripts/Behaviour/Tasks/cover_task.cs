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

		cc.movedirection (Vector3.zero);

		if(!cc.is_incover())
			cc.change_state (new NPCInCover ());

		parentNode.ChildTerminated (go, this, true);
	}
}


