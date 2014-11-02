using UnityEngine;
using System.Collections;

public class StrafeManager : MonoBehaviour {
	public Rigidbody rgb;

	public float force;

	// Update is called once per frame
	void FixedUpdate () {
		float inputX = ParsedInput.controller[0].RightStickX;
		float inputY = -ParsedInput.controller[0].RightStickY;

		Vector3 forceVector = new Vector3(inputX,inputY,0)*force;

		rgb.AddRelativeForce(forceVector);
	}
}
