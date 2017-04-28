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
		if (hitPoints > 0) {
//			spriteRenderer.sprite = floorSprites[hitPoints];
		} else {
			// Floor is broken
			//gameObject.SetActive(false);
//			spriteRenderer.sprite = brokenFloor;
		}
	}



	// What we do if we're touched
	public override bool OnTouch() 
	{
		Debug.Log("Floor tile touched");	
		degradeFloor();
		return true;
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

	public override bool AbortTouch() { 
		return hitPoints == 0;
	}

	override public void Highlight(bool showHighlight) {

		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;

		if (showHighlight) {
			gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			mat.SetColor ("_EmissionColor", Color.grey);

		} else {
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			mat.SetColor("_EmissionColor", Color.black);
		}


	}


}
