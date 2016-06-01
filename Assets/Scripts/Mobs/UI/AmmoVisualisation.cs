using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class AmmoVisualisation : MonoBehaviour {

	public CharController char_controller;
	public Sprite ammo_sprite;
	public int cols;

	private float spritescaleing;
	private int rows;

	private List<UnityEngine.UI.Image> ammos = new List<UnityEngine.UI.Image>();

	// Use this for initialization
	void Start () {
		Debug.Log ("Miep");
		if (!char_controller || !ammo_sprite)
			return;

		RectTransform rt = GetComponent<RectTransform> ();

		var zwich = (rt.rect.width / cols);
		Debug.Log ("zwich"+zwich);
		spritescaleing = zwich/ammo_sprite.rect.width;
		rows = Mathf.FloorToInt (rt.rect.height/(ammo_sprite.rect.height*spritescaleing));

		Debug.Log ("rt.rect "+rt.rect);
		Debug.Log ("ammo_sprite.rect "+ammo_sprite.rect);
		Debug.Log ("spritescaleing "+spritescaleing);
		Debug.Log ("rows "+rows);
		Debug.Log ("cols "+cols);
		Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>();

		foreach(Transform childObjects in allTransforms){
			if(gameObject.transform.IsChildOf(childObjects.transform) == false)
				DestroyImmediate(childObjects.gameObject);
		}
		ammos.Clear ();

		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < cols; ++x) {
				GameObject go = new GameObject();
				go.transform.parent = transform;
				UnityEngine.UI.Image r = go.AddComponent<UnityEngine.UI.Image> ();
				r.sprite = ammo_sprite;
				go.transform.localScale = Vector3.one * spritescaleing;
				go.transform.localPosition = new Vector2 (rt.rect.width - (x+1) * ammo_sprite.rect.width * spritescaleing, y * ammo_sprite.rect.height * spritescaleing);
				ammos.Add (r);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!char_controller || !ammo_sprite)
				return;



		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < cols; ++x) {
				ammos [y*cols+x].color = (y*cols+x<char_controller.Ammo ? Color.white:Color.grey);
			}
		}
	}
}
