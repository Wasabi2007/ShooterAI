using UnityEngine;
using System;
using System.Collections;


public class PlayerInCover : CharState
{
	public PlayerInCover ():base(States.InCover)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.changecolor (Color.gray);
		owner.movedirection (Vector2.zero);
		if (Input.GetKeyDown (KeyCode.Q)) {
			owner.changestate (new PlayerStanding ());
		}
		owner.target (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}
}


