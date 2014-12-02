using UnityEngine;
using System.Collections;

namespace Kirigami {
namespace Hero {

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
		} else if (other.gameObject.tag == "DeadBlock") {
			heroScript.Die();
		}
	}
}

}
}