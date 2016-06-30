using UnityEngine;
using System.Collections;

public class HeathPickUp : MonoBehaviour {

	public float worth = 1;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
			other.SendMessage ("heal", worth, SendMessageOptions.DontRequireReceiver);
			GameObject.Destroy (gameObject);
		}
	}
}
