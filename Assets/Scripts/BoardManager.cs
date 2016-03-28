using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	// Dimensions of the play area is set here.
	public int columns = 8;
	public int rows = 16;
	public int cliffRowOffset = 3;
	public Count disabledCliffCount = new Count (1, 7);
	public GameObject[] cliffTilesBody;
	public GameObject[] cliffTilesLeft;
	public GameObject[] cliffTilesRight;
	public GameObject[] cliffTilesSolo;
	public GameObject[] mountainTilesBody;
	public GameObject[] mountainTilesLeft;
	public GameObject[] mountainTilesRight;
	public GameObject[]	ladderTiles;
	public Sprite[]	cliffSprites;
	public GameObject mainBoardHolder;
	public GameObject subBoardHolder;

	private GameObject cliffRow1;
	private GameObject cliffRow2;
	private GameObject cliffRow3;
	private List<GameObject> cliffTilePositions = new List<GameObject> ();
	private List<GameObject> ladderTilePositions = new List<GameObject> ();
	private int leftmostCliffIndex;
	private int rightmostCliffIndex;
	private int preDisabledLadders;
	private int happyPathSeed;
	private int happyPathIndex;

	public void SetupScene ()
	{
		BoardSetup ();
	}

	// This generates the mountain tiles, the initial cliffs, and the ladders
	void BoardSetup ()
	{
		cliffSprites = Resources.LoadAll<Sprite> ("Sprites/LMSpriteSheet2");
		//	Tileboards to hold the mountain tiles are spawned here. 2 tileboards are spawned to facilitate an endless, seamless background
		mainBoardHolder = Instantiate (Resources.Load ("Prefabs/TileBoard", typeof(GameObject)), new Vector3 (0, 0, 0f), Quaternion.identity) as GameObject;
		subBoardHolder = Instantiate (Resources.Load ("Prefabs/TileBoard", typeof(GameObject)), new Vector3 (0, 8, 0f), Quaternion.identity) as GameObject;

		//	4 initial cliff rows are spawned here, each with their own ladder rows. Each element in a cliff/ladder row is initially active.
		GameObject cliffRow1 = Instantiate (Resources.Load ("Prefabs/CliffRow", typeof(GameObject)), new Vector3 (0, 0, 0f), Quaternion.identity) as GameObject;
		GameObject ladderRow1 = Instantiate (Resources.Load ("Prefabs/LadderRow", typeof(GameObject)), new Vector3 (0, cliffRow1.transform.position.y + 1, 0f), Quaternion.identity) as GameObject;
		ladderRow1.transform.SetParent (cliffRow1.transform);

		GameObject cliffRow2 = Instantiate (Resources.Load ("Prefabs/CliffRow", typeof(GameObject)), new Vector3 (0, cliffRow1.transform.position.y + cliffRowOffset, 0f), Quaternion.identity) as GameObject;
		GameObject ladderRow2 = Instantiate (Resources.Load ("Prefabs/LadderRow", typeof(GameObject)), new Vector3 (0, cliffRow2.transform.position.y + 1, 0f), Quaternion.identity) as GameObject;
		ladderRow2.transform.SetParent (cliffRow2.transform);

		GameObject cliffRow3 = Instantiate (Resources.Load ("Prefabs/CliffRow", typeof(GameObject)), new Vector3 (0, cliffRow2.transform.position.y + cliffRowOffset, 0f), Quaternion.identity) as GameObject;
		GameObject ladderRow3 = Instantiate (Resources.Load ("Prefabs/LadderRow", typeof(GameObject)), new Vector3 (0, cliffRow3.transform.position.y + 1, 0f), Quaternion.identity) as GameObject;
		ladderRow3.transform.SetParent (cliffRow3.transform);

		GameObject cliffRow4 = Instantiate (Resources.Load ("Prefabs/CliffRow", typeof(GameObject)), new Vector3 (0, cliffRow3.transform.position.y + cliffRowOffset, 0f), Quaternion.identity) as GameObject;
		GameObject ladderRow4 = Instantiate (Resources.Load ("Prefabs/LadderRow", typeof(GameObject)), new Vector3 (0, cliffRow4.transform.position.y + 1, 0f), Quaternion.identity) as GameObject;
		ladderRow4.transform.SetParent (cliffRow4.transform);

		//	This loop generates the mountain and cliff tiles based on the game area dimensions (rows, columns) and assigns them to their respective parents/holders
		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				//	Mountain tiles are spawned here and assigned to the 2 tile boards.
				GameObject mountainTileInstance;

				if (x == 0) {
					GameObject origMountainTile = mountainTilesLeft [Random.Range (0, mountainTilesLeft.Length)];
					mountainTileInstance = Instantiate (origMountainTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				} else if (x == columns - 1) {
					GameObject origMountainTile = mountainTilesRight [Random.Range (0, mountainTilesRight.Length)];
					mountainTileInstance = Instantiate (origMountainTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				} else {
					GameObject origMountainTile = mountainTilesBody [Random.Range (0, mountainTilesBody.Length)];
					mountainTileInstance = Instantiate (origMountainTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				}

				if (y < 8) {
					mountainTileInstance.transform.SetParent (mainBoardHolder.transform);
				} else {
					mountainTileInstance.transform.SetParent (subBoardHolder.transform);
				}

				//	Cliff tiles are spawned here and assigned to respective cliff rows. Cliff rows are set at definite row intervals.
				if (y == 0) {
					GameObject origCliffTile;
					if (x == 0) {
						origCliffTile = cliffTilesLeft [Random.Range (0, cliffTilesLeft.Length)];
					} else if (x == columns - 1) {
						origCliffTile = cliffTilesRight [Random.Range (0, cliffTilesRight.Length)];
					} else {
						origCliffTile = cliffTilesBody [Random.Range (0, cliffTilesBody.Length)];
					}
					GameObject cliffTileInstance = Instantiate (origCliffTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					cliffTileInstance.transform.SetParent (cliffRow1.transform);
				} else if (y == 3) {
					GameObject origCliffTile = cliffTilesBody [Random.Range (0, cliffTilesBody.Length)];
					GameObject cliffTileInstance = Instantiate (origCliffTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					cliffTileInstance.transform.SetParent (cliffRow2.transform);
				} else if (y == 6) {
					GameObject origCliffTile = cliffTilesBody [Random.Range (0, cliffTilesBody.Length)];
					GameObject cliffTileInstance = Instantiate (origCliffTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					cliffTileInstance.transform.SetParent (cliffRow3.transform);
				} else if (y == 9) {
					GameObject origCliffTile = cliffTilesBody [Random.Range (0, cliffTilesBody.Length)];
					GameObject cliffTileInstance = Instantiate (origCliffTile, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					cliffTileInstance.transform.SetParent (cliffRow4.transform);
				}
			}
		}

		happyPathSeed = -1;
		happyPathIndex = -1;
		InitialLadderRow (ladderRow1);
		RandomizeCliffs (cliffRow2, ladderRow2);
		RandomizeCliffs (cliffRow3, ladderRow3);
		RandomizeCliffs (cliffRow4, ladderRow4);
	}

	void InitialLadderRow (GameObject ladderRow)
	{
		InitialiseLadderTilePositions (ladderRow);
		int disabledLadderTileCount = Random.Range (columns / 2, columns);
		for (int i = 0; i < disabledLadderTileCount; i++) {
			GameObject randomladderTile = RandomizeLadderTilePosition (ladderTilePositions);
			randomladderTile.SetActive (false);
		}
	}

	public void RandomizeCliffs (GameObject cliffRow, GameObject ladderRow)
	{
		InitialiseCliffTilePositions (cliffRow);
		InitialiseLadderTilePositions (ladderRow);
		RandomizeCliffRow (cliffTilePositions, disabledCliffCount.minimum, disabledCliffCount.maximum, ladderTilePositions);
	}

	// This generates a list of all possible tiles in the game area
	void InitialiseCliffTilePositions (GameObject cliffRow)
	{
		cliffTilePositions.Clear ();
		Transform cliffRowTransform = cliffRow.transform;

		foreach (Transform child in cliffRowTransform) {
			if (!child.gameObject.CompareTag ("LadderRow")) {
				cliffTilePositions.Add (child.gameObject);
				child.gameObject.SetActive (true);
				child.gameObject.GetComponent<SpriteRenderer> ().sprite = cliffSprites [Random.Range (1, 4)];
			}
		}

		leftmostCliffIndex = 0;
		rightmostCliffIndex = columns - 1;
	}

	void InitialiseLadderTilePositions (GameObject ladderRow)
	{
		ladderTilePositions.Clear ();
		Transform ladderRowTransform = ladderRow.transform;

		foreach (Transform child in ladderRowTransform) {
			ladderTilePositions.Add (child.gameObject);
			child.gameObject.SetActive (true);
		}

		preDisabledLadders = 0;
	}

	void RandomizeCliffRow (List<GameObject> cliffTilePositions, int minCliff, int maxCliff, List<GameObject> ladderTilePositions)
	{
		happyPathIndex = RandomizeHappyPathIndex (happyPathSeed);

		int disabledCliffTileCount = Random.Range (minCliff, maxCliff + 1);
		for (int i = 0; i < disabledCliffTileCount; i++) {
			GameObject randomCliffTile = RandomizeTilePositions (cliffTilePositions, ladderTilePositions); //include happy path index as parameter here
			randomCliffTile.SetActive (false);
		}
		cliffSpriteSetter (cliffTilePositions);

		int disabledLadderTileCount;
		if (disabledCliffTileCount == 7) {
			disabledLadderTileCount = 7;
		} else {
			disabledLadderTileCount = Random.Range (((columns - disabledCliffTileCount) / 2) + disabledCliffTileCount, columns);
		}
		for (int i = preDisabledLadders + 1; i <= disabledLadderTileCount; i++) {
			GameObject randomladderTile = RandomizeLadderTilePosition (ladderTilePositions);
			randomladderTile.SetActive (false);
		}

		happyPathSeed = happyPathIndex;
	}

	GameObject RandomizeTilePositions (List<GameObject> cliffTilePositions, List<GameObject> ladderTilePositions)
	{
		GameObject randomTile = null;
		while (randomTile == null) {
			int randomIndex = Random.Range (leftmostCliffIndex, rightmostCliffIndex + 1);

			if (randomIndex != happyPathIndex) {
				if (randomIndex == leftmostCliffIndex) {
					randomTile = cliffTilePositions [randomIndex];

					ladderTilePositions [randomIndex].gameObject.SetActive (false);
					preDisabledLadders++;

					AssignLeftmostCliff (cliffTilePositions, randomIndex);
				} else if (randomIndex == rightmostCliffIndex) {
					randomTile = cliffTilePositions [randomIndex];
					ladderTilePositions [randomIndex].gameObject.SetActive (false);
					preDisabledLadders++;

					AssignRightmostCliff (cliffTilePositions, randomIndex);
				} else {
					GameObject leftRandomTile = cliffTilePositions [randomIndex - 1];
					GameObject rightRandomTile = cliffTilePositions [randomIndex + 1];

					if (leftRandomTile.activeSelf && rightRandomTile.activeSelf) {
						randomTile = cliffTilePositions [randomIndex];
						if (!randomTile.activeSelf) {
							randomTile = null;
						}
						if (ladderTilePositions [randomIndex].gameObject.activeSelf) {
							ladderTilePositions [randomIndex].gameObject.SetActive (false);
							preDisabledLadders++;
						}
					}
				}
			}
		} 

		return randomTile;
	}

	GameObject RandomizeLadderTilePosition (List<GameObject> ladderTilePositions)
	{
		GameObject randomTile = null;

		while (randomTile == null) {
			int randomIndex = Random.Range (0, ladderTilePositions.Count);
			if (happyPathIndex < 0) {
				happyPathIndex = randomIndex;
				happyPathSeed = randomIndex;
			}

			randomTile = ladderTilePositions [randomIndex];

			if (!randomTile.activeSelf || randomIndex == happyPathIndex) {
				randomTile = null;
			}

//			ladderTilePositions.RemoveAt (randomIndex);
		} 

		return randomTile;
	}

	void AssignLeftmostCliff (List<GameObject> cliffTilePositions, int currentLeftmostCliffIndex)
	{
		for (int i = currentLeftmostCliffIndex + 1; i < cliffTilePositions.Count; i++) {
			GameObject leftmostCandidate = cliffTilePositions [i];
			if (leftmostCandidate.activeSelf) {
				leftmostCliffIndex = i;
				break;
			}
		}
	}

	void AssignRightmostCliff (List<GameObject> cliffTilePositions, int currentRightmostCliffIndex)
	{
		for (int i = currentRightmostCliffIndex - 1; i >= 0; i--) {
			GameObject rightmostCandidate = cliffTilePositions [i];
			if (rightmostCandidate.activeSelf) {
				rightmostCliffIndex = i;
				break;
			}
		}
	}

	int RandomizeHappyPathIndex (int happyPathSeed)
	{
		List<int> happyPathIndexes = new List<int> ();

		for (int i = happyPathSeed - 2; i <= happyPathSeed + 2; i++) {
			if (i >= 0 && i <= 7) {
				happyPathIndexes.Add (i);
			}
		}

		int randomIndex = Random.Range (0, happyPathIndexes.Count);
		int happyPathIndex = happyPathIndexes [randomIndex];

		return happyPathIndex;
	}

	void cliffSpriteSetter (List<GameObject> cliffTilePositions)
	{
		for (int i = 0; i < cliffTilePositions.Count; i++) {
			if (cliffTilePositions [i].activeSelf) {
				if (i == 0 && cliffTilePositions [i + 1].gameObject.activeSelf) {
					cliffTilePositions [i].gameObject.GetComponent<SpriteRenderer> ().sprite = cliffSprites [0];
				} else if (i == 0 && !cliffTilePositions [i + 1].gameObject.activeSelf) {
					cliffTilePositions [i].gameObject.GetComponent<SpriteRenderer> ().sprite = cliffSprites [5];
				} else if (i == cliffTilePositions.Count - 1 && !cliffTilePositions [i - 1].gameObject.activeSelf) {
					cliffTilePositions [i].gameObject.GetComponent<SpriteRenderer> ().sprite = cliffSprites [5];
				} else if (i == cliffTilePositions.Count - 1 && cliffTilePositions [i - 1].gameObject.activeSelf) {
					cliffTilePositions [i].gameObject.GetComponent<SpriteRenderer> ().sprite = cliffSprites [4];
				} else if (cliffTilePositions [i - 1].gameObject.activeSelf && !cliffTilePositions [i + 1].gameObject.activeSelf) {
					cliffTilePositions [i].gameObject.GetComponent<SpriteRenderer> ().sprite = cliffSprites [4];
				} else if (!cliffTilePositions [i - 1].gameObject.activeSelf && !cliffTilePositions [i + 1].gameObject.activeSelf) {
					cliffTilePositions [i].gameObject.GetComponent<SpriteRenderer> ().sprite = cliffSprites [5];
				} else if (!cliffTilePositions [i - 1].gameObject.activeSelf && cliffTilePositions [i + 1].gameObject.activeSelf) {
					cliffTilePositions [i].gameObject.GetComponent<SpriteRenderer> ().sprite = cliffSprites [0];
				}
			}
		}
	}
}
