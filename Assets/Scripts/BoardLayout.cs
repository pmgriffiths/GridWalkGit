using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLayout : MonoBehaviour {

	public TouchableTile[] floorTiles;
	public TouchableTile[] outerWallTiles;

	// NB. This array MUST be be same length at TileType and populated by prefabs in the same order. 
	// Ouch - tight, tight coupling.
	public TouchableTile[] availableTileTypes;

	public enum TileType { Floor_0, Floor_1, Floor_2, Floor_3, Floor_X, Floor_Z, Wall_0, Wall_1};

	/**
	 * Floor_0 = 0,  .... Floor_X = 4, Floor_Y = 5, Wall_0 = 6, Wall_1 = 8 **/

	private class BoardTemplate { 
		public int rows  { get; set; }

		public int columns { get; set; }

		public int[][] template { get; set; }

		public BoardTemplate(int rows, int columns, int[][] template) {
			this.rows = rows;
			this.columns = columns; 
			this.template = template;
		}
	}

	private static int[][] minimal = new int[][] {
		new int[] {6, 6, 6}, 
		new int[] {6, 0, 6},
		new int[] {6, 6, 6}
	};


	private static int[][] four = new int[][] {
		new int[] {6, 6, 6, 6}, 
		new int[] {6, 0, 4, 6},
		new int[] {6, 6, 6, 6}
	};

	private static int[][] eightByFour = new int[][] {
		new int[] {6, 6, 6, 6, 6, 6, 6, 6}, 
		new int[] {6, 0, 4, 5, 0, 2, 4, 6},
		new int[] {6, 4, 0, 0, 4, 4, 4, 6},
		new int[] {6, 6, 6, 6, 6, 6, 6, 6}, 
	};

	private static int[][] fourBySix = new int[][] {
		new int[] {6, 6, 6, 6}, 
		new int[] {6, 0, 4, 6},
		new int[] {6, 0, 5, 6},
		new int[] {6, 0, 5, 6},
		new int[] {6, 0, 5, 6},
		new int[] {6, 6, 6, 6}
	};


	private Dictionary<int, BoardTemplate> levels = new Dictionary<int, BoardTemplate>();

	private void InitLevels() {
		if (levels.Count == 0) {
			levels.Add(0, new BoardTemplate(3, 3, minimal));
			levels.Add(1, new BoardTemplate(4, 4, four));
			levels.Add(2, new BoardTemplate(8, 4, eightByFour));
			levels.Add(3, new BoardTemplate(4, 6, fourBySix));
		}
	} 


	// Create a board for the level specified
	public void CreateLevel(Transform boardHolder, int level, Dictionary<Vector3, TouchableTile> tilePositions)
	{
		InitLevels();

		if (level < 0 || level >= levels.Count) {
			level = 0;
		}

		BoardTemplate levelLayout = levels[level];

		// Build the outer wall and floor tiles
		for (int x = 0; x < levelLayout.columns; x++) {
			for (int y = 0; y < levelLayout.rows; y++) {
				TouchableTile toInstantiate = availableTileTypes[(int)levelLayout.template[x][y]];

				// Quaternion.identity means no rotation
				Vector3 objectTransform = new Vector3(x, 0f, y);
				TouchableTile tileInstance = Instantiate(toInstantiate, objectTransform, Quaternion.identity);
				tileInstance.transform.SetParent(boardHolder);

				// Store the object in the game dictionary
				tilePositions.Add(objectTransform, tileInstance);

			}
		}
	}


	public void SetupBoard(Transform boardHolder, int rows, int columns, Dictionary<Vector3, TouchableTile> tilePositions)
	{
		// Build the outer wall and floor tiles
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				TouchableTile toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
				// Check for outer walls
				Debug.Log("OutWalls: " + outerWallTiles.Length);
				if (x == -1 || x == columns || y == -1 || y == rows) {
					// Replace object with outer wall object
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
					Debug.Log("WallType: " + toInstantiate.tileType);
				}

				// Quaternion.identity means no rotation
				Vector3 objectTransform = new Vector3(x, 0f, y);
				TouchableTile tileInstance = Instantiate(toInstantiate, objectTransform, Quaternion.identity);
				tileInstance.transform.SetParent(boardHolder);

				// Store the object in the game dictionary
				tilePositions.Add(objectTransform, tileInstance);

			}
		}
	}

}
