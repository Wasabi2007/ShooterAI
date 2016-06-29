using System;
using UnityEngine;


public class way_still_good_condition : Task
{
	float min_distance_factor;

	public way_still_good_condition (float min_distance_factor = 0.8f)
	{
		this.min_distance_factor = min_distance_factor;

	}

	public override void Update (float dt, GameObject go)
	{
		base.Update (dt, go);

		var cc = go.GetComponent<CharController> ();
		var claimed_node = cc.claimend_node;

		var target = GameObject.FindGameObjectWithTag ("Player");
		var direction_vector = go.transform.position - target.transform.position;
		var distance = Vector3.Distance(go.transform.position,target.transform.position);
		var should_be_position = target.transform.position + direction_vector.normalized * Mathf.Min (cc.follow_range, distance)*min_distance_factor;

		var result = cc.nav_path.node_still_viable_cover (claimed_node,go.transform.position, target.transform.position);
		parentNode.ChildTerminated (go, this, result);
	}
}


