using UnityEngine;
using System;


public class PlayerStanding : CharState
{
	public PlayerStanding ():base(States.Standing)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.changecolor (Color.blue);
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) {
			owner.changestate (new PlayerWalking ());
		}

		if (Input.GetMouseButton (0)) {
			owner.changestate (new PlayerShooting ());
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			owner.changestate (new PlayerInCover ());
		}

		owner.target (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}
}


