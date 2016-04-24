using UnityEngine;
using System;
using System.Collections;


public class NPCDucking : CharState
{
	public NPCDucking ():base(States.Ducking)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.changecolor (Color.gray);
	}
}


