using UnityEngine;
using System.Collections;

public class BackgroundScroller : VerticalScroller
{

	// Use this for initialization
	void Awake ()
	{
		base.Awake ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update ();

		Vector3 viewPos = cam.WorldToViewportPoint (this.gameObject.transform.position + new Vector3 (0, 3, 0));
		if (viewPos.y < 0) {
			GameManager.instance.bgIsVisible = false;
		}
	}
}
