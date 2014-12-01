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

	Animator anim;					//reference to the animator component
	bool flap = false;
	bool grounded = false, jumping = false, felldown = false, dead = false;
	bool to_falldown = false, to_recover = false, to_die = false;
	bool acelerating = false;
	float currentSpeed = 10.0f;

	Transform groundCheck;

	void Awake() {
		rigidbody2D.velocity = new Vector2(10, 0);
		groundCheck = transform.Find ("GroundCheck");
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		StartCoroutine ("IncreasingSpeedRandomly");
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.Linecast(transform.position, 
		                              groundCheck.position, 
		                              1 << LayerMask.NameToLayer("Ground")); 
		if (!felldown) {
			if (jumping && grounded) {
				jumping = false;
				anim.SetTrigger("Grounded");
			}

			if (Input.GetKeyDown(KeyCode.UpArrow) && grounded && !flap) {
				//			transform.position = new Vector3(transform.position.x+1,transform.position.y,0);
				flap = true;
			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				to_falldown = true;
			}
		}
	}

	void FixedUpdate() {
		if (to_falldown) {
			anim.SetTrigger("Fall");
			felldown = true;
			to_falldown = false;

			rigidbody2D.gravityScale = falldownGravityScale;
			rigidbody2D.velocity = new Vector2(baseSpeed, 0);
			rigidbody2D.AddForce(falldownForce);
			StopCoroutine("IncreasingSpeedRandomly");
			StartCoroutine("ReadyToRecover");

			return;
		} else if (to_recover) {
			anim.SetTrigger("Recover");
			felldown = false;
			to_recover = false;
			rigidbody2D.gravityScale = normalGravityScale;
			currentSpeed = baseSpeed;

//			StartCoroutine("AcelerateToCurrentSpeed");
			StartCoroutine("IncreasingSpeedRandomly");
			return;
		} else if (to_die) {
			anim.SetTrigger("Fall");
			felldown = true;
			dead = true;

			rigidbody2D.gravityScale = falldownGravityScale;
			rigidbody2D.velocity = new Vector2(baseSpeed, 0);
			rigidbody2D.AddForce(falldownForce);
			StopCoroutine("IncreasingSpeedRandomly");
			return;
		}

		if (!felldown) {
		if (flap) {
				//			transform.position = new Vector3(transform.position.x+1,transform.position.y,0);
				flap = false;
				anim.SetTrigger("Jump");
				rigidbody2D.AddForce(new Vector2(0, jumpForce));
				jumping = true;
			}

			if (!acelerating) {
				rigidbody2D.velocity = new Vector2(currentSpeed, rigidbody2D.velocity.y);
			}
		}
	}

	void FallDown() {
		anim.SetTrigger ("Fall");
	}

	void Recover() {
		anim.SetTrigger ("Recover");
	}

	IEnumerator ReadyToRecover() {
		if (to_recover)
			yield return null;
		yield return new WaitForSeconds(recoverSeconds);
		to_recover = true;
	}

	IEnumerator AcelerateToCurrentSpeed() {
		acelerating = true;
		while (rigidbody2D.velocity.x < currentSpeed) {
			rigidbody2D.AddForce(new Vector2(acelerateForce, 0));
			print ("ACELE CURR " + rigidbody2D.velocity);
			yield return null;
		}
		acelerating = false;
	}

	IEnumerator IncreasingSpeedRandomly() {
		while (!dead) {
			yield return new WaitForSeconds(Random.Range(1, 10));

//			acelerating = true;
			currentSpeed += Mathf.Log10(currentSpeed / 2.0f);
			print ("BEGIN ACELERATING " + currentSpeed);
		}
	}
}
