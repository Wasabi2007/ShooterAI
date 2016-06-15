using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {

	public GameObject item;
	public float respawn_time;

	private GameObject spawn;
	private float next_spawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (spawn) {
			next_spawn = Time.time + respawn_time;
		}

		if (spawn == null&& next_spawn < Time.time) {
			spawn = (GameObject)GameObject.Instantiate (item, transform.position, Quaternion.identity);
		}
	
	}
}
