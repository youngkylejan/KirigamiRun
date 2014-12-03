using UnityEngine;
using System.Collections;

namespace Kirigami
{
public class StartGameScript : MonoBehaviour {

	void OnMouseDown () {
		GameControllerScript.current.startGame ();
	}

	void OnTouchDown() {
		GameControllerScript.current.startGame();
	}
}
}
