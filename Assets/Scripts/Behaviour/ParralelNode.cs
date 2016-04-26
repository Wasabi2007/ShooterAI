using UnityEngine;
using System.Collections;

public class ParralelNode : BehaviourNode {


	int childReturns = 0;

	public override void Activate ()
	{
		base.Activate ();
		childReturns = 0;
		foreach (LeafNode childNode in childNodes) {
			childNode.Activate();
		}
	}

	public override void ChildTerminated (BehaviourInterface child,bool result)
	{
		child.Deactivate ();
		childReturns++;
		if (!result){
			if(!isRoot){
				parentNode.ChildTerminated(this,false);
			}else{
				Deactivate();
			}
		}

		if(!isRoot && childReturns >= childNodes.Count){
			parentNode.ChildTerminated(this,true);
		}

		if (isRoot && childReturns >= childNodes.Count) {
			Deactivate();
		}
		
	}
}
