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

	public override string get_path(string s = "")
	{
		s += this.GetType().Name + " -> " + "(" ;
		foreach (LeafNode child in ChildNodes)
			if (child.IsActive)
				s = child.get_path(s)+")";
		return s;
	}

	public override void ChildTerminated (GameObject go,BehaviourInterface child,bool result)
	{
		child.Deactivate (go);
		childReturns++;
		if (!result){
			if(!isRoot){
				parentNode.ChildTerminated(go,this,false);
				foreach (LeafNode childNode in childNodes) {
					childNode.Deactivate (go);
				}
			}else{
				Deactivate(go);
			}
		}

		if(!isRoot && childReturns >= childNodes.Count){
			parentNode.ChildTerminated(go,this,result);
		}

		if (isRoot && childReturns >= childNodes.Count) {
			Deactivate(go);
		}
		
	}
}
