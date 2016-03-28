using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{
	public float moveTime = 0.1f;
	[HideInInspector]public bool isFalling;
	public LayerMask blockingLayer;
	public LayerMask ladderAndBlockingLayer;
	public LayerMask ladderLayer;
	public Animator anim;

	private float inverseMoveTime;
	private Rigidbody2D rb2d;
	private BoxCollider2D boxCollider;
	private Vector2 rayOffset;

	// Use this for initialization
	protected virtual void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;	// Storing the reciprocal of moveTime to make future computations faster.
		boxCollider = GetComponent <BoxCollider2D> ();
		anim = GetComponent<Animator> ();
		rayOffset = new Vector2 (0.5f, 0.5f);
		isFalling = false;
	}

	protected bool Move (int xDir, int yDir)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		if (yDir > 0) {
			RaycastHit2D[] upHits = new RaycastHit2D[1];
			boxCollider.enabled = false;
			Physics2D.LinecastNonAlloc (start + rayOffset, end + rayOffset, upHits, ladderLayer);
			boxCollider.enabled = true;

			if (upHits [0].transform != null) {
				anim.SetTrigger ("heroClimb");
				StartCoroutine (SmoothMovement (end));
				return true;
			}
		} else if (yDir < 0) {
			RaycastHit2D[] downHits = new RaycastHit2D[1];
			Physics2D.LinecastNonAlloc (end + rayOffset, end + rayOffset, downHits, ladderLayer);

			if (downHits [0].transform != null) {
				anim.SetTrigger ("heroClimb");
				StartCoroutine (SmoothMovement (end));
				return true;
			}
		} else if (Mathf.Abs (xDir) > 1) {
			RaycastHit2D[] hits = new RaycastHit2D[1];
			Physics2D.LinecastNonAlloc (start + rayOffset, start + rayOffset, hits, ladderLayer);
			RaycastHit2D[] hits2 = new RaycastHit2D[1];
			boxCollider.enabled = false;
			Physics2D.LinecastNonAlloc ((start + rayOffset) + Vector2.down, (start + rayOffset) + Vector2.down, hits2, blockingLayer);
			boxCollider.enabled = true;
			if (hits2 [0].transform != null || hits [0].transform == null) {
				if (xDir > 0) {
					anim.SetTrigger ("facingRight");
				} else if (xDir < 0) {
					anim.SetTrigger ("facingLeft");
				}
				StartCoroutine (SmoothMovement (end));
				return true;
			}
		} else {
			RaycastHit2D[] hits = new RaycastHit2D[1];
			RaycastHit2D[] hits2 = new RaycastHit2D[1];
			boxCollider.enabled = false;
			Physics2D.LinecastNonAlloc (end + rayOffset, end + rayOffset, hits, blockingLayer);
			Physics2D.LinecastNonAlloc (end + rayOffset, end + rayOffset, hits2, ladderLayer);
			boxCollider.enabled = true;

			if (hits [0].transform == null || hits2 [0].transform != null) {
				if (xDir > 0) {
					anim.SetTrigger ("facingRight");
				} else if (xDir < 0) {
					anim.SetTrigger ("facingLeft");
				}
				StartCoroutine (SmoothMovement (end));
				return true;
			}
		}

		return false;
	}

	// This method determines the new position of an object and moves the object to that new position.
	protected IEnumerator SmoothMovement (Vector3 end)
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;	// stores the remaining distance(difference between current position and end position) as squared magnitude to make future computations faster.

		// This loop brings the object closer and closer to the end position while the remaining distance is greater than a constant (almost zero) number
		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, end, inverseMoveTime * Time.deltaTime);
			rb2d.MovePosition (newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			//yield return new WaitForSeconds (0.1f);
			yield return null;	// Waits a few frames before re-evaluating the loop.
		}
			
		Vector2 rayStart = transform.position;
		Vector2 rayEnd = rayStart + Vector2.down;

		RaycastHit2D[] hits = new RaycastHit2D[2];
		boxCollider.enabled = false;
		Physics2D.LinecastNonAlloc (rayStart + rayOffset, rayEnd + rayOffset, hits, ladderAndBlockingLayer);
		boxCollider.enabled = true;

		if (hits [0].transform == null) {
			rb2d.isKinematic = false;
			isFalling = true;
		}
	}

	protected virtual void AttemptMove (int xDir, int yDir)
	{
		bool canMove = Move (xDir, yDir);
	}
}
