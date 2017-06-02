using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor3D : TouchableTile {

	public int hitPoints;

	// See whether we can set colour from Unity editor
	public Color selectedColour;
	public Color expiredColour;

	// Color to show if we've been moved onto from the 'wrong' tile type
	public Color distressedColour;

	private bool distressed = false;

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

		SetColour(expiredColour);
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
	public override bool CanFinishTouch(BoardLayout.TileType lineType) {
		return false;
	}


	public override bool AbortTouch(BoardLayout.TileType pathType) { 
		return hitPoints == 0;
	}

	private void SetColour(Color colour) {
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		mat.SetColor("_EmissionColor", colour);
	}


	// TODO:: work out how to change color and reset tile type to enable
	// changes from nearby tiles

	override public void Highlight(bool showHighlight) {

		if (showHighlight) {
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			SetColour(distressed ? distressedColour : selectedColour);
		} else {
			gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			// TODO: use black to disable emission - not sure this is right
			SetColour(Color.black);
		}
	}
		

	override public bool SupportsLine(TouchableTile.Movement movement) {
		bool canMove = false;
		Debug.Log("Tile: " + tileType + " Move: " + movement.direction);

		// Direction tiles only support direction movement
		switch (tileType) {
			case BoardLayout.TileType.Floor_X:
				if (movement.direction == MovementDirection.X_DEC || movement.direction == MovementDirection.X_INC) {
					canMove = true;
				}
				break;
			case BoardLayout.TileType.Floor_Z:
				if (movement.direction == MovementDirection.Z_DEC || movement.direction == MovementDirection.Z_INC) {
					canMove = true;
				}
				break;
			default:
				canMove = true;
				break;
		}

		// We're not distressed starting with UNDEF as this is wall type - maybe type walls
		// more clearly later on.
		distressed = movement.prevType != tileType && movement.prevType != BoardLayout.TileType.UNDEF;
		Highlight (canMove);
		return canMove;
	}
}
