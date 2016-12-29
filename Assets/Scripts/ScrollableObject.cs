using UnityEngine;
using System.Collections;

public class ScrollableObject : MonoBehaviour
{
	[HideInInspector]public Rigidbody2D rb2d;

	private float scrollSpeed = .25f;
	private Vector2 targetPosition;
	private float targetPosY;

	// Use this for initialization
	protected void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		targetPosY = -12f;
		targetPosition = new Vector3 (rb2d.position.x, targetPosY);
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		Vector2 newPosition = Vector2.MoveTowards (rb2d.position, targetPosition, scrollSpeed * Time.deltaTime);
		rb2d.MovePosition (newPosition);
	}

	protected virtual void ResetPos (float yPos)
	{
		rb2d.MovePosition (new Vector2 (rb2d.position.x, yPos));
	}

	public void SetTargetPosX (float xPos)
	{
		targetPosition.x = xPos;
	}

	public void SetTargetPosY (float yPos)
	{
		targetPosY = yPos;
	}

	public float GetTargetPosY ()
	{
		return targetPosition.y;
	}

	public float GetTargetPosX ()
	{
		return targetPosition.x;
	}
}
