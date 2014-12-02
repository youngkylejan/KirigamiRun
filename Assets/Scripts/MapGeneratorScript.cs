using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kirigami {

	public class MapGeneratorScript : MonoBehaviour {

		const float mapLength = 20.48f;
		const float offset = 22.24f;
		const float groundWidth = 2.18f;
		const float groundHeight = 8.9f;
		const float removeRate = 2f;
		const float generateRate = 0.5f;

		const float minDeltaHeight = -8f;
		const float maxDeltaHeight = -5f;

		public GameObject groundPrefab;
		public Camera mainCamera;
		public List<GameObject> groundCollection = new List<GameObject>();

		// Use this for initialization
		void Start() {
			InitialFirstMap();
			StartCoroutine("Generator");
			StartCoroutine("Remover");
		}
		
		// Update is called once per frame
		void Update() {
			GameObject hero = GameObject.Find("Hero");
			//follow the target on the x-axis only
			transform.position = new Vector3 (hero.transform.position.x + offset,
			                                  transform.position.y, 
			                                  transform.position.z);
		}

		void InitialFirstMap() {
			GameObject ground = (GameObject)Instantiate (groundPrefab);

			Vector3 pos = transform.position;
			Random.seed = System.Guid.NewGuid().GetHashCode();
			pos.x = transform.position.x + mapLength + Random.Range (2f, 5f);
			pos.y = pos.y + Random.Range(minDeltaHeight, maxDeltaHeight);
			pos.z = -2;

			ground.transform.position = pos;
			ground.tag = "Scenery";

			groundCollection.Add (ground);
		}

		void GeneratorGrounds() {

			Random.seed = System.Guid.NewGuid().GetHashCode();
			int groundNumber = Random.Range (1, 4);
			bool firstFlag = true;

			while (groundNumber > 0) {

				GameObject newGround = (GameObject)Instantiate (groundPrefab);
				
				Random.seed = System.Guid.NewGuid().GetHashCode();
				
				Vector3 pos = new Vector3();
				pos.z = -2;

				GameObject lastGround = groundCollection[groundCollection.Count - 1];

				if (firstFlag) {
					pos.x = lastGround.transform.position.x + Random.Range(5f, 10f);
					pos.y = pos.y + Random.Range(minDeltaHeight, maxDeltaHeight);
					firstFlag = false;
				} else {
					pos.x = lastGround.transform.position.x + groundWidth;
					pos.y = lastGround.transform.position.y;
				}
				
				newGround.transform.position = pos;
				newGround.tag = "Scenery";
				groundCollection.Add(newGround);

				groundNumber--;
			}
		}

		IEnumerator Generator() {

			while(true) {

				Vector3 generatorPos = transform.position;
				GameObject lastGround = groundCollection[groundCollection.Count - 1];

				if (lastGround.transform.position.x  - generatorPos.x > mapLength) {
					yield return new WaitForSeconds(generateRate);
				} else {
					GeneratorGrounds();
				}

				yield return new WaitForSeconds(generateRate);
			}
		}

		IEnumerator Remover() {

			while(true) {

				while(true) {
					if (groundCollection.Count <= 0) {
						break;
					}
					else {
						GameObject frontGround = groundCollection[0];
						if (frontGround.transform.position.x < mainCamera.transform.position.x - mapLength / 2) {
							Destroy(frontGround);
							groundCollection.RemoveAt(0);
						}
						else {
							break;
						}
					}
				}

				yield return new WaitForSeconds(removeRate);
			}
		}

		public void Stop() {
			StopCoroutine("Generator");
			StopCoroutine("Remover");
		}
	}
}
