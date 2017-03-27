using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public float speed = 0.1f;

	public float MINSCALE = 2.0F; 
	public float MAXSCALE = 5.0F; 
	public float varianceInDistances = 5.0F; 
	private float touchDelta = 0.0F; 
	private Vector2 prevDist = new Vector2(0,0); 
	private Vector2 curDist = new Vector2(0,0); 
	private float speedTouch0 = 0.0F; 
	private float speedTouch1 = 0.0F;


	// Minimum pinch speed to used for zooming
	public float minPinchSpeed = 5.0f;

	// Whether the camera controls are enabled
	private bool cameraControlEnabled = false;

	// Use this for initialization
	void Start () {
		
	}

	public void EnableControls(bool cameraEnabled) {
		this.cameraControlEnabled = cameraEnabled;
	}
	
	// Check for controls if enabled
	void Update () {

		if (cameraControlEnabled) {

			if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) {

				curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
				//difference in previous locations using delta positions
				prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); 
				touchDelta = curDist.magnitude - prevDist.magnitude;

				speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
				speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

				Camera currentCamera = Camera.main;

				if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed)) {
					//				currentCamera.fieldOfView = Mathf.Clamp(currentCamera.fieldOfView + (1 * speed),15,90);
					currentCamera.orthographicSize = currentCamera.orthographicSize + speed;
				}
				if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed)) {
					//				currentCamera.fieldOfView = Mathf.Clamp(currentCamera.fieldOfView - (1 * speed),15,90);
					currentCamera.orthographicSize = currentCamera.orthographicSize - speed;
				}
			}   
		}
	}
}
