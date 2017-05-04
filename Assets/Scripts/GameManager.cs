using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	// Row + Column display text
	public Text rowText;
	private Text columnText;

	// Row + Column sliders
	private Slider rowSlider;
	private Slider columnSlider;

	private Toggle efxToggle;
	private Toggle musicToggle;

	private Text scoreAText;
	private Text scoreBText;

	private int scoreA = 0;
	private int scoreB = 0;

	// Use this for initialization
	void Awake () {
		InitialGameState initialGameState = GetComponent<InitialGameState> ();
		GameState.Instance.Initialise (initialGameState);

		Debug.Log("Getting rows + columns");
//		rowText = GameObject.Find("RowText").GetComponent<Text>();
		columnText = GameObject.Find("ColumnText").GetComponent<Text>();
		rowSlider = GameObject.Find("RowSlider").GetComponent<Slider>();
		columnSlider = GameObject.Find("ColumnSlider").GetComponent<Slider>();

		efxToggle = GameObject.Find ("EfxToggle").GetComponent<Toggle> ();
		musicToggle = GameObject.Find ("MusicToggle").GetComponent<Toggle> ();


	
		InitMenu();
	}
		
	// Initialise the menu scene
	void InitMenu() {
		Debug.Log("InitMenu");

		// Programatically attach to the controls
		Debug.Log("columnSlider is " + columnSlider.ToString());
		columnSlider.onValueChanged.RemoveAllListeners();
		columnSlider.onValueChanged.AddListener (delegate {	ColumnValueChanged (); });
		rowSlider.onValueChanged.RemoveAllListeners ();
		rowSlider.onValueChanged.AddListener (delegate { RowValueChanged ();});

		// Set the sound state
		efxToggle.isOn = GameState.Instance.soundManager.efxEnabled;
		musicToggle.isOn = GameState.Instance.soundManager.musicEnabled;

		UpdateRowsAndColumns();
	}



	public void StartLevel() {
		Debug.Log("GameManager starting board scene");
		// Application.LoadLevel(sceneIndex);
		SceneManager.LoadScene("BoardScene", LoadSceneMode.Single);

	}


	public void Start3dLevel() { 
		Debug.Log("Gamemanager starting 3d scene");
		SceneManager.LoadScene("Board3D", LoadSceneMode.Single);
	}

	public void RowValueChanged() {
		GameState.Instance.rows = (int)rowSlider.value;
		UpdateRowsAndColumns();
		Debug.Log("UpdateRows to " + GameState.Instance.rows);
	}

	public void ColumnValueChanged() { 
		GameState.Instance.columns = (int)columnSlider.value;
		UpdateRowsAndColumns();
		Debug.Log("UpdateColumns to " + GameState.Instance.columns);
	}

	private void UpdateRowsAndColumns() { 
		rowText.text = GameState.Instance.rows.ToString();	
		rowSlider.value = GameState.Instance.rows;
		columnText.text = GameState.Instance.columns.ToString();
		columnSlider.value = GameState.Instance.columns;

	}

	public void EnableSoundEffects(bool enabled) {
		GameState.Instance.soundManager.EnableEfx (enabled);
	}

	public void EnableMusic(bool enabled) {
		GameState.Instance.soundManager.EnableMusic (enabled);
	}
}
