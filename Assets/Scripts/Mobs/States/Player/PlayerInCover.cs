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
		owner.change_color (Color.gray);
		owner.movedirection (Vector2.zero);
		if (Input.GetKeyDown (KeyCode.Q)) {
			owner.change_state (new PlayerStanding ());
		}
		owner.target (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}
}


