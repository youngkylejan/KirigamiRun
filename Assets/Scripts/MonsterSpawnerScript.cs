using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawnerScript : MonoBehaviour {

	public GameObject[] monsters;
	public Camera mainCamera;
	public List<GameObject> monsterList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		StartCoroutine ("SpawnLoop");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StopSpawn() {
		StopCoroutine("SpawnLoop");
	}

	//this is a coroutine which manages when monsters are spawned
	IEnumerator SpawnLoop() {

		//infinite loop: use with caution
		while (true) {
			while (true) {
				if (monsterList.Count > 0) {
					GameObject frontMonster = monsterList[0];

					if (frontMonster.transform.position.x  < mainCamera.transform.position.x - 10.24f) {
						monsterList.RemoveAt(0);
						Destroy(frontMonster);
					}
					else {
						break;
					}
				}
				else {
					break;
				}
			}

			Vector2 pos = new Vector2();

			Random.seed = System.Guid.NewGuid().GetHashCode();
			pos.x = mainCamera.transform.position.x + SceneScrollerScript.amount + Random.Range(0, 20.48f);
			GameObject newMonster = (GameObject) Instantiate(monsters[Random.Range(0, 2)]);

			newMonster.transform.position = pos;
			monsterList.Add(newMonster);

			yield return new WaitForSeconds(Random.Range(1f, 3f));
		}
	}
	
}
