using UnityEngine;
using System;


public class NPCShooting : CharState
{
	public NPCShooting ():base(States.Shooting)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.changecolor (Color.red);
		if (owner.ismoveing ()) {
			owner.changestate(new NPCWalking());
		}
	}
}


