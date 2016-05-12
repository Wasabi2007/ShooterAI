using UnityEngine;
using System.Collections;

public class UtilFail : BehaviourNode {

	public override void Activate (GameObject go)
	{
		base.Activate (go);
		if (ChildNodes.Count > 1) {
			Debug.LogWarning("UtilFail Nodes only support 1 ChildNode. Only the first Child will be executet.");
		}

		if (ChildNodes.Count > 0){
			ChildNodes[0].Activate(go);
		}
	}

	public override void ChildTerminated (GameObject go,BehaviourInterface child, bool result)
	{
		ChildNodes [0].Deactivate (go);
		if (result) {
				ChildNodes [0].Activate (go);
		} else {
			parentNode.ChildTerminated(go,this,false);
		}
	}
}
