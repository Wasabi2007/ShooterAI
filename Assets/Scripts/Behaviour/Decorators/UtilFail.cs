using UnityEngine;
using System.Collections;

public class UtilFail : BehaviourNode {

	public override void Activate ()
	{
		base.Activate ();
		if (childNodes.Count > 1) {
			Debug.LogWarning("UtilFail Nodes only support 1 ChildNode. Only the first Child will be executet.");
		}

		if (childNodes.Count > 0){
			childNodes[0].Activate();
		}
	}

	public override void ChildTerminated (BehaviourInterface child, bool result)
	{
		childNodes [0].Deactivate ();
		if (result) {
						childNodes [0].Activate ();
				} else {
					parentNode.ChildTerminated(this,false);
				}
	}
}
