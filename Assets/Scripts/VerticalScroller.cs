using UnityEngine;
using System.Collections;

public class VerticalScroller : MonoBehaviour
{
	protected float scrollSpeed;
	protected Camera cam;

	protected virtual void Awake ()
	{
		cam = Camera.main;
	}
	// Update is called once per frame
	protected virtual void Update ()
	{
		this.gameObject.transform.Translate (Vector2.up * Time.deltaTime * scrollSpeed);
	}

	public void setScrollSpeed (float speed)
	{
		scrollSpeed = speed;
	}
}
