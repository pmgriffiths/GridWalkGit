using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	private CameraControl cameraControlScript;

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

	public TouchableTile[] floorTiles;
	public TouchableTile[] outerWallTiles;

	// Keeps hierarchy clean using this as parent of tiles
	private Transform boardHolder;

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

		// Get reference to Camera Control
		cameraControlScript = GetComponent<CameraControl>();

		// take our state from the game state
		rows = GameState.Instance.rows;
		columns = GameState.Instance.columns;
		level = ++GameState.Instance.level;

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

		levelImage = GameObject.Find("LevelImage");
		GameObject levelTextObj = GameObject.Find ("LevelText");
		Debug.Log ("Level text obj is " + levelTextObj);
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		levelText.text = "Level "+ level;
		levelImage.SetActive(true);
		Invoke("HideLevelImage", levelStartDelay);

		SetupScene(level, rows, columns);
		cameraControlScript.EnableControls(true);
	} 

	// Sets up the board
	void BoardSetup() 
	{
		Debug.Log("BoardManager boardSetup for rows " + rows + ", columns " + columns);

		boardHolder = new GameObject("Board").transform;


		// Build the outer wall and floor tiles
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				TouchableTile toInstantiate = floorTiles[floorTiles.Length - 1];
				// Check for outer walls
				if (x == -1 || x == columns || y == -1 || y == rows) {
					// Replace object with outer wall object
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}

				// Quaternion.identity means no rotation
				Vector3 objectTransform = new Vector3(x, y, 0f);
				TouchableTile tileInstance = Instantiate(toInstantiate, objectTransform, Quaternion.identity);
				tileInstance.transform.SetParent(boardHolder);

				// Store the object in the game dictionary
				tilePositions.Add(objectTransform, tileInstance);

			}
				
		} 

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
		
	private bool CheckForTile(Touch touch, out TouchableTile tile) {
		bool haveTile = false;
		tile = null;

		touchOrigin = touch.position;
		touchPositionText.text = "Touch position: " + touchOrigin.x + ", " + touchOrigin.y;

		Vector3 touchTarget = Camera.main.ScreenToWorldPoint(touchOrigin);

		// map from world to game position
		int touchX = (int)Mathf.Round(touchTarget.x);
		int touchY = (int)Mathf.Round(touchTarget.y);
		touchTargetText.text = "Touch Target: " + touchX+ ", " + touchY;

		// Check for a tile at this position and touch it
		Vector3 location = new Vector3(touchX, touchY, 0);

		if (tilePositions.TryGetValue(location, out tile)) {
			touchPositionText.text = "Got it";	
			haveTile = true;
		}

		return haveTile;
	}
		

	// Method for removing splash image
	private void HideLevelImage() {
		levelImage.SetActive(false);
	}


	private void Update() {

		#if UNITY_IOS 
		if (Input.touchCount > 0) {
			Touch myTouch = Input.touches[0];
			TouchableTile tile;
			bool foundTile = CheckForTile(myTouch, out tile);

			touchPhaseText.text = "Touch Phase: " + myTouch.phase.ToString();

			// Need a wall tile at start of touch
			if (foundTile) {

				if (myTouch.phase == TouchPhase.Began && currentTouchState == TouchState.NoTouch) {
					// Can we start a touch sequence
					TouchableTile.TileType touchedType;
					if (tile.CommenceTouch(out touchedType)) {
						currentTouchState = TouchState.PathStarted;
						pathTileType = touchedType;
						touchedTiles.Clear();
						touchedTiles.Add(tile);
						tile.UpdateOutline(true);
						GameState.Instance.soundManager.PlayPathStart();
					} 
				} else if (myTouch.phase == TouchPhase.Moved && currentTouchState == TouchState.PathStarted) {
					if (tile.AbortTouch()) { 
						// stop the line here
						AbortPath();
					} else {
						// We're drawing a line
						touchedTiles.Add(tile);
						tile.UpdateOutline(true);
						GameState.Instance.soundManager.PlayPathExtend();
					}
				} else if (myTouch.phase == TouchPhase.Ended && currentTouchState == TouchState.PathStarted) { 
					TouchableTile.TileType endTouchType;

					if (tile.CanFinishTouch(out endTouchType)) {
						if (endTouchType == pathTileType) {
							// This completes the sequence
							foreach (TouchableTile degradeTile in touchedTiles) {
								degradeTile.OnTouch();
								degradeTile.UpdateOutline(false);
							}
							touchedTiles.Clear();
							currentTouchState = TouchState.NoTouch; 
							GameState.Instance.soundManager.PlayPathSuccess();
						} else {
							// Abort the path
							AbortPath();
						}
					}
				}


				boardPositionText.text = currentTouchState.ToString();
									
						
/**				if (myTouch.phase == TouchPhase.Ended && foundTile) 
				{
						touchPositionText.text = "Got it";	
						tile.OnTouch();
					} else {
						touchPositionText.text = "No tiles at position";
				} */

			}
		}


		#endif		
	}

	private void AbortPath() {
		// stop the line here
		foreach (TouchableTile touchedTile in touchedTiles) { 
			touchedTile.UpdateOutline(false);
		}
		touchedTiles.Clear();
		currentTouchState = TouchState.NoTouch;
		GameState.Instance.soundManager.PlayPathAbort();;
	}

	public void EndLevel() {
		Debug.Log("BoardManager endLevel");
		SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
	}

}
