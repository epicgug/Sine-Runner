using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MovementController {

	void Start() {
	}

	void Update () {
		base.Update ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag.Contains ("Player")) {
			GameController.Instance.score++;
			Destroy (this.gameObject);
		}
	}
}
