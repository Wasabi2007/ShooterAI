using UnityEngine;
using System.Collections;

public class ParralelNode : BehaviourNode {


	int childReturns = 0;

	public override void Activate (GameObject go)
	{
		base.Activate (go);
		childReturns = 0;
		foreach (LeafNode childNode in childNodes) {
			childNode.Activate(go);
		}
	}

	public override void ChildTerminated (GameObject go,BehaviourInterface child,bool result)
	{
		child.Deactivate (go);
		childReturns++;
		if (!result){
			if(!isRoot){
				parentNode.ChildTerminated(go,this,false);
			}else{
				Deactivate(go);
			}
		}

		if(!isRoot && childReturns >= childNodes.Count){
			parentNode.ChildTerminated(go,this,true);
		}

		if (isRoot && childReturns >= childNodes.Count) {
			Deactivate(go);
		}
		
	}
}
