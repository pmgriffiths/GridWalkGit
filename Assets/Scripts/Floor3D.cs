﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor3D : TouchableTile {

	public int hitPoints;

	// Called once before game starts
	void Awake () {
	}

	public void degradeFloor() {
		Debug.Log("DegradeFloor: " + hitPoints);
		hitPoints--;

		if (hitPoints <= 0) { 
			// SetColour(Color.red);
			//gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			StartCoroutine("Fade");
		} else {
			Highlight(false);
		}
	}

	IEnumerator Fade() {
		Renderer renderer = GetComponent<Renderer> ();

		SetColour(Color.red);
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = renderer.material.color;
			c.a = f;
			renderer.material.color = c;
			gameObject.transform.localScale = new Vector3(f, f, f);
			yield return new WaitForSeconds(.1f);
		}
	}

	// What we do if we're touched
	public override void ApplyTouch() 
	{
		degradeFloor();
	}

	// Can we start a touch sequence ? 
	public override bool CommenceTouch(out BoardLayout.TileType tileType) {
		tileType = this.tileType;
		return false;
	}

	// Can we finish a touch seqeunce
	public override bool CanFinishTouch(out BoardLayout.TileType tileType) {
		tileType = this.tileType;
		return false;
	}


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
		
	override public bool SupportsDirection(TouchableTile.MovementDirection direction) {
		bool canMove = false;
		Debug.Log("Tile: " + tileType + " Move: " + direction);
		// Direction tiles only support direction movement
		switch (tileType) {
			case BoardLayout.TileType.Floor_X:
				if (direction == MovementDirection.X_DEC || direction == MovementDirection.X_INC) {
					canMove = true;
				}
				break;
			case BoardLayout.TileType.Floor_Z:
				if (direction == MovementDirection.Z_DEC || direction == MovementDirection.Z_INC) {
					canMove = true;
				}
				break;
			default:
				canMove = true;
				break;
		}
			
		return canMove;
	}
}
