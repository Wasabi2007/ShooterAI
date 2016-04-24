using UnityEngine;
using System;


public class NPCWalking : CharState
{
	public NPCWalking ():base(States.Walking)
	{
	}

	public override void update (CharController owner)
	{
		owner.changecolor (Color.green);
		if (!owner.ismoveing ()) {
			owner.changestate(new NPCStanding());
		}
	}
}


