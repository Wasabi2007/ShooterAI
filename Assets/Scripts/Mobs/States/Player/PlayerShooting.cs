using UnityEngine;
using System;


public class PlayerShooting : CharState
{
	public PlayerShooting ():base(States.Shooting)
	{
	}
		
	public override void update (CharController owner)
	{
		owner.changecolor (Color.red);

		if (Input.GetMouseButton (0)) {
			owner.shoot ();

			Vector2 dir = new Vector2 (0, 0);
			if (Input.GetKey (KeyCode.W)) {
				dir += Vector2.up;
			}
			if (Input.GetKey (KeyCode.S)) {
				dir -= Vector2.up;
			}
			if (Input.GetKey (KeyCode.A)) {
				dir += Vector2.left;
			}
			if (Input.GetKey (KeyCode.D)) {
				dir -= Vector2.left;
			}

			owner.movedirection (dir);

		} else if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) {
			owner.changestate (new PlayerWalking ());
		} else if (Input.GetKeyDown (KeyCode.Q)) {
			owner.changestate (new PlayerDucking ());
		} else {
			owner.changestate (new PlayerStanding ());	
		}
		owner.target (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}
}


