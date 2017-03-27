using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	// Ensures a game manager exists, creating one if necessary
	public GameObject gameManager;

	// Called when we wake up ? 
	void Awake () {
		if (GameManager.instance == null) {
			Instantiate(gameManager);	
		}
	}

}
