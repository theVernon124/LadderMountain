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
		}
	}

	void ResetPos (float yPos)
	{
		this.gameObject.transform.Translate (0, yPos, 0);
		boardScript.RandomizeCliffs (this.gameObject, this.gameObject.transform.GetChild (0).gameObject);
	}
}
