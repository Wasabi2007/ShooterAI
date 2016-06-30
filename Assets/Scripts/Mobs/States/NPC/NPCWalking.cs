using UnityEngine;
using System;


public class NPCWalking : CharState
{
	public NPCWalking ():base(States.Walking)
	{
	}

	public override void update (CharController owner)
	{
		owner.change_color (Color.green);
		if (!owner.is_moveing ()) {
			owner.change_state(new NPCStanding());
		}
	}
}


