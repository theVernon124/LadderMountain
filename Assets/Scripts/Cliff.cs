using UnityEngine;
using System.Collections;

public class Cliff : MonoBehaviour
{
	public float yOffset = 12f;
	private BoardManager boardScript;
	private Camera cam;

	void Awake ()
	{
		boardScript = GameObject.FindObjectOfType<BoardManager> () as BoardManager;
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 viewPos = cam.WorldToViewportPoint (this.gameObject.transform.position + new Vector3 (0, 4, 0));
		if (viewPos.y < 0) {
			ResetPos (yOffset);

			if (!GameManager.instance.bgIsVisible && !this.gameObject.transform.GetChild (0).gameObject.activeSelf) {
				this.gameObject.transform.GetChild (0).gameObject.SetActive (true);
			}
			if (GameManager.instance.canSpawnAirplanes && !this.gameObject.transform.GetChild (1).gameObject.activeSelf && this.gameObject.transform.GetChild (1).CompareTag ("Airplane")) {
				this.gameObject.transform.GetChild (1).gameObject.SetActive (true);
			}
		}
	}

	void ResetPos (float yPos)
	{
		if (GameManager.instance.disableCloudsAndPlanes) {
			this.gameObject.transform.GetChild (0).gameObject.SetActive (false);
			this.gameObject.transform.GetChild (1).gameObject.SetActive (false);
		}
		this.gameObject.transform.Translate (0, yPos, 0);
		GameObject ladderRow = this.gameObject.transform.GetChild (1).CompareTag ("LadderRow") ? this.gameObject.transform.GetChild (1).gameObject : this.gameObject.transform.GetChild (2).gameObject;
		boardScript.RandomizeCliffs (this.gameObject, ladderRow);
	}
}
