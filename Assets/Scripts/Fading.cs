using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{
	public Texture2D fadeOutTexture;
	public Texture2D fadeInTexture;
	public float fadeSpeed = 1f;
	public int drawDepth = -1000;
	public bool fadeInFlag = true;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void OnGUI ()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;

		if (fadeInFlag) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeInTexture);
		} else {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
		}
	}

	public float BeginFade (int direction)
	{
		fadeInFlag = false;
		fadeDir = direction;
		return fadeSpeed;
	}

	void OnLevelWasLoaded ()
	{
		Debug.Log ("onlevelwasloaded");
		BeginFade (-1);
	}
}
