using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

	public Bullet bullet;

	public int ammo;
	public int ammo_max;
	public int clips;
	public float bullet_speed = 10;
	public float bullet_firerate = 0.1f;
	public float reload_speed = 1;
	public float bullet_damage = 10;
	protected float last_shoot;
	protected float reload_time;


	public virtual bool shoot (SingleUnityLayer BulletLayer){

		if (last_shoot + bullet_firerate < Time.time && ammo > 0) {
			last_shoot = Time.time;

			ammo--;

			if (ammo == 0)
				reload_time = Time.time + reload_speed;
			return true;
		}
		reload ();

		return false;

	}

	public void reload(){
		if (ammo == 0 && clips != 0 && reload_time < Time.time) {
			clips--;
			ammo = ammo_max;
		}
	}

	public void add_clips(int clips){
		this.clips += clips;
	}
}
