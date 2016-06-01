using UnityEngine;
using System.Collections;

public class HealthScaling : MonoBehaviour {

	public CharController char_controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 scale = transform.localScale;
		scale.x = (char_controller.Health / char_controller.MaxHealth);
		transform.localScale = scale;
	}
}
