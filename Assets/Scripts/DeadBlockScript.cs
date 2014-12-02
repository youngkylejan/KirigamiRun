using UnityEngine;
using System.Collections;

namespace Kirigami {
	public class DeadBlockScript : MonoBehaviour {
		
		// Update is called once per frame
		void Update () {
			GameObject hero = GameObject.Find("Hero");
			transform.position = new Vector3 (hero.transform.position.x,
			                                 transform.position.y,
			                                 transform.position.z);
		}
	}
}