using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wall3D : TouchableTile {

	// What type are we
	private TouchableTile.TileType tileType = TouchableTile.TileType.Wall_0;

	// Called once before game starts
	void Awake () {
	}

	// What we do if we're touched
	public override void ApplyTouch() 
	{
		Debug.Log("Floor tile touched");	
		Highlight(false);
	}


	// Can we start a touch sequence ? 
	public override bool CommenceTouch(out TouchableTile.TileType tileType) {
		tileType = this.tileType;
		return true;
	}


	// Can we finish a touch seqeunce - only if we match the path type
	public override bool CanFinishTouch(out TouchableTile.TileType tileType) {
		tileType = this.tileType;
		return true;
	}

	// Do we abort a touch sequence
	public override bool AbortTouch() {
		return false;
	}

	override public void Highlight(bool showHighlight) {
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;

		if (showHighlight) {
			gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			mat.SetColor ("_EmissionColor", Color.yellow);

		} else {
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			mat.SetColor("_EmissionColor", Color.black);
		}
	}



}
