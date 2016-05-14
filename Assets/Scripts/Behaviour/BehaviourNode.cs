using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BehaviourNode : ParentNode,LeafNode {
	protected bool isActive = false;
	protected List<LeafNode> childNodes = new List<LeafNode> ();

	public bool IsActive {get{return isActive;} set{ isActive = value; }}
	public List<LeafNode> ChildNodes {get{return childNodes;} set{ childNodes = value; }}
	public ParentNode parentNode { get; set;}

	protected bool isRoot { get { return (parentNode == null || parentNode == this); } }

	public virtual void Activate (GameObject go){
		isActive = true;
	}
	public virtual void Deactivate (GameObject go){
		isActive = false;
		foreach (LeafNode childNode in ChildNodes) {
			childNode.Deactivate(go);
		}
	}
	public abstract void ChildTerminated (GameObject go,BehaviourInterface child,bool result);

	public void AddChild (LeafNode child){
		ChildNodes.Add (child);
		child.parentNode = this;
	}

	public virtual void Update(float dt, GameObject go){
		foreach (LeafNode child in ChildNodes)
			if(child.IsActive)
				child.Update (dt,go);
	}
}
