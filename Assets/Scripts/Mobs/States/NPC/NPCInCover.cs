using UnityEngine;
using System;
using System.Collections;


public class NPCInCover : CharState
{
	public NPCInCover ():base(States.InCover)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.changecolor (Color.gray);
	}
}


