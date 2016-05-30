using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GridSnapp : MonoBehaviour {

	public float x_grid_size = 1;
	public float y_grid_size = 1;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Transform[] gos = transform.GetComponentsInChildren<Transform> ();

		foreach (Transform t in gos) {
			var pos = t.position;
			pos.x = Mathf.RoundToInt(t.position.x / x_grid_size) * x_grid_size;
			pos.y = Mathf.RoundToInt(t.position.y / y_grid_size) * y_grid_size;
			t.position = pos;
		}
	}
}
