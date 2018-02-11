using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTracker : DeviceTracker {

	public AxisKeys[] axisKeys;
	public KeyCode[] buttonKeys;

	//Overide built in Unity: resests length of two input arrays to match input manager
	void Reset() {
		im = GetComponent<InputManager> ();
		axisKeys = new AxisKeys[im.axisCount];
		buttonKeys = new KeyCode[im.buttonCount];
	}

	// Update is called once per frame
	void Update () {

		//POPULATE INPUT DATA STRUCTURE TO PASS TO INPUT MANAGER

	 	//Input found
		for (int i = 0; i < axisKeys.Length; i++) {
			float val = 0f;
			if (Input.GetKey(axisKeys [i].positive)) {
				val = val + 1f;
				newData = true;
			}
			if (Input.GetKey(axisKeys [i].negative)) {
				val = val - 1f;
				newData = true;
			}
			data.axes [i] = val;
		}

		for (int i = 0; i < buttonKeys.Length; i++) {
			if (Input.GetKey(buttonKeys [i])) {
				data.buttons [i] = true;
				newData = true;
			}
		}

		//CHECK FOR INPUT
		if (newData) {
			im.PassInput (data);
			newData = false;
			data.Reset();
		}
	}
}

[System.Serializable]
public struct AxisKeys {
	public KeyCode positive;
	public KeyCode negative;
}
