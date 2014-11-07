using UnityEngine;
using System.Collections;

public class StrafeManager : MonoBehaviour {
	public Rigidbody rgb;

	public float force;

	// Update is called once per frame
	void FixedUpdate () {
		if (networkView.isMine) {
			float inputX = ParsedInput.controller[0].RightStickX;
			float inputY = -ParsedInput.controller[0].RightStickY;
			if (Input.GetKey(KeyCode.RightArrow)) {
				inputX = 1;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				inputX = -1;
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				inputY = -1;
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				inputY = 1;
			}

			Vector3 forceVector = new Vector3(inputX,inputY,0)*force;

			rgb.AddRelativeForce(forceVector);
		} else {
			enabled =false;
		}
	}
}
