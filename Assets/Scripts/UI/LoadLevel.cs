using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public string level_name;

	public void OnSubmit(){
		SceneManager.LoadScene (level_name);
	}
}
