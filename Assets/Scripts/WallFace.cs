using UnityEngine;
using System.Collections;

public class WallFace : MonoBehaviour
{
	public float yOffset = 16f;
	private Camera cam;

	void Awake ()
	{
		cam = Camera.main;
	}
	// Update is called once per frame
	void Update ()
	{
		Vector3 viewPos = cam.WorldToViewportPoint (this.gameObject.transform.position + new Vector3 (0, 8, 0));
		if (viewPos.y < 0) {
			ResetPos (yOffset);
		}
	}

	void ResetPos (float yPos)
	{
		this.gameObject.transform.Translate (0, yPos, 0);
	}
}
