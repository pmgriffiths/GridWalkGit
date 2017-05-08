using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchableTile : MonoBehaviour {

	public enum TileType : int { Wall_0 = 0, Wall_1, Wall_2, Wall_3, Floor_0, Floor_1, Floor_2, Floor_3, Floor_X, Floor_Z };

	// Variables/types used for tracking movement direction between tiles
	public enum MovementDirection { NONE, X_INC, X_DEC, Z_INC, Z_DEC, Y_INC, Y_DEC, DIAGONAL};

	public TileType tileType;

	// This tile has been touched
//	public abstract bool OnTouch ();

	// Can this originate a touch sequence
	public abstract bool CommenceTouch(out TileType type);

	// Could this tile finish a touch sequence
	public abstract bool CanFinishTouch(out TileType type);

	// Does this tile interupt a touch sequence ? 
	public abstract bool AbortTouch (); 

	// Can it extend a sequence ? 
//	public abstract bool ExtendTouch ();

	public abstract void ApplyTouch(); 

	// Show/hide an outline
	public abstract void Highlight(bool highlight);

	public virtual bool SupportsDirection(MovementDirection direction) {
		return true;
	}
}
