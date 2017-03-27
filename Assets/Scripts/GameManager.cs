using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public BoardManager boardScript;

	public CameraControl cameraControlScript;

	// time to display level text
	public float levelStartDelay = 2f;

	private Text levelText;

	// Reference to cover image
	private GameObject levelImage;

	// Main menu scene name
	public string menuSceneName;

	// Game scene name
	public string mainSceneName;

	// Public Variables
	private int columns = 8;
	private int rows = 8;

	// Row + Column display text
	private Text rowText;
	private Text columnText;

	// Row + Column sliders
	private Slider rowSlider;
	private Slider columnSlider;

	// Ensure we're a singleton
	public static GameManager instance = null;

	private int level = 1;

	// Use this for initialization
	void Awake () {

		Debug.Log("GameManager is awake. Instance is null ? " + (instance == null));

		if (instance == null) {
			instance = this;
			// InitMenu();
		}

		// Persist game manager between scenes
		DontDestroyOnLoad(gameObject);

		// Get reference to board manager
		boardScript = GetComponent<BoardManager>();

		// Get reference to Camera Control
		cameraControlScript = GetComponent<CameraControl>();

		SceneManager.activeSceneChanged += ChangedScene;

		// Text for rows + columns
		Debug.Log("Getting rows + columns");
		rowText = GameObject.Find("RowText").GetComponent<Text>();
		columnText = GameObject.Find("ColumnText").GetComponent<Text>();
		rowSlider = GameObject.Find("RowSlider").GetComponent<Slider>();
		columnSlider = GameObject.Find("ColumnSlider").GetComponent<Slider>();
	
	}

	void ChangedScene(Scene previousScene, Scene newScene) {
		Debug.Log("ActiveSceneChanged was " + previousScene.name + " to " + newScene.name);

		if (newScene.name.Equals(mainSceneName)) {
			// The game board was loaded so display it
			InitGame();
		} else {
			// Set the menu back up 
			InitMenu();
		}
	}

	// Initialise the menu scene
	void InitMenu() {
//		SceneManager.LoadScene(menuSceneName);
		Debug.Log("InitMenu");
		UpdateRowsAndColumns();
	}

	// Initialise the game board 
	void InitGame() {
		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		levelText.text = "Level: " + level;
		levelImage.SetActive(true);
		Invoke("HideLevelImage", levelStartDelay);

		boardScript.SetupScene(level, rows, columns);
		boardScript.EnableInput(true);
		cameraControlScript.EnableControls(true);
	}

	public void StartLevel() {
		Debug.Log("StartLevel: " + mainSceneName);
		// Application.LoadLevel(sceneIndex);
		SceneManager.LoadScene(mainSceneName, LoadSceneMode.Single);

	}

	public void EndLevel() {
		Debug.Log("GameManager endLevel");
		if (instance != null) {
			Debug.Log("EndLevel: " + instance.menuSceneName);
			instance.boardScript.EnableInput(false);
			SceneManager.LoadScene(instance.menuSceneName, LoadSceneMode.Single);
		} else { 
			Debug.LogError("GameManager instance is null");
		}
	}

	private void HideLevelImage() {
		levelImage.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void UpdateRows(Slider rowSlider) {
		instance.rows = (int)rowSlider.value;
		UpdateRowsAndColumns();
		// Debug.Log("UpdateRows to " + rows);
	}

	public void UpdateColumns(Slider columnSlider) { 
		instance.columns = (int)columnSlider.value;
		UpdateRowsAndColumns();
		// Debug.Log("UpdateColumns to " + columns);
	}

	private void UpdateRowsAndColumns() { 
		rowText.text = instance.rows.ToString();	
		rowSlider.value = instance.rows;
		columnText.text = instance.columns.ToString();
		columnSlider.value = instance.columns;

	}

}
