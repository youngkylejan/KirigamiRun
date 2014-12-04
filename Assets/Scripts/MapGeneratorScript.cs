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

		const int difficultInterval = 10;
		const int easyFixedTime = 4;
		const int hardFixedTime = 2;
		const int easyEdge = 8;

		int easyTimes;
		int hardTimes;

		public GameObject groundPrefab;
		public Camera mainCamera;
		public GameObject hero;
		public List<GameObject> groundCollection = new List<GameObject>();

		// Use this for initialization
		void Start() {
			Random.seed = System.Guid.NewGuid().GetHashCode();
			easyTimes = 0;
			hardTimes = 0;

			InitialFirstMap();
			StartCoroutine("Generator");
			StartCoroutine("Remover");
		}
		
		// Update is called once per frame
		void Update() {
			hero = GameObject.Find("Hero");
			//follow the target on the x-axis only
			transform.position = new Vector3 (hero.transform.position.x + offset,
			                                  transform.position.y, 
			                                  transform.position.z);
		}

		void InitialFirstMap() {
			GameObject ground = (GameObject)Instantiate (groundPrefab);

			Vector3 pos = transform.position;
			Random.seed = System.Guid.NewGuid().GetHashCode();
			pos.x = transform.position.x + mapLength + Random.Range (3f, 6f);
			pos.y = pos.y + Random.Range(minDeltaHeight, maxDeltaHeight);
			pos.z = ground.transform.position.z;

			ground.transform.position = pos;
			ground.tag = "Scenery";

			groundCollection.Add (ground);
		}

		int GenerateDifficultyDegree() {
			int degree = Random.Range (1, difficultInterval);

			if (easyTimes >= easyFixedTime) {
				while (degree <= easyEdge) {
					degree = Random.Range(1, difficultInterval);
				}
			}

			if (hardTimes >= hardFixedTime) {
				while (degree > easyEdge) {
					degree = Random.Range(1, difficultInterval);
				}
			}

			if (degree <= easyEdge) {
				easyTimes++;
				hardTimes = 0;
			} else {
				hardTimes++;
				easyTimes = 0;
			}

			return degree;
		}

		float GenerateDeltaWidth(ref bool difficult) {

			Vector2 velocity = hero.rigidbody2D.velocity;

			float maxDistance = velocity.x * 1.9f;
			float minDistance = groundWidth + 3f;

			float unitDistance = maxDistance / difficultInterval;
			int difficultyDegree = GenerateDifficultyDegree ();

			if (difficultyDegree > easyEdge) {
				difficult = true;
			}

			float dis = Random.Range ((float)unitDistance * difficultyDegree, (float)unitDistance * (difficultyDegree + 1));

			if (dis < (minDistance)) {
				dis = minDistance;
			}

			return dis;
		}

		float GetLastGroundHeight() {
			GameObject lastGround = groundCollection[groundCollection.Count - 1];
			return lastGround.transform.position.y;
		}

		float GenerateDeltaHeight(float originHeight, ref bool difficult) {

			if (!difficult) {
				return Random.Range(minDeltaHeight, maxDeltaHeight);
			} else {
				float lastGroundHeight = GetLastGroundHeight();
				float nextGroundHeight = 0f;
				while (nextGroundHeight - lastGroundHeight > 3f) {
					nextGroundHeight = originHeight + Random.Range(minDeltaHeight, maxDeltaHeight);
				}
				return nextGroundHeight;
			}
		}

		void GenerateGrounds() {

			int groundNumber = Random.Range (2, 5);
			bool firstFlag = true;
			bool difficult = false;

			while (groundNumber > 0) {

				GameObject newGround = (GameObject)Instantiate (groundPrefab);

				Vector3 pos = new Vector3();

				GameObject lastGround = groundCollection[groundCollection.Count - 1];

				if (firstFlag) {
					pos.x = lastGround.transform.position.x + GenerateDeltaWidth(ref difficult);
					pos.y = GenerateDeltaHeight(pos.y, ref difficult);
					firstFlag = false;
				} else {
					pos.x = lastGround.transform.position.x + groundWidth;
					pos.y = lastGround.transform.position.y;
				}

				pos.z = newGround.transform.position.z;
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

				if (lastGround.transform.position.x - generatorPos.x > mapLength) {
					yield return new WaitForSeconds(generateRate);
				} else {
					GenerateGrounds();
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
						if (frontGround.transform.position.x < mainCamera.transform.position.x - mapLength) {
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
