using UnityEngine;
using System.Collections;

namespace Kirigami {
namespace Hero {

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

	bool toJump = false, 
		 toStopJumping = false,
		 toGround = false,
		 toFalldown = false,
	     toRecover = false;
	bool isJumping = false,
		 isFalldown = false,
	     isStarted = false;
	int jumpTimes = 0;

//	Kirigami.GameControllerScript gameControllerScript;

	// Public functions
	public void FallDown() {
		toFalldown = true;
	}
	
	public void Ground() {
		if (!isJumping)
			return;
		toGround = true;
		jumpTimes = 0;
	}

	public void Recover() {
		if (!isFalldown) return;
		toRecover = true;
	}

	public void StartRunning() {
		if (!isStarted) {
			isStarted = true;
			bodyScript.TriggerStart();
			StartCoroutine ("IncreasingSpeedRandomly");
		}
	}

	public void Die() {
		isStarted = false;
		if (!isFalldown) {
			toFalldown = true;
		}
		rigidbody2D.velocity = new Vector2(0, 0);

		Kirigami.GameControllerScript.current.HeroDied();
		StopCoroutine("IncreasingSpeedRandomly");
	}

	public Vector2 CurrentVelocity() {
		return rigidbody2D.velocity;
	}

	// Private functions

	// Use this for initialization
	void Start () {
		bodyScript = gameObject.GetComponentInChildren<HeroBodyScript> ();
		currentSpeed = baseSpeed;
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
		if (!isFalldown) {
			if (HasKeyDownEvent()) {
				if (jumpTimes < MAX_JUMP_TIMES) {
					jumpTimes++;
					toJump = true;
				}
			} else if (HasKeyReleaseEvent() && isJumping && !toGround) {
				toStopJumping = true;
			} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
				StartRunning();
			}
		}
	}

	void FixedUpdate() {
		if (toJump) {
			toJump = false;
			isJumping = true;
//			if (jumpTimes != 1) {
//				bodyScript.TriggerGrounded();
//			}
			bodyScript.TriggerJump();
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		} else if (toStopJumping) {
			toStopJumping = false;
			if (rigidbody2D.velocity.y > 0) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
			}
		} else if (toGround) {
 			toGround = false;
			isJumping = false;
			bodyScript.TriggerGrounded();

		} else if (toFalldown) {
			toFalldown = false;
			isFalldown = true;

			bodyScript.TriggerFallDown();
			StopCoroutine("IncreasingSpeedRandomly");
		} else if (toRecover) {
			toRecover = false;
			isFalldown = false;
			bodyScript.TriggerRecover();
			StartCoroutine("IncreasingSpeedRandomly");
			currentSpeed = baseSpeed;
		}

		if (isStarted && !isFalldown) {
			rigidbody2D.velocity = new Vector2(currentSpeed, rigidbody2D.velocity.y);
		}
	}

	IEnumerator IncreasingSpeedRandomly() {
		const float MAX_SPEED = 25.0f;
		while (!isFalldown) {
			int waitSec = Random.Range(1, 5);
			yield return new WaitForSeconds(waitSec);

			// Vn = Vn-1 + a * delta_t
			if (currentSpeed >= MAX_SPEED) continue;

			float a = 1.0f / Mathf.Pow((currentSpeed / MAX_SPEED) * 2.5f + 1.5f, 2.0f);
			currentSpeed += a * waitSec;
			print ("BEGIN ACELERATING " + currentSpeed);
		}
	}
}

}
}
