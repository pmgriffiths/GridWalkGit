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


	// Test change for git hub

	private Text levelText;

	// Reference to cover image
	private GameObject levelImage;

	// Main menu scene name
	public string menuSceneName;

	// Game scene name
	public string mainSceneName;

	// Are the in the menu
	private bool displayMenu = true;

	// Public Variables
	private int columns = 8;
	private int rows = 8;

	// Ensure we're a singleton
	public static GameManager instance = null;

	private int level = 1;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
			InitMenu();
		} else if (instance != null) {
			// Destroy(gameObject);
		}

		// Persist game manager between scenes
		DontDestroyOnLoad(gameObject);

		// Get reference to board manager
		boardScript = GetComponent<BoardManager>();

		// Get reference to Camera Control
		cameraControlScript = GetComponent<CameraControl>();

		SceneManager.activeSceneChanged += ChangedScene;

/**		if (displayMenu) {
			InitMenu();
		} else {
			InitGame();
		} */
	}

	void ChangedScene(Scene previousScene, Scene newScene) {
		Debug.Log("ActiveSceneChanged was " + previousScene.name + " to " + newScene.name);

		if (newScene.name.Equals(mainSceneName)) {
			// The game board was loaded so display it
			InitGame();
		}
	}

/**	private void OnLevelWasLoaded(int index)
	{
		Debug.Log("GameManger.OnLevelWasLoaded displayMenu" + displayMenu);
		if (SceneManager.GetActiveScene().name.Equals(mainSceneName)) {
			level++;
			InitGame();
		}
	} 
*/

	// Initialise the menu scene
	void InitMenu() {
//		SceneManager.LoadScene(menuSceneName);
		Debug.Log("InitMenu called and change to scene: " + menuSceneName);
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
		displayMenu = false;
		SceneManager.LoadScene(mainSceneName, LoadSceneMode.Single);

	}

	public void EndLevel() {
		// Application.LoadLevel(sceneIndex);
		Debug.Log("EndLevel: " + menuSceneName);
		displayMenu = true;
		boardScript.EnableInput(false);
		SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
	}

	private void HideLevelImage() {
		levelImage.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void UpdateRows(Slider rowSlider) {
		this.rows = (int)rowSlider.value;
		Debug.Log("UpdateRows to " + rows);
	}

	public void UpdateColumns(Slider columnSlider) { 
		this.columns = (int)columnSlider.value;
		Debug.Log("UpdateColumns to " + columns);
	}

}
