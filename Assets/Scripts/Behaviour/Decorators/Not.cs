using UnityEngine;
using System.Collections;

public class Not : BehaviourNode {


	public Not(){
	}

	public override void Activate (GameObject go)
	{
		base.Activate (go);
		if (ChildNodes.Count > 1) {
			Debug.LogWarning("Not Nodes only support 1 ChildNode. Only the first Child will be executet.");
		}

		if (ChildNodes.Count > 0){
			ChildNodes[0].Activate(go);
		}
	}

	public override void ChildTerminated (GameObject go,BehaviourInterface child, bool result)
	{

		if (parentNode != null) {
			parentNode.ChildTerminated (go, this, !result);
			ChildNodes[0].Deactivate(go);
		} else {
			Deactivate (go);
		}
	}
}

