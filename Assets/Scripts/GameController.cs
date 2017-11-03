using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private static GameController _instance;

	public static GameController Instance { get { return _instance; } }

	private float maxAmplitude;
	private float minAmplitude;
	private float minPeriod;
	private float maxPeriod;
	private bool inMouseLoop = false;
	private bool inGameArea;
	public float amplitudeMovementScalar;
	public float periodMovementScalar;
	public int health = 3;
	public int score = 0;
	private Vector3 startYMouse;
	private Vector3 startXMouse;
	// Use this for initialization
	void Start () {
		maxAmplitude = SineWave.Instance.maxAmplitude;
		minAmplitude = SineWave.Instance.minAmplitude;
		maxPeriod = SineWave.Instance.maxPeriod;
		minPeriod = SineWave.Instance.minPeriod;

		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		InvokeRepeating ("IncreaseScore", 1f, 1f);

//		Debug.Log (minAmplitude);
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	
	// Update is called once per frame
	void Update () {
		Control ();
		if(health <= 0) {
			//TODO: Lose Screen
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void IncreaseScore() {
		score++;
	}

	void Control() {
		if(Input.GetMouseButtonDown(0)) {
			inMouseLoop = true;
			this.startXMouse = Input.mousePosition;
			this.startYMouse = Input.mousePosition;
		}

		if(Input.GetMouseButtonUp(0)) {
			inMouseLoop = false;
		}

//		Debug.Log ("mouseYDelta:  " + mouseYDelta / movementScalar + "    newAmplitude: " + newAmplitude);
		if(Input.mousePosition.y != startYMouse.y && inMouseLoop ) { //&& inGameArea
			Vector3 mouseYWorld =  Input.mousePosition;
			float mouseYDelta = mouseYWorld.y - this.startYMouse.y;
			this.startYMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

			float newAmplitude = SineWave.Instance.amplitude + (mouseYDelta / amplitudeMovementScalar);

			if (newAmplitude > minAmplitude && newAmplitude < maxAmplitude) {
				SineWave.Instance.amplitude = newAmplitude;
//				Debug.Log ("Moved to: " + newAmplitude);
			}
		}

		if(Input.mousePosition.x != startXMouse.x && inMouseLoop ) {//&& inGameArea
			float oldPeriod = SineWave.Instance.period;
			float yPos = SineWave.Instance.amplitude * Mathf.Sin (oldPeriod * (Time.time));
			Vector3 mouseXWorld = Input.mousePosition;
			float mouseXDelta = mouseXWorld.x - this.startXMouse.x;
			this.startXMouse = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);

			float newPeriod = SineWave.Instance.period - (mouseXDelta / periodMovementScalar);
//			Debug.Log (SineWave.Instance.minPeriod + "  " + newPeriod + "  " + SineWave.Instance.maxPeriod);

			if(newPeriod > minPeriod && newPeriod < maxPeriod) {
				SineWave.Instance.period = newPeriod;
			}
		}

		if(Input.mousePosition.x > Screen.width * .2) {
			inGameArea = true;
		} else {
			inGameArea = false;
		}










		/*
		 *   mouseWorld = screenToWorld(mouse)
  delta = mouseWorld - start
  start = mouseWorld
		 * /


//		Debug.Log ("inside control");
//		if(Input.touchCount > 0)
//			Debug.Log ("Touch Phase: " + Input.GetTouch (0).phase);
//
//		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
//			Debug.Log ("Touch Count: " + Input.touchCount);
//			Debug.Log ("MoveInside here");
//			float checkSum = SineWave.Instance.amplitude + Input.GetTouch(0).deltaPosition.y;
//			// Get movement of the finger since last frame
//			Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
//			if (minAmplitude < checkSum && checkSum < maxAmplitude) {
//				Debug.Log ("AAAAAAAAAAAAAAAAAAAAA");
//				SineWave.Instance.amplitude += Input.GetTouch (0).deltaPosition.y;
//			}
//		}

//		if(Input.GetMouseButton(0) && !inMouseLoop) {
//			inMouseLoop = true;
//			yStartMouse = Input.mousePosition.y;
//		}
//
//		if(Input.mousePosition.y != yStartMouse && inMouseLoop) {
//			float mouseDelta = Input.mousePosition.y - yStartMouse;
//			Debug.Log (Input.mousePosition.y + "     " + yStartMouse + "   mouseDelta:   " + mouseDelta);
//			yStartMouse = Input.mousePosition.y;
//			Vector3 mouseDeltaWorldCoordinates = new Vector3 (0, mouseDelta, 0);
//			float checkSum = SineWave.Instance.amplitude + mouseDeltaWorldCoordinates.y;
//			Debug.Log ("1 and mouseDelta: " + Camera.main.ScreenToWorldPoint(mouseDeltaWorldCoordinates).y);*/
//
//			if (minAmplitude > checkSum && checkSum < maxAmplitude) {
//				Debug.Log ("2");
//				SineWave.Instance.amplitude -= mouseDeltaWorldCoordinates.y / 100;
//				Debug.Log ("Moved by: " + mouseDeltaWorldCoordinates.y / 100);
//
//			}
//
//		}
//

//

	}
	/*
 * if mouse down
  start = screenToWorld(mouse.x, mouse.y,0)
  moving = true

if moving
  mouseWorld = screenToWorld(mouse)
  delta = mouseWorld - start
  start = mouseWorld
  <do something with delta.X and delta.Y>

if mouse up
  moving = false */
}


  