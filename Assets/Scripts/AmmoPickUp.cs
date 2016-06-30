using UnityEngine;
using System.Collections;

public class AmmoPickUp : MonoBehaviour {

	public int worth = 1;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
			other.SendMessage ("add_clips", worth, SendMessageOptions.DontRequireReceiver);
			GameObject.Destroy (gameObject);
		}
	}
}
