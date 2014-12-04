using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kirigami {

	public class BackgroundScrollerScript : MonoBehaviour {
		
		const float offset = 20.48f;
		const float maxCloudDeltaHeight = 6f;
		const float maxSkyMonsterDelataHeight = 3f;

		public int triggerNum;

		public GameObject bg_prefab;
		public List<GameObject> cloud_prefabs;
		public List<GameObject> skySprite_prefabs;
		public List<GameObject> bgList = new List<GameObject>();
		public List<GameObject> cloudList = new List<GameObject>();
		public List<GameObject> skyMonsterList = new List<GameObject>();

		void GenerateCloud(Vector3 pos) {

			int number = Random.Range (0, 4);

			while (number > 0) {

				GameObject cloud = (GameObject)Instantiate(cloud_prefabs[Random.Range(0, cloud_prefabs.Count)]);
				cloud.transform.position = new Vector3(Random.Range(pos.x, pos.x + offset),
				                                       Random.Range(cloud.transform.position.y, cloud.transform.position.y + maxCloudDeltaHeight),
				                                       cloud.transform.position.z);
				cloudList.Add(cloud);
				number--;
			}
		}

		void RemoveCloud(Vector3 pos) {

			while (cloudList.Count > 0) {

				GameObject cloud = cloudList[0];

				if (cloud.transform.position.x < pos.x - offset * 3) {
					Destroy(cloud);
					cloudList.RemoveAt(0);
				} else {
					break;
				}
			}
		}

		void GenerateSkyMonster(Vector3 pos) {

			int number = Random.Range (0, 2);
			
			while (number > 0) {
				GameObject skyMonster = (GameObject)Instantiate(skySprite_prefabs[Random.Range(0, skySprite_prefabs.Count)]);
				skyMonster.transform.position = new Vector3(Random.Range(pos.x, pos.x + offset),
				                                       Random.Range(skyMonster.transform.position.y, skyMonster.transform.position.y + maxSkyMonsterDelataHeight),
				                                       skyMonster.transform.position.z);
				skyMonsterList.Add(skyMonster);
				number--;
			}
		}

		void RemoveSkyMonster(Vector3 pos) {

			while (skyMonsterList.Count > 0) {

				GameObject skyMonster = skyMonsterList[0];

				if (skyMonster.transform.position.x < pos.x - offset * 3) {
					Destroy(skyMonster);
					skyMonsterList.RemoveAt(0);
				} else {
					break;
				}
			}
		}

		void OnTriggerEnter2D(Collider2D other) {

			if (other.tag == "Background") {

				GameObject nextBg = (GameObject)Instantiate(bg_prefab);
				nextBg.transform.position = new Vector3 (other.transform.position.x + offset * 2, 
				                                         nextBg.transform.position.y,
				                                         nextBg.transform.position.z);
				bgList.Add(nextBg);

				GenerateCloud(nextBg.transform.position);
				GenerateSkyMonster(nextBg.transform.position);

				if (triggerNum == 1) {
					Destroy(bgList[0]);
					bgList.RemoveAt(0);

					RemoveCloud(nextBg.transform.position);
					RemoveSkyMonster(nextBg.transform.position);

					triggerNum = 0;
				}

				triggerNum++;
			}
		}

		void Start() {
			Random.seed = System.Guid.NewGuid().GetHashCode();

			triggerNum = 0;
			GameObject bg1 = (GameObject)Instantiate(bg_prefab);
			Vector3 pos1 = new Vector3 (offset * 2, bg1.transform.position.y, bg1.transform.position.z);
			bg1.transform.position = pos1;
			bgList.Add(bg1);

			GameObject bg2 = (GameObject)Instantiate(bg_prefab);
			Vector3 pos2 = new Vector3 (offset * 3, bg2.transform.position.y, bg2.transform.position.z);
			bg2.transform.position = pos2;
			bgList.Add(bg2);
		}

		void Update() {
			GameObject hero = GameObject.Find("Hero");
			//follow the target on the x-axis only
			transform.position = new Vector3 (hero.transform.position.x - offset,
			                                  transform.position.y, 
			                                  transform.position.z);
		}
	}
}
