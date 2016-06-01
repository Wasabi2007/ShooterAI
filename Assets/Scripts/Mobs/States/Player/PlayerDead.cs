﻿using UnityEngine;
using System;


public class PlayerDead : CharState
{
	public PlayerDead ():base(States.Dead)
	{
	}
	

	public override void update (CharController owner)
	{
		owner.changecolor (Color.clear);
	}
}

