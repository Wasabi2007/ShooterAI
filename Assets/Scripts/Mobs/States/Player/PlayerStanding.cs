using UnityEngine;
using System;


public class PlayerStanding : CharState
{
	public PlayerStanding ():base(States.Standing)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.change_color (Color.blue);
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) {
			owner.change_state (new PlayerWalking ());
		}

		if (Input.GetMouseButton (0)) {
			owner.change_state (new PlayerShooting ());
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			owner.change_state (new PlayerInCover ());
		}

		owner.target (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}
}


