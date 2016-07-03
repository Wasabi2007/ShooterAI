using UnityEngine;
using System.Collections;

public class target_moved_condition : Task
{

	public override void Activate (GameObject go)
	{
		base.Activate (go);
	}

	public override void Update(float dt, GameObject go){
		base.Update (dt, go);

		CharController cc = go.GetComponent<CharController>();
		cc.claim_node (null);
		GameObject player = GameObject.FindGameObjectWithTag ("Player");


		var node = cc.Path.ToArray()[cc.Path.Count-1];

		Node end_node = cc.nav_path.get_nearest_node (go.transform.position);

		parentNode.ChildTerminated (go,this, node == end_node);


	}
}

