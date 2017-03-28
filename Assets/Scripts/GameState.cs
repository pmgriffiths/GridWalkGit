using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState> {

	// Ensure we can't be constructed
	protected GameState() { }
	
	public int rows = 8;
	public int columns = 8;

	public int level = 0;

}
