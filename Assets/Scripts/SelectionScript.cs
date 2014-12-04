using UnityEngine;
using System.Collections;

public class SelectionScript : MonoBehaviour {

	public GameObject selection,title;
	public GameObject start;

	public GameObject heroListEnabled, heroListDisabled;
	
	void OnMouseDown () {
		title.SetActive (false);
		selection.SetActive (false);
		start.SetActive (true);
		heroListEnabled.SetActive (true);
		heroListDisabled.SetActive (true);
	}
	
	void OnTouchDown() {
		title.SetActive (false);
		selection.SetActive (false);
		start.SetActive (true);
		heroListEnabled.SetActive (true);
		heroListDisabled.SetActive (true);
	}
}
