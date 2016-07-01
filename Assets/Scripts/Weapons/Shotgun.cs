using UnityEngine;
using System.Collections;

public class Shotgun : Weapon
{
	public float spreat_size;
	public int pallets;

	public override bool shoot (SingleUnityLayer BulletLayer)
	{
		if (base.shoot (BulletLayer)){
			for (int i = 0; i < pallets; ++i) {
				GameObject go =	GameObject.Instantiate<GameObject> (bullet.gameObject);
				go.layer = BulletLayer.LayerIndex;
				go.transform.position = transform.position;
				var bul = go.GetComponent<Bullet> ();
				bul.damage = 10;
				bul.speed = bullet_speed;

				var variation = Random.insideUnitCircle*spreat_size;

				bul.dir = (Vector2)(Quaternion.AngleAxis (transform.rotation.eulerAngles.z, Vector3.forward) * Vector2.right) + variation;
			}
		}

		return ammo > 0;
	}
}

