using UnityEngine;
using System.Collections;

public class HorizontalScroller: MonoBehaviour
{
	public float xOffset;
	public Vector2 scrollDirection;
	private Camera cam;
	
	// Use this for initialization
	void Awake ()
	{
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.gameObject.transform.Translate (scrollDirection * Time.deltaTime * .25f);

		Vector3 viewPos = cam.WorldToViewportPoint (this.gameObject.transform.position + new Vector3 (2, 0, 0));
		if (viewPos.x < 0) {
			this.gameObject.transform.Translate (xOffset, 0, 0);
		}
	}
}
