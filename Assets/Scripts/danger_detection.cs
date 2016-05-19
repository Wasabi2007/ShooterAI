using UnityEngine;
using System.Collections;

public class danger_detection : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.tag == "Bullet")
			gameObject.SendMessageUpwards ("danger", coll.transform.position,SendMessageOptions.DontRequireReceiver);
	}
}
