using UnityEngine;
using System;


public class NPCStanding : CharState
{
	public NPCStanding ():base(States.Standing)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.changecolor (Color.blue);
		if (owner.ismoveing ()) {
			owner.changestate(new NPCWalking());
		}
	}
}


