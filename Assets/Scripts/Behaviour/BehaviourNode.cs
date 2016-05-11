using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BehaviourNode : ParentNode,LeafNode {
	private bool isActive = false;

	public bool IsActive {get{return isActive;} set{ isActive = value; }}
	public List<LeafNode> childNodes {get; set;}
	public ParentNode parentNode { get; set;}

	protected bool isRoot { get { return (parentNode == null || parentNode == this); } }

	public virtual void Activate (){
		isActive = true;
	}
	public virtual void Deactivate (){
		isActive = false;
		foreach (LeafNode childNode in childNodes) {
			childNode.Deactivate();
		}
	}
	public abstract void ChildTerminated (BehaviourInterface child,bool result);

	void AddChild (LeafNode child){
		childNodes.Add (child);
	}

	public virtual void Update(float dt, GameObject go){
		foreach (LeafNode child in childNodes)
			if(child.IsActive)
				child.Update (dt,go);
	}
}
