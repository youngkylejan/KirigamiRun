using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {

	public float baseSpeed = 10.0f;
	public float jumpForce = 10000.0f;
	public Vector2 falldownForce;
	public float acelerateForce = 1000.0f;
	public float recoverSeconds = 120;
	public float normalGravityScale = 5;
	public float falldownGravityScale = 1.5f;

	float currentSpeed;
	const int MAX_JUMP_TIMES = 2;
	HeroBodyScript bodyScript;
	Transform groundCheck;

	bool toJump = false, 
		 toStopJumping = false,
		 toGround = false,
		 toFalldown = false;
	bool isGrounded = false,
		 isJumping = false,
		 isFalldown = false;
	int jumpTimes = 0;


	// Use this for initialization
	void Start () {
		bodyScript = gameObject.GetComponentInChildren<HeroBodyScript> ();
		currentSpeed = baseSpeed;
//		StartCoroutine ("IncreasingSpeedRandomly");
		groundCheck = transform.Find ("GroundCheckForHero");
	}

	bool HasKeyDownEvent() {
		return (Input.touchCount > 0
					&& Input.touches[0].phase == TouchPhase.Began)
			   || (Input.GetKeyDown(KeyCode.UpArrow));
	}

	bool HasKeyReleaseEvent() {
		return (Input.touchCount > 0
		        	&& Input.touches[0].phase == TouchPhase.Ended)
			|| (Input.GetKeyUp(KeyCode.UpArrow));
	}
	
	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.Linecast(transform.position, 
		                              groundCheck.position, 
		                              1 << LayerMask.NameToLayer("Ground")); 

		if (!isFalldown) {
			if (isGrounded && isJumping) {
				jumpTimes = 0;
				toGround = true;
			}

			if (HasKeyDownEvent()) {
				if (jumpTimes < MAX_JUMP_TIMES) {
					jumpTimes++;
					toJump = true;
				}
			} else if (HasKeyReleaseEvent()) {
				toStopJumping = true;
			}
		}
	}

	void FixedUpdate() {
		if (toJump) {
			toJump = false;
			isJumping = true;
			if (jumpTimes == 1) {
				bodyScript.TriggerJump();
			}
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		}

		if (toStopJumping) {
			toStopJumping = false;
			if (rigidbody2D.velocity.y > 0) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
			}
		}

		if (toGround) {
			toGround = false;
			isJumping = false;
			bodyScript.TriggerGrounded();
		}

		if (toFalldown) {
			toFalldown = false;
			isFalldown = true;

			bodyScript.TriggerFallDown();
			StopCoroutine("IncreasingSpeedRandomly");
		}

		if (!isFalldown) {
			rigidbody2D.velocity = new Vector2(currentSpeed, rigidbody2D.velocity.y);
		}
	}

	public void FallDown() {
		toFalldown = true;
	}

	IEnumerator IncreasingSpeedRandomly() {
		const float MAX_SPEED = 30.0f;
		while (!isFalldown) {
			int waitSec = Random.Range(1, 5);
			yield return new WaitForSeconds(waitSec);

			// Vn = Vn-1 + a * delta_t
			if (currentSpeed >= MAX_SPEED) continue;

			float a = 1.0f / Mathf.Pow((currentSpeed / MAX_SPEED) * 2 + 0.5f, 5.0f);
			currentSpeed += a * waitSec;
			print ("BEGIN ACELERATING " + currentSpeed);
		}
	}
}
