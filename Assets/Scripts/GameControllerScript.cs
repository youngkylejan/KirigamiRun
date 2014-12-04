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

	GameObject[] heros;
	GameObject[] heroSelectionDisabledObjects, heroSelectionEnabledObjects;
	GameObject heroSelectionDisabled, heroSelectionEnabled;

	Hero.HeroScript heroScript;
	CameraFollowerScript cameraScript;

	// Use this for initialization
	void Start () {
		heroScript = hero.GetComponent<Hero.HeroScript>();
		cameraScript = mainCamera.GetComponent<CameraFollowerScript>();

		heros = new GameObject[] {
				GameObject.Find("Hero/StupidDragonBody"),
//				GameObject.Find("Hero/DragonBody"),
//				GameObject.Find("Hero/CuteBody"),
			};

		foreach (GameObject obj in heros) {
			obj.SetActive(false);
		}
		heros[0].SetActive(true);

		heroSelectionDisabledObjects = new GameObject[] {
				GameObject.Find("HeroListDisabled/1"),
//				GameObject.Find("HeroListDisabled/2"),
//				GameObject.Find("HeroListDisabled/3"),
			};

		heroSelectionEnabledObjects = new GameObject[] {
				GameObject.Find("HeroListEnabled/1"),
//				GameObject.Find("HeroListEnabled/2"),
//				GameObject.Find("HeroListEnabled/3"),
			};

		heroSelectionDisabled = GameObject.Find ("HeroListDisabled");
		heroSelectionEnabled = GameObject.Find ("HeroListEnabled");

		for (int i = 0; i < heroSelectionDisabledObjects.Length; ++i) {
			heroSelectionEnabledObjects[i].SetActive(false);
		}

		heroSelectionDisabled.SetActive(false);
		heroSelectionEnabled.SetActive(false);

		heroSelectionEnabledObjects[0].SetActive(true);
		heroSelectionDisabledObjects[0].SetActive(false);
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

		heroSelectionDisabled.SetActive(false);
		heroSelectionEnabled.SetActive(false);
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

	public void heroSelection(int index) {
		index -= 1;
		for (int i = 0; i < heroSelectionEnabledObjects.Length; ++i) {
			if (i == index) {
				heroSelectionEnabledObjects[i].SetActive(true);
				heroSelectionDisabledObjects[i].SetActive(false);
				heros[i].SetActive(true);
			} else {
				heroSelectionEnabledObjects[i].SetActive(false);
				heroSelectionDisabledObjects[i].SetActive(true);
				heros[i].SetActive(false);
			}
		}

		heroScript.RefreshHero();
	}
}
}