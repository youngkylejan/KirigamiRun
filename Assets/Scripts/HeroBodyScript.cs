using UnityEngine;
using System.Collections;

namespace Kirigami {
namespace Hero {

public class HeroBodyScript : MonoBehaviour {

	Animator anim;					//reference to the animator component

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
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

	public void TriggerStart() {
		anim.SetTrigger ("Start");
	}

	public void TriggerReady() {
		anim.SetTrigger ("Ready");
	}
}

}
}
