using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private static UIController _instance;

	public static UIController Instance { get { return _instance; } }


	private void Awake() {
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	public Text startColorHex;
	public Text endColorHex;
	public Text scoreText;
	public string startColorHexString;
	public string endColorHexString;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateText ();
	}

	void UpdateText() {
		scoreText.text = "Score: " + GameController.Instance.score;
		startColorHexString = startColorHex.text;
//		startColorHexString = startColorHexString.Remove (0, 1);
		endColorHexString = endColorHex.text;
//		endColorHexString = endColorHexString.Remove (0, 1);
	}
}
