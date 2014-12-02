using UnityEngine;
using System.Collections;

public class HeroBodyScript : MonoBehaviour {

	Animator anim;					//reference to the animator component

	HeroScript heroScript;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		heroScript = gameObject.GetComponentInParent<HeroScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TriggerJump() {
		anim.SetTrigger("Jump");
	}

	public void TriggerGrounded() {
		anim.SetTrigger ("Grounded");
	}

	public void TriggerFallDown() {
		anim.SetTrigger ("Fall");
	}

	public void TriggerRecover() {
		anim.SetTrigger ("Recover");
	}

	void OnCollisionEnter2D(Collision2D other) {
		print ("BODY COLLIDE " + other.gameObject.name + " " + Time.time);
		heroScript.FallDown ();
	}
}
