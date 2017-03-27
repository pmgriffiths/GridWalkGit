using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	// Reference to click location
//	private Text coordsText;

	//Used to store location of screen touch origin for mobile controls.
	private Vector2 touchOrigin = -Vector2.one; 

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		#if UNITY_IOS 

		if (Input.touchCount > 0) {
			Touch myTouch = Input.touches[0];

			if (myTouch.phase == TouchPhase.Ended) {

				touchOrigin = myTouch.position;
				Vector3 touchTarget = Camera.main.ScreenToWorldPoint(touchOrigin);

				// map to world position
				int touchX = Mathf.FloorToInt(touchTarget.x);
				int touchY = Mathf.FloorToInt(touchTarget.y);

				Transform tileTransform = this.transform;
				Vector3 tilePosition = tileTransform.position;

				if (touchX == tilePosition.x && touchY == tilePosition.y) {
					Debug.Log("Canvas: Touch position: " + touchOrigin.x + ", " + touchOrigin.y);
					Debug.Log("Canvas: Touch target : " + touchX + ", " + touchY);
					Debug.Log("Canvas: BoardManager position: " + tilePosition.x + ", " + tilePosition.y);
					// degradeFloor();

//					coordsText.text = "Canvas click: " + touchX + ", " + touchY;
				}

			}
		}

		#endif
	}
}