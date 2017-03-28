using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState> {

	// Ensure we can't be constructed
	protected GameState() { }

	void Awake() {
		Debug.Log ("GameState awake method");
	}

	
	public int rows = 4;
	public int columns = 4;

	public int level = 0;

}
