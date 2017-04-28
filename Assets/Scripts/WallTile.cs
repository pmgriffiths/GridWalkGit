using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallTile : TouchableTile {

	private SpriteRenderer spriteRenderer;

	// Outine color
	public Color color = Color.white;

	// Wall Tile Types
	public Sprite[] wallTiles; 

	// What type are we
	private TouchableTile.TileType tileType;

	// Called once before game starts
	void Awake () {
		//		Debug.Log("FloorTile : awake");
		spriteRenderer = GetComponent<SpriteRenderer>();
		int ourType = Random.Range (0, wallTiles.Length);
		tileType = (TileType)ourType;
		spriteRenderer.sprite = wallTiles[ourType];
	}

	// What we do if we're touched
	public override void ApplyTouch() 
	{
		Debug.Log("Floor tile touched");	
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
		MaterialPropertyBlock mpb = new MaterialPropertyBlock();
		spriteRenderer.GetPropertyBlock(mpb);
		mpb.SetFloat("_Outline", showHighlight ? 1f : 0);
		mpb.SetColor("_OutlineColor", color);
		spriteRenderer.SetPropertyBlock(mpb);
	}
}
