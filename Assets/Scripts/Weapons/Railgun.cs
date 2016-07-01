using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Railgun : Weapon
{
	public LayerMask mask;

	public override bool shoot (SingleUnityLayer BulletLayer)
	{
		if (base.shoot (BulletLayer)){

			var dir = (Vector2)(Quaternion.AngleAxis (transform.rotation.eulerAngles.z, Vector3.forward) * Vector2.right);

			var hits = Physics2D.RaycastAll (transform.position, dir,float.MaxValue,mask);
			var line_render = GetComponent<LineRenderer> ();
			//line_render.SetVertexCount (2);
			line_render.SetPosition(0,transform.position);
			line_render.SetPosition(1,transform.position+(Vector3)dir*100000);

			RaycastHit2D min_dist_hit = new RaycastHit2D();
			float min_dist = float.MaxValue;
			var info = new System.Object[]{ bullet_damage, dir, null };
			foreach (var hit in hits) {
				if (hit.collider.gameObject.CompareTag (transform.parent.tag))
					continue;

				if (hit.collider.gameObject.CompareTag("Envoy")) {
					if (hit.distance < min_dist) {
						min_dist_hit = hit;
						min_dist = hit.distance;
					}
				}
				hit.collider.gameObject.SendMessage ("apply_directed_damage", info, SendMessageOptions.DontRequireReceiver);
				hit.collider.SendMessageUpwards ("danger",(Vector3)hit.point, SendMessageOptions.DontRequireReceiver);
			}
			if (min_dist_hit) {
				line_render.SetPosition (1, min_dist_hit.point);
			}

			StartCoroutine("fade");
		}

		return ammo > 0;
	}


	IEnumerator fade() {
		var line_render = GetComponent<LineRenderer> ();

		for (float f = 1; f >= 0; f -= 1f/16f) {
			Color c = Color.white;
			c.a = f;
			line_render.SetColors (c,c);
			yield return new WaitForSeconds(0.01f);
		}
	}
}

