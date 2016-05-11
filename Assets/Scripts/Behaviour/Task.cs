using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Task : LeafNode , BehaviourInterface {

	private bool isActive = false;

	public bool IsActive {get{return isActive;} set{ isActive = value; }}
	public ParentNode parentNode {get; set;}

	public virtual void Activate (){
		isActive = true;
	}
	public virtual void Deactivate (){
		isActive = false;
	}
	public virtual void ChildTerminated (BehaviourInterface child,bool result){}

	public virtual void Update(float dt, GameObject go){
	}


}
