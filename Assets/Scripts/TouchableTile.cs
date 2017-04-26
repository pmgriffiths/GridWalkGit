using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchableTile : MonoBehaviour {

	public enum TileType : int { Wall_0 = 0, Wall_1 = 1, Wall_2 = 2, Wall_3 = 3, Floor };

	// This tile has been touched
	public abstract bool OnTouch ();

	// Can this originate a touch sequence
	public abstract bool CommenceTouch(out TileType type);

	// Could this tile finish a touch sequence
	public abstract bool CanFinishTouch(out TileType type);

	// Does this tile interupt a touch sequence ? 
	public abstract bool AbortTouch (); 

	// Show/hide an outline
	public abstract void Highlight(bool highlight);
}
