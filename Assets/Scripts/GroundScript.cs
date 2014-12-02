using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Hero") {
			other.rigidbody.velocity = new Vector3(other.rigidbody.velocity.x, 0, 0);
		}
	}
}
