using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {

	public static GameControllerScript current;		//a reference to our game control so we can access it statically
	public MonsterSpawnerScript monsterSpawner;		//a reference to the monster spawner
	public GUIText scoreText;						//a reference to text that shows the player's score
	public GameObject gameOvertext;					//a reference to the object that contains the text that appears when the player dies
	
	int score = 0;								//the player's score
	bool isGameOver = false;					//is the game over?

	// Use this for initialization
	void Start () {
		
	}

	void Awake() {
		//if we don't currently have a game control...
		if (current == null)
			//...set this one to be it...
			current = this;
		//...otherwise...
		else if(current != this)
			//...destroy this one because it is a duplicate
			Destroy (gameObject);
	}
	
	void Update() {
		//if the game is over and the player has pressed some input...
		if (isGameOver && Input.anyKey) {
			//...start a new game.
			Application.LoadLevel(Application.loadedLevel);		
		}
	}
	
	public void BirdScored() {
		//the bird can't score if the game is over
		if (isGameOver)	
			return;
		//increase score
		score++;
		//adjust the score text
		scoreText.text = "Score: " + score;
	}
	
	public void HeroDied() {
		//don't spawn new monsters
		monsterSpawner.StopSpawn ();
		//show the game over text
		gameOvertext.SetActive (true);
		//set the game to be over
		isGameOver = true;
	}
}
