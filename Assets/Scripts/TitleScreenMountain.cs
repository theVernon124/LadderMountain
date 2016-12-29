using UnityEngine;
using System.Collections;

public class TitleScreenMountain : MonoBehaviour
{
	public GameObject[] mountainTilesBody;
	public GameObject[] mountainTilesLeft;
	public GameObject[] mountainTilesRight;
	public GameObject[] cliffTiles;
	public GameObject[] ladderTiles;
	public int columns;
	public int rows;

	void Awake ()
	{
		GenerateMountain ();
	}

	void GenerateMountain ()
	{
		GenerateMountainFace ();
		GenerateCliffs ();
		GenerateLadders ();
	}

	void GenerateMountainFace ()
	{
		GameObject mountainTileInstance;
		GameObject randomTile;

		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				if (x == 0) {
					randomTile = mountainTilesLeft [Random.Range (0, mountainTilesLeft.Length)];
					mountainTileInstance = Instantiate (randomTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				} else if (x == columns - 1) {
					randomTile = mountainTilesRight [Random.Range (0, mountainTilesRight.Length)];
					mountainTileInstance = Instantiate (randomTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				} else {
					randomTile = mountainTilesBody [Random.Range (0, mountainTilesBody.Length)];
					mountainTileInstance = Instantiate (randomTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				}

				mountainTileInstance.transform.SetParent (this.gameObject.transform);
			}
		}
	}

	void GenerateCliffs ()
	{
		GameObject CliffTileInstance;
		GameObject randomTile;

		for (int y = 0; y < rows; y += 3) {
			for (int x = 0; x < columns; x++) {
				if ((int)Random.Range (0, 2) == 1) {
					randomTile = cliffTiles [Random.Range (0, cliffTiles.Length)];
					CliffTileInstance = Instantiate (randomTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					CliffTileInstance.transform.SetParent (this.gameObject.transform);
				}
			}
		}
	}

	void GenerateLadders ()
	{
		GameObject LadderTileInstance;
		GameObject randomTile;

		for (int y = 1; y < rows; y += 3) {
			for (int x = 0; x < columns; x++) {
				if ((int)Random.Range (0, 4) == 1) {
					randomTile = ladderTiles [Random.Range (0, ladderTiles.Length)];
					LadderTileInstance = Instantiate (randomTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					LadderTileInstance.transform.SetParent (this.gameObject.transform);
				}
			}
		}
	}

}
