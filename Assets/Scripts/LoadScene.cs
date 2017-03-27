using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	// Change to scene (String)
	public void ChangeToScene (string sceneName) {
		// Application.LoadLevel(sceneIndex);
		SceneManager.LoadScene(sceneName);

		Debug.Log("Change to scene: " + sceneName);
		
	}
	
}
