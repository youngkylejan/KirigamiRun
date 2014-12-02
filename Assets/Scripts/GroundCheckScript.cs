using UnityEngine;
using System.Collections;

public class GroundCheckScript : MonoBehaviour {

	HeroScript heroScript;

	// Use this for initialization
	void Start () {
		heroScript = gameObject.GetComponentInParent<HeroScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Scenery") {
			heroScript.Ground();
		}
	}
}
