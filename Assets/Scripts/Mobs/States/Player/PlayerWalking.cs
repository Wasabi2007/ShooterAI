using UnityEngine;
using System;


public class PlayerWalking : CharState
{
	public PlayerWalking ():base(States.Walking)
	{
	}

	public override void update (CharController owner)
	{
		owner.change_color (Color.green);
		Vector2 dir = new Vector2(0,0);
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

		if (!(Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.Q))) {
			owner.change_state (new PlayerStanding());
		}

		if (Input.GetMouseButton (0)) {
			owner.change_state (new PlayerShooting ());
		}

		if (Input.GetKey (KeyCode.Q)) {
			owner.change_state (new PlayerInCover ());
		} else {
			owner.movedirection (dir);
		}
		owner.target (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}
}


