using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameState : Singleton<GameState> {

	private bool initialised = false;
	public SoundManager soundManager;

	public int rows = 4;
	public int columns = 4;

	public int level = 0;

	public int scoreA = 0;
	public int scoreB = 0;

	// Ensure we can't be constructed
	protected GameState() { }

	public void Initialise(InitialGameState initValues) {
		if (!initialised) {
			Debug.Log ("GameState initialised");
			rows = initValues.initialRows;
			columns = initValues.initialRows;
			soundManager = initValues.intialSoundManager;

			// Instantiate the audio sources for hte objects.
			soundManager.efxSource = gameObject.AddComponent<AudioSource>();
			soundManager.musicSource = gameObject.AddComponent<AudioSource>();

			initialised = true;
		} else {
			Debug.Log ("Attempt to re-initialise GameState - ignored");
		}
	}
		

	void Awake() {
		Debug.Log ("GameState awake method");
	}

	
}
