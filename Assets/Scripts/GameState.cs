using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState> {



	// Ensure we can't be constructed
	protected GameState() { }

	public void Initialise(InitialGameState initValues) {
		if (!initialised) {
			Debug.Log ("GameState initialised");
			rows = initValues.initialRows;
			columns = initValues.initialRows;
			soundManager = initValues.intialSoundManager;
			initialised = true;
		} else {
			Debug.Log ("Attempt to re-initialise GameState - ignored");
		}
	}
		

	public SoundManager soundManager;

	void Awake() {
		Debug.Log ("GameState awake method");
	}

	
	private bool initialised = false;
	public int rows = 4;
	public int columns = 4;

	public int level = 0;

}
