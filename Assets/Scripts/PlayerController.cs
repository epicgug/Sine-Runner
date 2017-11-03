using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public SineWave sineWave;
	public Slider startMaxSlider;
	public Slider endMaxSlider;
	// Use this for initialization
	void Start () {
		sineWave = this.GetComponent<SineWave> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * .2f, 0, 0)) + new Vector3(0, sineWave.amplitude * Mathf.Sin (SineWave.Instance.t), 0);
		newPos.y += 5;
		newPos.z = 1;
		this.transform.position = newPos;
//		Debug.Log ("Time.time = " + Time.time + "  Sin = " + Mathf.Sin (Time.time) + "   newPos Y = " + newPos.y);
	}

	public void updateStartSineWave() {
		sineWave.startWidthOfLine = startMaxSlider.value;
	}

	public void updateEndSineWave() {
		sineWave.endWidthOfLine = endMaxSlider.value;
	}


}

