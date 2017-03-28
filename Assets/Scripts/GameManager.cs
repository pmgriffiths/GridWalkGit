using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Row + Column display text
	private Text rowText;
	private Text columnText;

	// Row + Column sliders
	private Slider rowSlider;
	private Slider columnSlider;

	// Ensure we're a singleton
//	public static GameManager instance = null;

	private int level = 1;
		

	// Use this for initialization
	void Awake () {
		Debug.Log("Getting rows + columns");
		rowText = GameObject.Find("RowText").GetComponent<Text>();
		columnText = GameObject.Find("ColumnText").GetComponent<Text>();
		rowSlider = GameObject.Find("RowSlider").GetComponent<Slider>();
		columnSlider = GameObject.Find("ColumnSlider").GetComponent<Slider>();
	
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

		UpdateRowsAndColumns();
	}



	public void StartLevel() {
		Debug.Log("GameManager starting board scene");
		// Application.LoadLevel(sceneIndex);
		SceneManager.LoadScene("BoardScene", LoadSceneMode.Single);

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
}
