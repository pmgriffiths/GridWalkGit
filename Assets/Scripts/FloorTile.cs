using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : TouchableTile {

	// Sequence of sprites for degrading floor
	public Sprite[] floorSprites;

	// Sprite for empty floor tile
	public Sprite brokenFloor;

	private SpriteRenderer spriteRenderer;

	private int hitPoints;

	// Outine color
	public Color color = Color.white;

	// Called once before game starts
	void Awake () {
//		Debug.Log("FloorTile : awake");
		spriteRenderer = GetComponent<SpriteRenderer>();
		hitPoints = floorSprites.Length - 1;
		spriteRenderer.sprite = floorSprites[hitPoints];
	}
	
	public void degradeFloor() {
		Debug.Log("DegradeFloor: " + hitPoints);
 		hitPoints--;
		if (hitPoints > 0) {
			spriteRenderer.sprite = floorSprites[hitPoints];
		} else {
			// Floor is broken
			//gameObject.SetActive(false);
			spriteRenderer.sprite = brokenFloor;
		}
	}

	private void Update()
	{
		#if UNITY_IOS 
/**
		if (Input.touchCount > 0)
		{
			Touch myTouch = Input.touches[0];

			if (myTouch.phase == TouchPhase.Ended)
			{

				touchOrigin = myTouch.position;
				Vector3 touchTarget = Camera.main.ScreenToWorldPoint(touchOrigin);

				// map to world position
				int touchX = Mathf.FloorToInt(touchTarget.x);
				int touchY = Mathf.FloorToInt(touchTarget.y);

				Transform tileTransform = this.transform;
				Vector3 tilePosition = tileTransform.position;

				if (touchX == tilePosition.x && touchY == tilePosition.y) {
					Debug.Log("Touch position: " + touchOrigin.x + ", " + touchOrigin.y);
					Debug.Log("Touch target : " + touchX + ", " + touchY);
					Debug.Log("Tile position: " + tilePosition.x + ", " + tilePosition.y);
					degradeFloor();
				}

			}
		}
*/
		/**
		//Check if Input has registered more than zero touches
		if (Input.touchCount > 0)
		{
			//Store the first touch detected.
			Touch myTouch = Input.touches[0];

			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began)
			{
				//If so, set touchOrigin to the position of that touch
				touchOrigin = myTouch.position;
			}

			//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				//Set touchEnd to equal the position of this touch
				Vector2 touchEnd = myTouch.position;

				//Calculate the difference between the beginning and end of the touch on the x axis.
				float x = touchEnd.x - touchOrigin.x;

				//Calculate the difference between the beginning and end of the touch on the y axis.
				float y = touchEnd.y - touchOrigin.y;

				//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
				touchOrigin.x = -1;

				//Check if the difference along the x axis is greater than the difference along the y axis.
				if (Mathf.Abs(x) > Mathf.Abs(y))
					//If x is greater than zero, set horizontal to 1, otherwise set it to -1
					horizontal = x > 0 ? 1 : -1;
				else
					//If y is greater than zero, set horizontal to 1, otherwise set it to -1
					vertical = y > 0 ? 1 : -1;
			}
		} 
		*/
		#endif
	}

	// What we do if we're touched
	public override bool OnTouch() 
	{
		Debug.Log("Floor tile touched");	
		degradeFloor();
		return true;
	}

	// Can we start a touch sequence ? 
	public override bool CommenceTouch() {
		return false;
	}

	// Can we finish a touch seqeunce
	public override bool FinishTouch() {
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
