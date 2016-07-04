using UnityEngine;
using System.Collections;

public class Selector : BehaviourNode {

	int childIndex = 0;

	public override void Activate (GameObject go)
	{
		base.Activate (go);
		childIndex = 0;
		if (childNodes.Count > 0) {
			childIndex++;
			childNodes[childIndex-1].Activate(go);
		}
	}
	

	public override void ChildTerminated (GameObject go,BehaviourInterface child,bool result)
	{
		child.Deactivate (go);
		if (childIndex >= childNodes.Count) {
			if(!isRoot){
				Debug.Log ("Selector returned :"+result);
				parentNode.ChildTerminated(go,this,result);
			}else{
				Deactivate(go);
			}
			return;
		}
		
		if (!result) {
			childNodes [childIndex].Activate (go);
			childIndex++;
		} else {
			if(!isRoot){
				Debug.Log ("Selector returned true");
				parentNode.ChildTerminated(go,this,true);
			}else{
				Deactivate(go);
			}
		}
		
	}
}
