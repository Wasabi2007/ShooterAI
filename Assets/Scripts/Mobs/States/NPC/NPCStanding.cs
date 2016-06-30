using UnityEngine;
using System;


public class NPCStanding : CharState
{
	public NPCStanding ():base(States.Standing)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.change_color (Color.blue);
		if (owner.is_moveing ()) {
			owner.change_state(new NPCWalking());
		}
	}
}


