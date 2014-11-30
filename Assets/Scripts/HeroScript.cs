using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {

	Animator anim;					//reference to the animator component
	bool flap = false;

	private GameObject groundCheck;
	private float distToGround;

	void Awake() {
		groundCheck = GameObject.Find ("Map");
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigidbody2D.velocity = new Vector2(10, 0);
		distToGround = collider2D.bounds.extents.y;
	}

	void OnCollisionEnter2D(Collision2D other) {
		print (other.gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		bool grounded = transform.position.y <= -2;
		if (Input.GetKeyDown(KeyCode.UpArrow) && grounded && !flap) 
		{
			//			transform.position = new Vector3(transform.position.x+1,transform.position.y,0);
			flap = true;
		}

		rigidbody2D.velocity = new Vector2(100, rigidbody2D.velocity.y);
	}

	void FixedUpdate()
	{
		if (flap) 
		{
			//			transform.position = new Vector3(transform.position.x+1,transform.position.y,0);
			flap = false;
			anim.SetTrigger("Jump");
			rigidbody2D.AddForce(new Vector2(0, 10000));
		}
	}
}
