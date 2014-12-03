using UnityEngine;
using System.Collections;

namespace Kirigami {
public class CameraFollowerScript : MonoBehaviour {

	public float initXOffset;
	public float startXOffset;
	public float initYOffset;
	public float startYOffset;

	public float maxY, minY;
	
	public Transform player;			//target for the camera to follow
	float xOffset;						//how much x-axis space should be between the camera and target
	float yOffset;

	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
//	public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
//	public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.

	bool toMoveToStartPos = false;
	
	void Awake () {

	}

	void Start() {
		xOffset = initXOffset;
		yOffset = initYOffset;
	}
	
	void Update() {
		//follow the target on the x-axis only
//		transform.position = new Vector3 (player.position.x + xOffset, 
//		                                  transform.position.y, 
//		                                  transform.position.z);
		if (!toMoveToStartPos)
			TrackPlayer ();
	}

	bool CheckXMargin() {
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - (player.position.x + xOffset)) > xMargin;
	}
	
	bool CheckYMargin() {
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - (player.position.y + yOffset)) > yMargin;
	}

	public void ReadyToStartGame() {
		xOffset = startXOffset;
		yOffset = startYOffset;
		toMoveToStartPos = true;
	}
	
	void FixedUpdate () {
//		TrackPlayer();
		if (toMoveToStartPos) {
			MoveToStartPosition();

			if (Mathf.Abs(transform.position.x - (player.position.x + xOffset)) < 0.1f) {
				toMoveToStartPos = false;
				GameControllerScript.current.realStartGame();
			}
		}
	}

	void MoveToStartPosition() {
		float targetX = Mathf.Lerp(transform.position.x, player.position.x + xOffset, xSmooth * Time.deltaTime);
		float targetY = transform.position.y;
		
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}

	void TrackPlayer () {
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
//		float targetX = transform.position.x;
		float targetX = player.position.x + xOffset;
		float targetY = transform.position.y;
		
		// If the player has moved beyond the x margin...
//		if(CheckXMargin())
//			// ... the target x coordinate should be a Lecrp between the camera's current x position and the player's current x position.
//			targetX = Mathf.Lerp(transform.position.x, player.position.x + xOffset, xSmooth * Time.deltaTime);
//		
		float playerY = player.position.y + yOffset;
		if (playerY < minY) {
			playerY = minY;
		} else if (playerY > maxY) {
			playerY = maxY;
		}
		
		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, playerY, ySmooth * Time.deltaTime);
		
//		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
//		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
//		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);


//		
		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
}