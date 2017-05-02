using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLayout : MonoBehaviour {

	public TouchableTile[] floorTiles;
	public TouchableTile[] outerWallTiles;

	public void SetupBoard(Transform boardHolder, int rows, int columns, Dictionary<Vector3, TouchableTile> tilePositions)
	{
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
				Vector3 objectTransform = new Vector3(x, 0f, y);
				TouchableTile tileInstance = Instantiate(toInstantiate, objectTransform, Quaternion.identity);
				tileInstance.transform.SetParent(boardHolder);

				// Store the object in the game dictionary
				tilePositions.Add(objectTransform, tileInstance);

			}
		}
	}

}
