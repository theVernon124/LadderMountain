using UnityEngine;
using System.Collections;

public class CameraScroller : VerticalScroller
{
	private float colorFadeTime;
	private float t;
	private Color initialCamColor;
	private Color spaceColor;

	protected override void Awake ()
	{
		base.Awake ();

		initialCamColor = cam.backgroundColor;
		spaceColor = new Color (.32f, .19f, .52f);
		colorFadeTime = 300f;
		t = 0;
	}
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

		if (GameManager.instance.sunIsVisible && gameObject.transform.childCount > 0) {
			if (!gameObject.transform.GetChild (0).gameObject.activeSelf) {
				gameObject.transform.GetChild (0).gameObject.SetActive (true);
			}
		}

		if (GameManager.instance.starfieldIsVisible && gameObject.transform.childCount > 1) {
			if (!gameObject.transform.GetChild (1).gameObject.activeSelf) {
				gameObject.transform.GetChild (1).gameObject.SetActive (true);
			}
			if (!gameObject.transform.GetChild (2).gameObject.activeSelf) {
				gameObject.transform.GetChild (2).gameObject.SetActive (true);
			}
		}

		if (t <= 1 && scrollSpeed > 0) {
			t += Time.deltaTime / colorFadeTime;
			cam.backgroundColor = Color.Lerp (initialCamColor, spaceColor, t);
		}
	}

	public void setScrollSpeed (float speed)
	{
		scrollSpeed = speed;
	}
}
