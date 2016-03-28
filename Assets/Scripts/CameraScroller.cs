using UnityEngine;
using System.Collections;

public class CameraScroller : MonoBehaviour
{
	private float scrollSpeed;
	
	// Update is called once per frame
	void Update ()
	{
		this.gameObject.transform.Translate (Vector2.up * Time.deltaTime * scrollSpeed);
	}

	public void setScrollSpeed (float speed)
	{
		scrollSpeed = speed;
	}
}
