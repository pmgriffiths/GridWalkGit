using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialGameState : MonoBehaviour {

	/**
	 * Variables required for initialisation. Can be used by first scene 
	 * to initialise variables from prefabs without resource loads
	 */

	public int initialRows;

	public int initialColumns;

	public SoundManager intialSoundManager;

	void Awake() {
	}
}
