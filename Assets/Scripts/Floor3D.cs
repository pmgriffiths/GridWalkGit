using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor3D : TouchableTile {

	public int hitPoints;

	// Outine color
	public Color color = Color.white;

	// Called once before game starts
	void Awake () {
	}

	public void degradeFloor() {
		Debug.Log("DegradeFloor: " + hitPoints);
		hitPoints--;

		if (hitPoints <= 0) { 
			SetColour(Color.red);
			gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

		} else {
			Highlight(false);
		}
	}


	// What we do if we're touched
	public override void ApplyTouch() 
	{
		degradeFloor();
	}

	// Can we start a touch sequence ? 
	public override bool CommenceTouch(out TouchableTile.TileType tileType) {
		tileType = TileType.Floor;
		return false;
	}

	// Can we finish a touch seqeunce
	public override bool CanFinishTouch(out TouchableTile.TileType tileType) {
		tileType = TileType.Floor;
		return false;
	}

	// Can we extend a touch sequence, ? 
//	public override bool CanExtendTouch() {
//		return hitPoints > 0;
//	}

	public override bool AbortTouch() { 
		return hitPoints == 0;
	}

	private void SetColour(Color colour) {
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		mat.SetColor("_EmissionColor", colour);
	}

	override public void Highlight(bool showHighlight) {

		if (showHighlight) {
			gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			SetColour(Color.grey);
		} else {
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			SetColour(Color.black);
		}


	}


}
