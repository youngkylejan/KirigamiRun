using UnityEngine;
using System.Collections;

namespace Kirigami
{
public class StartGameScript : MonoBehaviour {

	void OnMouseDown () {
			print ("HERE!!");
		GameControllerScript.current.startGame ();
	}

	void OnTouchDown() {
			print ("HERE!! TOUCH");
		GameControllerScript.current.startGame();
	}
}
}
