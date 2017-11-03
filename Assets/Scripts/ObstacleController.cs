using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MovementController {

	void Start () {
	}

	void Update() {
		base.Update ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag.Contains ("Player")) {
			GameController.Instance.health--;
			Destroy (this.gameObject);
		}
	}
}
