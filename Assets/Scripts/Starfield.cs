using UnityEngine;
using System.Collections;

public class Starfield : VerticalScroller
{

	// Use this for initialization
	void Awake ()
	{
		base.Awake ();
	}

	void Start ()
	{
		gameObject.transform.parent = null;
		gameObject.transform.Translate (new Vector3 (0f, 0f, 10f));
		scrollSpeed = .7f;
	}

	// Update is called once per frame
	void Update ()
	{
		base.Update ();

		Vector3 viewPos = cam.WorldToViewportPoint (this.gameObject.transform.position + new Vector3 (0, 8, 0));
		if (viewPos.y < 0) {
			gameObject.transform.Translate (new Vector3 (0f, 16f, 0f));
		}
	}
}
