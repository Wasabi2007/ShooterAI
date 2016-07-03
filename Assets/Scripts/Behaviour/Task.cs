using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Task : LeafNode , BehaviourInterface {

	private bool isActive = false;

	public bool IsActive {get{return isActive;} set{ isActive = value; }}
	public ParentNode parentNode {get; set;}

    public string get_path(string s = "")
    {
        s += this.GetType().Name;
        return s;
    }

	public virtual void Activate (GameObject go){
		//Debug.Log (this.GetType().Name);
		isActive = true;
		var r = (BehaviourNode)parentNode;
		while (!r.isRoot) {
			r = (BehaviourNode)r.parentNode;
		}
		Debug.Log (r.get_path());
	}
	public virtual void Deactivate (GameObject go){
		isActive = false;
	}
	public virtual void ChildTerminated (GameObject go,BehaviourInterface child,bool result){}

	public virtual void Update(float dt, GameObject go){
	}


}
