using UnityEngine;
using System.Collections;

public interface LeafNode : BehaviourInterface{
	ParentNode parentNode{ get; set; }
}
