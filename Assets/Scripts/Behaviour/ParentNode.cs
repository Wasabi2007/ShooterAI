using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ParentNode : BehaviourInterface {
	List<LeafNode> ChildNodes{ get; set; }
}
