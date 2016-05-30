using UnityEditor;

public class CustomHotkeys {

	//CTRL+D to duplicate
	[MenuItem("Edit/Custom/Duplicate %d")]
	static void Duplicate () {
		EditorApplication.ExecuteMenuItem("Edit/Duplicate");
	}

	//CTRL+Q to quit
	[MenuItem("Edit/Custom/Quit Unity %q")]
	static void Quit () {
		EditorApplication.ExecuteMenuItem("File/Quit");
	}
}
