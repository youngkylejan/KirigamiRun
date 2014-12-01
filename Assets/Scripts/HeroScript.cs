using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {

	Animator anim;					//reference to the animator component
	bool flap = false;
	bool grounded = false, jumping = false;

	Transform groundCheck;

	void Awake() {
		rigidbody2D.velocity = new Vector2(10, 0);
		groundCheck = transform.Find ("GroundCheck");
	}

	// Use this for initialization
	void Start () {		
		anim = GetComponent<Animator> ();
		rigidbody2D.velocity = new Vector2(10, 0);
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.Linecast(transform.position, 
		                              groundCheck.position, 
		                              1 << LayerMask.NameToLayer("Ground"));  

		if (jumping && grounded) {
			jumping = false;
			anim.SetTrigger("Grounded");
		}

		if (Input.GetKeyDown(KeyCode.UpArrow) && grounded && !flap) {
			//			transform.position = new Vector3(transform.position.x+1,transform.position.y,0);
			flap = true;
		}
	}

	void FixedUpdate() {
		if (flap) {
			//			transform.position = new Vector3(transform.position.x+1,transform.position.y,0);
			flap = false;
			anim.SetTrigger("Jump");
			rigidbody2D.AddForce(new Vector2(0, 10000));

			jumping = true;
		}
		rigidbody2D.velocity = new Vector2(20, rigidbody2D.velocity.y);
	}
}
