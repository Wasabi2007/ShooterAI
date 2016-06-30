﻿using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class PlayerDead : CharState
{
	public PlayerDead ():base(States.Dead)
	{
	}
	

	public override void update (CharController owner)
	{
		owner.change_color (Color.clear);
		SceneManager.LoadScene ("End");
	}
}


