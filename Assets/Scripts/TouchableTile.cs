using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchableTile : MonoBehaviour {

	// This tile has been touched
	public abstract bool OnTouch ();

	// Can this originate a touch sequence
	public abstract bool CommenceTouch();

	// Does this tile finish a touch sequence
	public abstract bool FinishTouch();

	// Does this tile interupt a touch sequence ? 
	public abstract bool AbortTouch (); 

	// Show/hide an outline
	public abstract void UpdateOutline(bool showOutline);
}
