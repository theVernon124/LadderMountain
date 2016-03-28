﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public float turnDelay;
	public static GameManager instance = null;
	public BoardManager boardScript;
	[HideInInspector] public bool playersTurn;

	private bool delayOccuring;

	// Use this for initialization
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		turnDelay = .3f;
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
		playersTurn = true;
	}

	void InitGame ()
	{
		boardScript.SetupScene ();
	}

	public void GameOver ()
	{
		enabled = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (playersTurn || delayOccuring) {
			return;
		}

		StartCoroutine (PlayerMoveDelay ());
	}

	IEnumerator PlayerMoveDelay ()
	{
		delayOccuring = true;

		yield return new WaitForSeconds (turnDelay);

		playersTurn = true;
		delayOccuring = false;
	}
}
