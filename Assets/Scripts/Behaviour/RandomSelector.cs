using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSelector : BehaviourNode {

	protected List<LeafNode> leftTotry;

	public override void Activate (GameObject go)
	{
		base.Activate (go);
		leftTotry = new List<LeafNode>(childNodes);
		activateRandom (go);
	}

	private void activateRandom(GameObject go){
		int index = Random.Range (0, leftTotry.Count);
		leftTotry [index].Activate (go);
		leftTotry.RemoveAt (index);
	}

	public override void ChildTerminated (GameObject go,BehaviourInterface child, bool result)
	{
		child.Deactivate (go);
		if(result){
			parentNode.ChildTerminated(go,this,true);
		}else{
			if(leftTotry.Count > 0){
				activateRandom(go);
			}else{
				parentNode.ChildTerminated(go,this,false);
			}
		}
	}

}
