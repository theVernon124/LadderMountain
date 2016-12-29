using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Allows us to use UI.

public class Player : MovingObject
{
	[HideInInspector] public Rigidbody2D playerRb2d;
	public float restartLevelDelay = 1f;
	public Camera cam;
	public Text scoreText;
	public Text GameOverText;
	public GameObject bgRight;
	public GameObject bgLeft;
	public Sun sun;
	public Starfield sField1, sField2;

	private int highestY;
	private int score;
	private bool isGameOver;
	private bool gameStarted;
	private CameraScroller camScroller;
	private BackgroundScroller bgRightScroller;
	private BackgroundScroller bgLeftScroller;

	private Vector2 touchOrigin = -Vector2.one;
	//Used to store location of screen touch origin for mobile controls.

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		camScroller = cam.GetComponent<CameraScroller> ();
		bgRightScroller = bgRight.GetComponent<BackgroundScroller> ();
		bgLeftScroller = bgLeft.GetComponent<BackgroundScroller> ();
		playerRb2d = GetComponent<Rigidbody2D> ();
		highestY = (int)transform.position.y;
		score = 0;
		scoreText.text = "Score: " + score;
		isGameOver = false;
		gameStarted = true;
	}

	void Update ()
	{
		if (transform.position.y == highestY + 3) {
			score++;
			highestY = (int)transform.position.y;
			scoreText.text = "Score: " + score;
		}

		if (score >= 50 && !GameManager.instance.canSpawnAirplanes) {
			GameManager.instance.canSpawnAirplanes = true;
		}
		if (score >= 75 && !GameManager.instance.sunIsVisible) {
			GameManager.instance.sunIsVisible = true;
		}
		if (score >= 100) {
			GameManager.instance.disableCloudsAndPlanes = true;
			GameManager.instance.starfieldIsVisible = true;
		}

		Vector3 viewPos = cam.WorldToViewportPoint (this.gameObject.transform.position);
		Vector3 viewPosFull = cam.WorldToViewportPoint (this.gameObject.transform.position + Vector3.up);
		if (gameStarted) {
			if (viewPos.y >= 0.3f) {
				gameStarted = false;
			} else if (viewPosFull.y < 0f) {
				camScroller.setScrollSpeed (0f);
				bgLeftScroller.setScrollSpeed (0f);
				bgRightScroller.setScrollSpeed (0f);
				GameOverText.gameObject.SetActive (true);
				isGameOver = true;
			}
		} else if (viewPos.y <= 0.5f && viewPos.y >= 0) {
			camScroller.setScrollSpeed (0.75f);
			sun.setScrollSpeed (0.65f);
			sField1.setScrollSpeed (0.65f);
			sField2.setScrollSpeed (0.65f);
			bgLeftScroller.setScrollSpeed (0.65f);
			bgRightScroller.setScrollSpeed (0.65f);
		} else if (viewPos.y > 0.5f) {
			camScroller.setScrollSpeed (1.75f);
			sun.setScrollSpeed (1.65f);
			sField1.setScrollSpeed (1.65f);
			sField2.setScrollSpeed (1.65f);
			bgLeftScroller.setScrollSpeed (1.65f);
			bgRightScroller.setScrollSpeed (1.65f);
		} else if (viewPosFull.y < 0f) {
			camScroller.setScrollSpeed (0f);
			sun.setScrollSpeed (0f);
			sField1.setScrollSpeed (0f);
			sField2.setScrollSpeed (0f);
			bgLeftScroller.setScrollSpeed (0f);
			bgRightScroller.setScrollSpeed (0f);
			GameOverText.gameObject.SetActive (true);
			isGameOver = true;
		}


		#if UNITY_STANDALONE || UNITY_WEBPLAYER

		if (Input.GetKeyDown (KeyCode.R) && isGameOver) {
			isGameOver = false;
			SceneManager.LoadScene ("Scene01", LoadSceneMode.Single);
		}

		if (!GameManager.instance.playersTurn || isFalling) {
			return;
		}

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (Input.GetKeyDown (KeyCode.X)) {
			horizontal = 2;
		} else if (Input.GetKeyDown (KeyCode.Z)) {
			horizontal = -2;
		}

		if (horizontal != 0) {
			vertical = 0;
		}

		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		if (Input.touchCount > 0 && isGameOver) {
		isGameOver = false;
		SceneManager.LoadScene ("Scene01", LoadSceneMode.Single);
		} 

		if (!GameManager.instance.playersTurn || isFalling) {
		return;
		}

		int horizontal = 0;
		int vertical = 0;

		if (Input.touchCount > 0) {
			//Store the first touch detected.
			Touch myTouch = Input.GetTouch (0);

			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began) {
				//If so, set touchOrigin to the position of that touch
				touchOrigin = myTouch.position;
			}

			//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
			else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
				//Set touchEnd to equal the position of this touch
				Vector2 touchEnd = myTouch.position;

				//Calculate the difference between the beginning and end of the touch on the x axis.
				float x = touchEnd.x - touchOrigin.x;

				//Calculate the difference between the beginning and end of the touch on the y axis.
				float y = touchEnd.y - touchOrigin.y;

				//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
				touchOrigin.x = -1;

				if (Mathf.Abs (x) < 3f && Mathf.Abs (y) < 3f) {
					if (touchEnd.x < Screen.width / 2) {
						horizontal = -1;
					} else if (touchEnd.x > Screen.width / 2) {
						horizontal = 1;
					}
				}
				//Check if the difference along the x axis is greater than the difference along the y axis.
				else if (Mathf.Abs (x) > Mathf.Abs (y)) {
					//If x is greater than zero, set horizontal to 2, otherwise set it to -2
					horizontal = x > 0 ? 2 : -2;
				} else {
					//If y is greater than zero, set horizontal to 1, otherwise set it to -1
					vertical = y > 0 ? 1 : -1;
				}
			}
		}

		#endif

		if (horizontal != 0 || vertical != 0) {
			AttemptMove (horizontal, vertical);
		}
	}

	protected override void AttemptMove (int xDir, int yDir)
	{
		GameManager.instance.playersTurn = false;
		base.AttemptMove (xDir, yDir);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if ((other.gameObject.CompareTag ("Cliff") && isFalling) || (other.gameObject.CompareTag ("Ladder") && isFalling)) {
			playerRb2d.isKinematic = true;
			float yCorrection = Mathf.Round (transform.position.y) - transform.position.y;
			transform.Translate (new Vector2 (0f, yCorrection));
			isFalling = false;
		}
	}
}
