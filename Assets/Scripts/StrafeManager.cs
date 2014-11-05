using UnityEngine;
using System.Collections;

public class StrafeManager : MonoBehaviour {
	public Rigidbody rgb;

	public float force;

	// Update is called once per frame
	void FixedUpdate () {
		float inputX;
		float inputY;
		if (Input.GetKey(KeyCode.RightArrow)) {
			inputX = force;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			inputX = -force;
		} else {
			inputX = ParsedInput.controller[0].LeftStickX * force;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			inputY = force;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			inputY = -force;
		} else {
			inputY = ParsedInput.controller[0].LeftStickY * force;
		}
		Vector3 forceVector = new Vector3(inputX,inputY,0);

		rgb.AddRelativeForce(forceVector);
	}
}
