using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour
{
	public void LoadScene (int level)
	{
		StartCoroutine (ChangeLevel (level));
	}

	IEnumerator ChangeLevel (int level)
	{
		float fadeTime = GameObject.Find ("Main Camera").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime * 2f);
		Application.LoadLevel (level);
	}
}