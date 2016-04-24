using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour {

	public Vector2 dir;
	public float speed;
	public float damage;

	private Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		rigid.MovePosition (rigid.position + dir * speed * Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		coll.gameObject.SendMessage("applydamage", damage, SendMessageOptions.DontRequireReceiver);
		GameObject.Destroy (gameObject);
	}
}
