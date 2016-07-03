using UnityEngine;
using System.Collections;
using System;

public class MeleeWeapon : Weapon
{

	public float swing_angle;
	public float reach;

	private float f = Mathf.PI;
	private float increment = Mathf.PI;

	void Start(){
		increment = Mathf.PI/bullet_firerate;
	}

	void Update(){
		f += increment * Time.deltaTime;
		f = Mathf.Clamp(f,0,Mathf.PI);
		transform.localRotation = Quaternion.AngleAxis (Mathf.Sin(f)*swing_angle, Vector3.forward);
		
	}

	public override bool shoot (SingleUnityLayer BulletLayer)
	{
		if (base.shoot (BulletLayer)){
			var hits = Physics2D.OverlapCircleAll(transform.position,reach);

			var info = new System.Object[]{ bullet_damage, new Vector2(), null };
			foreach(var hit in hits){
				if(hit.gameObject.CompareTag(transform.parent.gameObject.tag))
					continue;

				var dir = (Vector2)(Quaternion.AngleAxis (transform.rotation.eulerAngles.z, Vector3.forward) * Vector2.right);

				var normal = (hit.transform.position-transform.position).normalized;

				var angle = Vector3.Angle(dir,normal);

				if(angle < swing_angle){
					hit.SendMessage ("apply_directed_damage", info, SendMessageOptions.DontRequireReceiver);
				}
			}
			f = 0.0f;
		}
		//StartCoroutine("rotate_animation");

		return ammo > 0;
	}
}

