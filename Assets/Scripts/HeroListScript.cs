using UnityEngine;
using System.Collections;

namespace Kirigami{
public class HeroListScript : MonoBehaviour {

	void OnMouseDown () {
 		onSelect();
	}
	
	void OnTouchDown() {
		onSelect();
	}

	void onSelect() {
		GameControllerScript.current.heroSelection(int.Parse (gameObject.name));
	}
}
}
