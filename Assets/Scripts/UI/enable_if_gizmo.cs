using UnityEngine;
using System;
using System.Reflection;
using System.Collections;


public class enable_if_gizmo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bool visible = AreGizmosVisible ();
		for (int i = 0; i < transform.childCount; ++i) {
			transform.GetChild (i).gameObject.SetActive (visible);
		}	
	}


	/// <summary>
	/// Ares the gizmos visible.
	/// 
	/// Author: Edwin 
	/// Source: http://answers.unity3d.com/questions/41591/how-can-i-tell-if-the-gizmos-button-has-been-check.html
	/// </summary>
	/// <returns><c>true</c>, if gizmos visible was ared, <c>false</c> otherwise.</returns>
	bool AreGizmosVisible()
	{
		Assembly asm = Assembly.GetAssembly(typeof(UnityEditor.Editor));
		Type type = asm.GetType("UnityEditor.GameView");
		if (type != null)
		{
			UnityEditor.EditorWindow window = UnityEditor.EditorWindow.GetWindow(type);
			FieldInfo gizmosField = type.GetField("m_Gizmos", BindingFlags.NonPublic | BindingFlags.Instance);
			if(gizmosField != null)
				return (bool)gizmosField.GetValue(window);
		}
		return false;
	}
}
