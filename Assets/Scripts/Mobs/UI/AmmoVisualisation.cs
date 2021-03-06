﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class AmmoVisualisation : MonoBehaviour {

	public CharController char_controller;
	public Sprite ammo_sprite;
	public int cols;

	private float spritescaleing;
	private int rows;
	public int calc_cols;


	private List<UnityEngine.UI.Image> ammos = new List<UnityEngine.UI.Image>();

	// Use this for initialization
	void Start () {
		if (!char_controller || !ammo_sprite)
			return;

		RectTransform rt = GetComponent<RectTransform> ();

		calc_cols = cols;

		if (char_controller.ammo_max < cols) {
			calc_cols = char_controller.ammo_max;
			rows = 1;
			var zwich = (rt.rect.width / calc_cols);
			spritescaleing = zwich / ammo_sprite.rect.width;
		} else {

			var zwich = (rt.rect.width / calc_cols);
			spritescaleing = zwich / ammo_sprite.rect.width;
			rows = Mathf.CeilToInt (rt.rect.height / (ammo_sprite.rect.height * spritescaleing));
			if (rows * calc_cols > char_controller.ammo_max)
				rows--;
		}

		Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>();

		foreach(Transform childObjects in allTransforms){
			if(gameObject.transform.IsChildOf(childObjects.transform) == false)
				DestroyImmediate(childObjects.gameObject);
		}
		ammos.Clear ();

		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < calc_cols; ++x) {
				GameObject go = new GameObject();
				go.transform.parent = transform;
				UnityEngine.UI.Image r = go.AddComponent<UnityEngine.UI.Image> ();
				r.sprite = ammo_sprite;
				go.transform.localScale = Vector3.one * spritescaleing;
				go.transform.localPosition = new Vector2 ( - (x+1) * ammo_sprite.rect.width * spritescaleing, y * ammo_sprite.rect.height * spritescaleing);
				ammos.Add (r);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (ammos.Count != char_controller.ammo_max) {
			Start ();
		}

		if (!char_controller || !ammo_sprite)
				return;



		for (int y = 0; y < rows; ++y) {
			for (int x = 0; x < calc_cols; ++x) {
				ammos [y*cols+x].color = (y*cols+x<char_controller.ammo ? Color.white:Color.grey);
			}
		}
	}
}
