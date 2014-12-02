using UnityEngine;
using System.Collections;

namespace Kirigami
{
public class ReStartGameScript : MonoBehaviour {

	void OnMouseDown () {
		Application.LoadLevel(Application.loadedLevel);
	}
}
}
