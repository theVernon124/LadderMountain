using UnityEngine;
using System.Collections;

public class Sun : VerticalScroller
{

	// Use this for initialization
	void Awake ()
	{
		base.Awake ();
	}

	void Start ()
	{
		gameObject.transform.parent = null;
		gameObject.transform.Translate (new Vector3 (0, 0, 10f));
		scrollSpeed = .7f;
	}

	// Update is called once per frame
	void Update ()
	{
		base.Update ();

		Vector3 viewPos = cam.WorldToViewportPoint (this.gameObject.transform.position + new Vector3 (0, 2, 0));
		if (viewPos.y < 0) {
			gameObject.SetActive (false);
		}
	}
}
