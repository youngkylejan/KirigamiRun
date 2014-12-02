using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var renderer = gameObject.GetComponent<Renderer>();
		float width = renderer.bounds.size.x;
		float height = renderer.bounds.size.y;
		print (width);
		print (height);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
