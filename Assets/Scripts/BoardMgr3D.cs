using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardMgr3D : MonoBehaviour {

/**	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	} */

	// Debug output text
	private Text boardPositionText;
	private Text touchPhaseText;
	private Text touchPositionText;
	private Text touchTargetText;

	// time to display level text
	public float levelStartDelay = 2f;

	private Text levelText;

	// Public Variables
	private int columns;
	private int rows;
	private int level;

	private Text scoreAText;
	private Text scoreBText;
	private int scoreA = 0;
	private int scoreB = 0;

	// Position of previously touched tile
	private Vector3 previousPosition;

	// Keeps hierarchy clean using this as parent of tiles
	private Transform boardHolder;

	public BoardLayout boardLayout;

	// Tracks objects on the game board and whether there is an object there.
	private List<Vector3> gridPositions = new List<Vector3>();

	// Map of game objects on board by position
	private Dictionary<Vector3, TouchableTile> tilePositions = new Dictionary<Vector3, TouchableTile>();

	//Used to store location of screen touch origin for mobile controls.
	private Vector2 touchOrigin = -Vector2.one; 

	// State of touch movement
	enum TouchState {NoTouch, PathStarted};
	TouchState currentTouchState = TouchState.NoTouch;

	// Sequence of touched tiles
	private HashSet<TouchableTile> touchedTiles = new HashSet<TouchableTile>();

	// Reference to cover image
	private GameObject levelImage;

	// Tile type of path being drawn
	TouchableTile.TileType pathTileType;

	public void Awake() {
		Debug.Log ("BoardManager is awake");

		// take our state from the game state
		rows = GameState.Instance.rows;
		columns = GameState.Instance.columns;
		level = ++GameState.Instance.level;

		scoreA = GameState.Instance.scoreA;
		scoreB = GameState.Instance.scoreB;


		InitGame ();
	}

	// Clears the list of grid positions
	private void IntialiseList() 
	{ 
		gridPositions.Clear();

		// Remove each tile
		foreach (TouchableTile oldTile in tilePositions.Values) {
			Destroy(oldTile);
		}
		tilePositions.Clear();
		touchedTiles.Clear();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				// List of posible positions for tiles
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	// Initialise the game board 
	void InitGame() {
		SetupScene(level, rows, columns);

		levelImage = GameObject.Find("LevelImage");
		GameObject levelTextObj = GameObject.Find ("LevelText");
		Debug.Log ("Level text obj is " + levelTextObj);
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		levelText.text = "Level "+ level;
		levelImage.SetActive(true);
		Invoke("HideLevelImage", levelStartDelay);

		scoreAText = GameObject.Find("ScoreA").GetComponent<Text>();
		scoreBText = GameObject.Find("ScoreB").GetComponent<Text>();
		SetScore();
	} 

	// Sets up the board
	void BoardSetup() 
	{
		Debug.Log("BoardManager boardSetup for rows " + rows + ", columns " + columns);

		boardHolder = new GameObject("Board").transform;

		boardLayout.SetupBoard(boardHolder, rows, columns, tilePositions);

		// Position the camera
		Camera cam = Camera.main;
		cam.transform.position = new Vector3(-4, 4,  -4);

	}

	void GetDebugTextReferences()
	{
		boardPositionText = GameObject.Find("BoardPosition").GetComponent<Text>();
		touchPhaseText = GameObject.Find("TouchPhase").GetComponent<Text>();
		touchPositionText = GameObject.Find("TouchPosition").GetComponent<Text>();
		touchTargetText = GameObject.Find("TouchTarget").GetComponent<Text>();
	}


	public void SetupScene (int level, int rows, int columns)
	{
		this.rows = rows;
		this.columns = columns;
		IntialiseList();
		BoardSetup();


		GetDebugTextReferences();
	}

	private bool CheckForTile(Vector2 touchPoint, out TouchableTile tile) {
		bool haveTile = false;
		tile = null;

		touchPositionText.text = "Touch position: " + touchPoint.x + ", " + touchPoint.y;

		Vector3 directedTouch = Camera.main.ScreenToWorldPoint(touchPoint);

		Ray hitRay = new Ray(directedTouch, Camera.main.transform.forward);

//		Ray ray = Camera.main.ScreenPointToRay(touchPoint);
		RaycastHit hit;

		if (Physics.Raycast(hitRay, out hit, 100)) {
			GameObject hitTile = hit.collider.gameObject;

			// Look for tile at that location
			if (tilePositions.TryGetValue(hitTile.transform.position, out tile)) {
				haveTile = true;
			} else { 
				Debug.LogError("Hit something but not known tile: " + hitTile);
			}

 		}

	//	Debug.Log("hit Something " + hitSomething);

		// map from world to game position
//		int touchX = (int)Mathf.Round(touchTarget.x);
//		int touchY = (int)Mathf.Round(touchTarget.y);
//		int touchZ = (int)Mathf.Round(touchTarget.z);
//		Debug.Log("Touch Target: " + touchX+ ", " + touchY + ", " + touchZ); 
//		touchTargetText.text = "Touch Target: " + touchX+ ", " + touchY + ", " + touchZ;
//		touchTargetText.text = "hit Something " + hitSomething;


//		// Check for a tile at this position and touch it
//		Vector3 location = new Vector3(touchX, touchY, 0);

//		if (tilePositions.TryGetValue(location, out tile)) {
//			touchPositionText.text = "Got it";	
//			haveTile = true;
//		}

//		Debug.Log("FoundTile: " + haveTile);
		return haveTile;
	}


	// Method for removing splash image
	private void HideLevelImage() {
		levelImage.SetActive(false);
	}


	private void Update() {

		Vector2 touchPoint = new Vector2(0,0);
		TouchableTile tile;

		bool touchStarted = false;
		bool touchMoved = false;
		bool touchEnded = false;

		#if UNITY_IOS
		if (Input.touchCount > 0) {
			Touch myTouch = Input.touches[0];

			switch (myTouch.phase) {
				case TouchPhase.Began:
					touchStarted = true;
					break;
				case TouchPhase.Moved:
					touchMoved = true;
					break;
				case TouchPhase.Ended:
					touchEnded = currentTouchState == TouchState.PathStarted;
					break;
			}
			touchPoint = myTouch.position;
		}
		#else
		if (Input.GetMouseButtonDown(0)) {
			touchStarted = true;
		} else if (Input.GetMouseButton(0)) {
			touchMoved = true;
		} else if (Input.GetMouseButtonUp(0)) {
			touchEnded = true;
		}
		Vector3 mouseClick = Input.mousePosition;
		touchPoint = new Vector2(mouseClick.x, mouseClick.y);
		#endif
			
		if (touchStarted || touchMoved || touchEnded) {
			bool foundTile = CheckForTile(touchPoint, out tile);
			touchPhaseText.text = "Touch start/move/end " + touchStarted + "/" + touchMoved + "/" + touchEnded;

			// Need a wall tile at start of touch
			if (foundTile) {

				// Check whether we've actually moved between tiles
				Vector3 tilePosition = tile.transform.position;
				if (previousPosition != tilePosition) {
					// Yes - this is a new tile
					TouchableTile.MovementDirection directionMoved =  FindDirection(previousPosition, tilePosition);

					if (touchStarted && currentTouchState == TouchState.NoTouch) {
						// Can we start a touch sequence
						TouchableTile.TileType touchedType;
						if (tile.CommenceTouch(out touchedType)) {
							currentTouchState = TouchState.PathStarted;
							pathTileType = touchedType;
							touchedTiles.Clear();
							touchedTiles.Add(tile);
							previousPosition = tilePosition;
							tile.Highlight(true);
							Debug.Log("GS.instance" + GameState.Instance);
							Debug.Log("GS.instance.soundMgr" + GameState.Instance.soundManager);

							GameState.Instance.soundManager.PlayPathStart();
						} 
					} else if (touchMoved && currentTouchState == TouchState.PathStarted) {
						if (tile.AbortTouch() || !tile.SupportsDirection(directionMoved)) { 
							// stop the line here
							AbortPath();
						} else {
							// We're drawing a line - check whether we already have this one.
							if (!touchedTiles.Contains(tile)) {
								touchedTiles.Add(tile);
								previousPosition = tilePosition;
								Debug.Log("Direction : " + directionMoved);
								tile.Highlight(true);
								GameState.Instance.soundManager.PlayPathExtend();
								scoreA++;
							} 
						}
					} else if (touchEnded && currentTouchState == TouchState.PathStarted) { 
						TouchableTile.TileType endTouchType;

						if (tile.CanFinishTouch(out endTouchType)) {
							if (endTouchType == pathTileType) {
								// This completes the sequence
								foreach (TouchableTile degradeTile in touchedTiles) {
									degradeTile.ApplyTouch();
								}
								touchedTiles.Clear();
								currentTouchState = TouchState.NoTouch; 
								GameState.Instance.soundManager.PlayPathSuccess();
								scoreB += scoreA;
								scoreA = 0;
							} else {
								// Abort the path
								AbortPath();
								currentTouchState = TouchState.NoTouch; 
							}
						}
					}
				} else {
					// Same tile as previous - ignore
				}
			} else {
				AbortPath();
				currentTouchState = TouchState.NoTouch;
			}

			boardPositionText.text = currentTouchState.ToString();

			SetScore();
		}
		
	}

	// Finds the movement direction from the previous tile to the current one
	private TouchableTile.MovementDirection FindDirection(Vector3 previous, Vector3 current) {
		TouchableTile.MovementDirection direction = TouchableTile.MovementDirection.NONE;

		// NB we can only move in one direction at once - diagonals are errors
		float deltaX = current.x - previous.x;
		float deltaY = current.y - previous.y;
		float deltaZ = current.z - previous.z;

		if (deltaX != 0) {
			if (deltaY == 0 && deltaZ == 0) {
				direction = deltaX > 0 ? TouchableTile.MovementDirection.X_INC : TouchableTile.MovementDirection.X_DEC;
			} else {
				direction = TouchableTile.MovementDirection.DIAGONAL;
			}
		} else if (deltaZ != 0) {
			if (deltaY == 0) { 
				direction = deltaZ > 0 ? TouchableTile.MovementDirection.Z_INC : TouchableTile.MovementDirection.Z_DEC;
			} else {
				direction = TouchableTile.MovementDirection.DIAGONAL;
			}
		} else if (deltaY != 0) {
			direction = deltaY > 0 ? TouchableTile.MovementDirection.Y_INC : TouchableTile.MovementDirection.Y_DEC;
		}

		return direction;
	}

	private void AbortPath() {
		// stop the line here
		foreach (TouchableTile touchedTile in touchedTiles) { 
			touchedTile.Highlight(false);
		}
		touchedTiles.Clear();
		currentTouchState = TouchState.NoTouch;
		GameState.Instance.soundManager.PlayPathAbort();
	}

	public void EndLevel() {
		Debug.Log("BoardManager endLevel");
		SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
	}


	private void SetScore() {
		scoreAText.text = scoreA.ToString();
		scoreBText.text = scoreB.ToString();
	}
}
