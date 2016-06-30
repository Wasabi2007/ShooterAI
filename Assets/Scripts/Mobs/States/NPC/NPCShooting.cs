using UnityEngine;
using System;


public class NPCShooting : CharState
{
	public NPCShooting ():base(States.Shooting)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.change_color (Color.red);
		if (owner.is_moveing ()) {
			owner.change_state(new NPCWalking());
		}
	}
}


