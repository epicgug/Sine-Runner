using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

	private bool inPlay = false;

	private float speed;
	private float height;
	private float width;

	void Start () {
	}

	public void Update () {
		Move ();
	}

	public void Move() {
		if(inPlay) {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
			if(this.transform.position.x < ((width * -1) - 1)) {
				Destroy (this.gameObject);
			}
		}
		this.speed = SineWave.Instance.stepSize * ObstacleSpawner.Instance.speedScalar;
	}

	public void Create(float speed, float height, float width) {
		this.speed = speed;
		this.height = height;
		this.width = width;
		inPlay = true;

	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log ("sdfasdaf");
		if (col.gameObject.tag.Contains ("Player")) {
			GameController.Instance.health--;
			Destroy (this.gameObject);
		}
	}
}
