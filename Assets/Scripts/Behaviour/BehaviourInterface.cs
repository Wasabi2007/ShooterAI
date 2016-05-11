using UnityEngine;
using System.Collections;

public interface BehaviourInterface  {

	bool IsActive {get; set;}
	void Activate ();
	void Deactivate ();
	void ChildTerminated (BehaviourInterface child,bool result);
	void Update(float dt, GameObject go);
}
