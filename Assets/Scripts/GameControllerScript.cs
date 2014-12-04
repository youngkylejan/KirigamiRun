using UnityEngine;
using System.Collections;

namespace Kirigami
{
public class GameControllerScript : MonoBehaviour {

	public static GameControllerScript current;		//a reference to our game control so we can access it statically
	public GUIText scoreText;						//a reference to text that shows the player's score
	public GameObject gameOvertext;	//a reference to the object that contains the text that appears when the player dies
	public GameObject highestText;
	public GameObject reStartButton;
	public GameObject startButton;
	public GameObject title;
	public GameObject hero;
	public GameObject mainCamera;
	public int startPosition;
	int score = 0;								//the player's score
	bool isGameOver = false;					//is the game over?

	Hero.HeroScript heroScript;
	CameraFollowerScript cameraScript;

	// Use this for initialization
	void Start () {
		heroScript = hero.GetComponent<Hero.HeroScript>();
		cameraScript = mainCamera.GetComponent<CameraFollowerScript>();
	}

	void Awake() {

		startPosition = (int)hero.transform.position.x;
		//if we don't currently have a game control...
		if (current == null)
			//...set this one to be it...
			current = this;
		//...otherwise...
		else if(current != this)
			//...destroy this one because it is a duplicate
			Destroy (gameObject);
	}

	public void startGame()
	{
		startButton.SetActive (false);
		title.SetActive (false);
		cameraScript.ReadyToStartGame();
		heroScript.Ready();
	}

	public void realStartGame()
	{
		scoreText.gameObject.SetActive (true);
		heroScript.StartRunning();

		mainCamera.GetComponentInChildren<AudioSource>().Play();
	}
	
	void Update() {
		//if the game is over and the player has pressed some input...
		HeroScored ();
	}
	
	public void HeroScored() {
		//the bird can't score if the game is over
		if (!isGameOver) {

			score = (int)hero.transform.position.x - startPosition;
			scoreText.text = "Score: " + ((score < 0) ? 0 : score);
		}
	}

	public void setHighest()
	{
		if(PlayerPrefs.HasKey("Highest") == false){
			
			PlayerPrefs.SetInt("Highest",score);
			return;
			
		}else{
			
			if(PlayerPrefs.GetInt("Highest") < score){
				PlayerPrefs.SetInt("Highest",score);
			}
		}
	}
	
	public void HeroDied() 
	{
		setHighest();

		activateGameOverText ();

		isGameOver = true;
//			print ("HERO DIE");

		mainCamera.GetComponentInChildren<AudioSource>().Stop();
	}

	public void activateGameOverText()
	{
		gameOvertext.SetActive (true);
		gameOvertext.guiText.text = "YOUR RESULT:" + score;
		
		highestText.SetActive (true);
		highestText.guiText.text = "HIGHEST:" + PlayerPrefs.GetInt ("Highest");
		
		reStartButton.SetActive (true);
		
		scoreText.gameObject.SetActive (false);
	}
}
}