using UnityEngine;
using System.Collections;

public class Spreatgun : Weapon
{

	public float angle;
	public int lines;

	public override bool shoot (SingleUnityLayer BulletLayer)
	{
		if (base.shoot (BulletLayer)){
			for (float i = transform.rotation.eulerAngles.z-(angle/2.0f); i < transform.rotation.eulerAngles.z+(angle/2.0f); i+=(angle/lines)) {
				GameObject go =	GameObject.Instantiate<GameObject> (bullet.gameObject);
				go.layer = BulletLayer.LayerIndex;
				go.transform.position = transform.position;
				var bul = go.GetComponent<Bullet> ();
				bul.damage = bullet_damage;
				bul.speed = bullet_speed;
				bul.dir = (Vector2)(Quaternion.AngleAxis (i, Vector3.forward) * Vector2.right);
			}
		}

		return ammo > 0;
	}
}
