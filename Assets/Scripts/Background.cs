using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
	private Camera cam;
	// Use this for initialization
	void Awake ()
	{
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 viewPos = cam.WorldToViewportPoint (this.gameObject.transform.position + new Vector3 (0, 3, 0));
		if (viewPos.y < 0) {
			GameManager.instance.bgIsVisible = false;
		}
	}
}
