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

	public virtual void Activate (){
		isActive = true;
	}
	public virtual void Deactivate (){
		isActive = false;
		foreach (LeafNode childNode in ChildNodes) {
			childNode.Deactivate();
		}
	}
	public abstract void ChildTerminated (BehaviourInterface child,bool result);

	void AddChild (LeafNode child){
		ChildNodes.Add (child);
	}

	public virtual void Update(float dt, GameObject go){
		foreach (LeafNode child in ChildNodes)
			if(child.IsActive)
				child.Update (dt,go);
	}
}
