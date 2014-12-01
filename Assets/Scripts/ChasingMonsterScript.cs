using UnityEngine;
using System.Collections;

public class ChasingMonsterScript : MonoBehaviour {

	public Animator monAnim; 
	public bool metHero = false;

	// Use this for initialization
	void Start () {
		monAnim = GetComponent<Animator> ();
//		rigidbody2D.velocity = new Vector2 (12, 0);
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Hero") {
			print("Fuck you! ");
			metHero = true;
		} else if (other.gameObject.tag == "Monster") {
			other.collider.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (metHero == true) {
			rigidbody2D.velocity = new Vector2 (0, 0);
			monAnim.SetTrigger("Stop");
		} else {
			rigidbody2D.velocity = new Vector2 (11.5f, 0);
		}
	}

	void OnBecameInvisible () {
		Destroy(this.gameObject);  
	}

}
