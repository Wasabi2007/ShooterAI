using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour {

	public Vector2 dir;
	public float speed;
	public float damage;

	private Rigidbody2D rigid;
	private System.Object[] info;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		info = new System.Object[]{ damage, dir, gameObject };
	}
	
	// Update is called once per frame
	void Update () {
		rigid.MovePosition (rigid.position + dir * speed * Time.deltaTime);

		var view_port_pos = Camera.main.WorldToViewportPoint (transform.position);
		if (Mathf.Sign (view_port_pos.x) > 1 || Mathf.Sign (view_port_pos.y) > 1) {
			
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		
		coll.gameObject.SendMessage("applydirecteddamage", info, SendMessageOptions.DontRequireReceiver);
		if(coll.gameObject.layer == 0){
			GameObject.Destroy (gameObject);
		}
	}
}
