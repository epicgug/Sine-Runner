using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class SineWave : MonoBehaviour {

	private static SineWave _instance;

	public static SineWave Instance { get { return _instance; } }


	private void Awake() {
		Vector3 maxAmpVector3 = Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height, 0));
		maxAmplitude = maxAmpVector3.y - .5f;
		minAmplitude = .5f;
		minPeriod = .1f;
		maxPeriod = 5f;

		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	public Shader lineShader;
	public Color32 c1 = Color.yellow;
	public Color32 c2 = Color.red;
	public int lengthOfLineRenderer;
	public float startWidthOfLine;
	public float endWidthOfLine;
	public float period;
	public float stepSize;
	public float amplitude;

	public float maxAmplitude;
	public float minAmplitude;
	public float maxPeriod;
	public float minPeriod;

	public float t = 0;
	public float timeFactor;

	void Start() {
//		lengthOfLineRenderer *= scalar;
//		startWidthOfLine /= scalar;
//		endWidthOfLine /= scalar;
//		period /= scalar;
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(lineShader);
	}
	void Update() {
		RenderLine ();
		UpdateUIValues ();
	}

	void RenderLine() {
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = true;

		int i = 0;

		var points = new Vector3[lengthOfLineRenderer];
		timeFactor = period;
		t += Time.deltaTime * timeFactor;

		if(t > 2 * Mathf.PI) {
			t -= (2 * Mathf.PI);
		}



		while (i < lengthOfLineRenderer) {
			points[i] = new Vector3(i * stepSize, amplitude * Mathf.Sin(period * (i * stepSize) + t), 0);
			i++;
		}

		for(int j = 0; j < points.Length; j++) {
			Vector3 newXPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * .2f, 0, 0));
			points [j].x += newXPosition.x;
		}

		lineRenderer.SetPositions(points);
		lineRenderer.startWidth = startWidthOfLine;
		lineRenderer.endWidth = endWidthOfLine;
		lineRenderer.positionCount = lengthOfLineRenderer;
	}

	void UpdateUIValues() {
//		Debug.Log (UIController.Instance.startColorHexString);
//		Debug.Log (int.Parse (UIController.Instance.startColorHexString));
//		c1 = Parse(UIController.Instance.startColorHexString);
//		c2 = Parse(UIController.Instance.endColorHexString);
//		Debug.Log (c1);
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.startColor = c1;
		lineRenderer.endColor = c2;
	}

	public static Color32 Parse(string hexstring) {
		if (hexstring.StartsWith("#"))
		{
			hexstring = hexstring.Substring(1);
		}

		if (hexstring.StartsWith("0x"))
		{
			hexstring = hexstring.Substring(2);
		}

		if (hexstring.Length != 6) 
		{
			throw new UnityException(string.Format("{0} is not a valid color string.", hexstring));
		}

		byte r = byte.Parse(hexstring.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hexstring.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hexstring.Substring(4, 2), NumberStyles.HexNumber);

		return new Color32(r, g, b, 255);
	}
}
