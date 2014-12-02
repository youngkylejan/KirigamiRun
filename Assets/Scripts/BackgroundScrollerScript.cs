using UnityEngine;
using System.Collections;

public class BackgroundScrollerScript : MonoBehaviour {
	
	public float offset = 20.48f;		//amount to move the scenery forward
	
	void OnTriggerEnter2D(Collider2D other) {
		//if this object's trigger collider hits another object tagged "Background"...
		if (other.tag == "Background") {
			//...get the other object's position...
			Vector3 pos = other.transform.position;
			//...add the amount to move it on the x-axis...
			pos.x += offset * 2;
			//...apply that to the other object's position.
			other.transform.position = pos;	
		}
	}

	void Update() {
		GameObject hero = GameObject.Find("Hero");
		//follow the target on the x-axis only
		transform.position = new Vector3 (hero.transform.position.x - offset,
		                                  transform.position.y, 
		                                  transform.position.z);
	}
}
