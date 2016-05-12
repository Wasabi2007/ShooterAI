using UnityEngine;
using System.Collections;

public interface BehaviourInterface  {

	bool IsActive {get; set;}
	void Activate (GameObject go);
	void Deactivate (GameObject go);
	void ChildTerminated (GameObject go,BehaviourInterface child,bool result);
	void Update(float dt, GameObject go);
}
