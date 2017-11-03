using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;

public class ObstacleSpawner : MonoBehaviour {

	private static ObstacleSpawner _instance;

	public static ObstacleSpawner Instance { get { return _instance; } }

	public class SinePattern {
		public float amplitude;
		public float period;

		public SinePattern(float amplitude, float period) {
			this.amplitude = amplitude;
			this.period = period;
		}
	}

	public float height;
	public float speed;
	public float SpawnDelay;
	public float speedScalar;
	public float maxSpawnVariance;
	public GameObject box;
	public GameObject triangle;
	public GameObject goodBox;
	public GameObject goodTriangle;
	public ArrayList obstacles = new ArrayList ();
	public ArrayList pickups = new ArrayList ();

	private static Random random;
	private List<Collider2D> triggerList = new List<Collider2D>();
	private bool spawnNext = true;
	public float screenWidth;
	public List<SinePattern> sinePatterns = new List<SinePattern>();
	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
		#endif
		random = new Random ();
		speedScalar = (float)SineWave.Instance.stepSize * 10000;
		obstacles.Add (box);
		obstacles.Add (triangle);
		pickups.Add (goodBox);
		pickups.Add (goodTriangle);

		sinePatterns.Add(new SinePattern (.01f, 200));
		sinePatterns.Add(new SinePattern (.02f, 150));
		sinePatterns.Add(new SinePattern (.03f, 100));
		sinePatterns.Add(new SinePattern (.04f, 50));
		sinePatterns.Add(new SinePattern (.05f, 175));
		sinePatterns.Add(new SinePattern (.07f, 125));
		sinePatterns.Add(new SinePattern (.08f, 75));
		sinePatterns.Add(new SinePattern (.09f, 25));
		sinePatterns.Add(new SinePattern (.1f, 360));

		sinePatterns.Shuffle ();
		height = Camera.main.orthographicSize;
		screenWidth = height * Camera.main.aspect;

		BoxCollider2D collider = this.GetComponent<BoxCollider2D> ();
		collider.size = new Vector3 (screenWidth + 2, height * 2);
		collider.transform.position = new Vector3(screenWidth + 2, collider.transform.position.y);
		this.transform.position = new Vector3 (0f, 0f);

		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		createSinePattern (sinePatterns, 10, 1, 0, (GameObject)pickups [0]);
		createWall (box);
	}
	
	// Update is called once per frame
	void Update () {
		MakeObstacleGroup ();
	}

	//Decides when to create an obstacle or pickup depending on score and objects already on screen
	void MakeObstacleGroup() {
		if(triggerList.Count == 0) { //Check if any objects are on screen already
			bool createdObject = false;
			if(GameController.Instance.score <= 20 && !createdObject) {
				createWall (box);
				createdObject = true;
			}
			if(GameController.Instance.score <= 50 && !createdObject) {
				createWall (box);
				createdObject = true;
			}
			if (!createdObject) {
				createWall (box);
			}
		}
	}

	//Deprecated function for primitive random block placement
	public IEnumerator SpawnObstacle() {
		spawnNext = false;
		float waitSeconds = Random.Range (-maxSpawnVariance, maxSpawnVariance);
		yield return new WaitForSeconds (waitSeconds + SpawnDelay);
		int obstacleId = Random.Range (0, obstacles.Count);
		speed = SineWave.Instance.stepSize * speedScalar;
		spawnItem ((GameObject)obstacles[obstacleId]);
		spawnNext = true;
	}


	//Generic sine function with amplitude, period, and degrees
	public float makeSineSpawn(float amplitude, float period, float degree) {
		return Mathf.Rad2Deg * (amplitude * Mathf.Sin ((period * degree)));
	}

	/* Creates obstacles or collectables in a sine pattern, takes a list of sinePatterns and loops through each sinePattern
	 * sinePattern: Class containing amplitude and period
	 * numObjects: Amount of objects for each sine pattern
	 * degreeDelta: Difference of degrees for each obstacle in the sine wave
	 * startDegree: Degree at which the first obstacle is created
	 * item: Obstacle to spawn
	 * */
	public void createSinePattern(List<SinePattern> sinePattern, int numObjects, float degreeDelta, int startDegree, GameObject item) {
		float xSpacing = 3;
		int numPatterns = sinePattern.Count;
		for(int i = 0; i < numPatterns; i++) {
			for (int j = 0; j < numObjects; j++) {
				float angle = (startDegree + j * degreeDelta) * (i + 1);
				float x = (float) (screenWidth + j * xSpacing) + (i * numObjects * xSpacing);
				float y = makeSineSpawn(sinePattern[i].amplitude, sinePattern[i].period, angle);
				//			foreach (SinePattern pattern in sinePattern) {
				//				y *= makeSineSpawn (pattern.amplitude, pattern.period, angle);
				//			}
				spawnItem (item, y, x);
			}
		}
	}
		
	// spawnItem takes an obstacle as a Gameobject and spawns it at a random y position and off the screen
	void spawnItem(GameObject obstacle) {
		float newHeight = Random.Range (-height, height);
		float newSpeed = speed;
		GameObject newObstacle = Instantiate (obstacle, new Vector3 (screenWidth, newHeight, 0), Quaternion.identity);
		newObstacle.GetComponent<ObstacleController>().Create (newSpeed, newHeight, screenWidth);
	}

	// Takes an obstacle as a Gameobject and spawns it at the given x and y positions.
	void spawnItem(GameObject obstacle, float height, float x) {
		float newSpeed = speed;
		GameObject newObstacle = Instantiate (obstacle, new Vector3 (x, height, 0), Quaternion.identity);
		newObstacle.GetComponent<MovementController>().Create (newSpeed, height, x);
	}

	//createWall generates a tower of obstacles with one missing for the player to go through
	void createWall(GameObject obstacle) {
		float size = obstacle.GetComponent<SpriteRenderer> ().bounds.size.y;
		int numObjects = Mathf.RoundToInt(height / size);
		numObjects *= 2;
		float missingPiece = Random.Range (1, numObjects);
		Debug.Log (missingPiece);
		for(int i = 0; i < numObjects; i++) {
			if (i == missingPiece) {
				continue;
			}

			spawnItem (obstacle, -height + (i * size), screenWidth + 1);
		}
	}

	void create

	//Called when something enters the trigger
	void OnTriggerEnter2D(Collider2D other) {
		//If the object is not already in the list
		if(!triggerList.Contains(other)) {
			//Check if gameobject is an obstacle
			if(other.gameObject.tag.Contains("Obstacle")) {
				//Add the object to the list
				triggerList.Add(other);
			}
		}

	}

	//Called when something exits the trigger
	void OnTriggerExit2D(Collider2D other) {
		//If the object is in the list
		if(triggerList.Contains(other)) {
			//remove it from the list
			triggerList.Remove(other);
		}
	}

	

}
