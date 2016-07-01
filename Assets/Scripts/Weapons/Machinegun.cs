using UnityEngine;
using System.Collections;

public class Machinegun : Weapon
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override bool shoot (SingleUnityLayer BulletLayer)
	{
		if (base.shoot (BulletLayer)){
			GameObject go =	GameObject.Instantiate<GameObject> (bullet.gameObject);
			go.layer = BulletLayer.LayerIndex;
			go.transform.position = transform.position;
			var bul = go.GetComponent<Bullet> ();
			bul.damage = bullet_damage;
			bul.speed = bullet_speed;
			bul.dir = Quaternion.AngleAxis (transform.rotation.eulerAngles.z, Vector3.forward) * Vector2.right;
		}

		return ammo > 0;
	}
}

