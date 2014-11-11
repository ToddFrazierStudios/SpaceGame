using UnityEngine;
using System.Collections;

public class StrafeManager : MonoBehaviour {
	public Rigidbody rgb;

	public float force;

	// Update is called once per frame
	void FixedUpdate () {
		float inputX = ParsedInput.controller[0].RightStickX;
		if (inputX > 0) {
			inputX = (inputX + 1) * (inputX + 1);
		} else if (inputX < 0) {
			inputX = -(inputX - 1) * (inputX - 1);
		}
		float inputY = -ParsedInput.controller[0].RightStickY;
		if (inputY > 0) {
			inputY = (inputY + 1) * (inputY + 1);
		} else if (inputY < 0) {
			inputY = -(inputY - 1) * (inputY - 1);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			inputX = 4;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			inputX = -4;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			inputY = -4;
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			inputY = 4;
		}

		Vector3 forceVector = new Vector3(inputX,inputY,0)*force;

		rgb.AddRelativeForce(forceVector);
	}
}
