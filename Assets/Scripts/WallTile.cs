using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : TouchableTile {

	private SpriteRenderer spriteRenderer;

	// Outine color
	public Color color = Color.white;

	// Called once before game starts
	void Awake () {
		//		Debug.Log("FloorTile : awake");
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	// What we do if we're touched
	public override bool OnTouch() 
	{
		Debug.Log("Floor tile touched");	
		return true;
	}


	// Can we start a touch sequence ? 
	public override bool CommenceTouch() {
		return true;
	}


	// Can we finish a touch seqeunce
	public override bool FinishTouch() {
		return true;
	}

	// Do we abort a touch sequence
	public override bool AbortTouch() {
		return false;
	}

	override public void UpdateOutline(bool showOutline) {
		MaterialPropertyBlock mpb = new MaterialPropertyBlock();
		spriteRenderer.GetPropertyBlock(mpb);
		mpb.SetFloat("_Outline", showOutline ? 1f : 0);
		mpb.SetColor("_OutlineColor", color);
		spriteRenderer.SetPropertyBlock(mpb);
	}
}
