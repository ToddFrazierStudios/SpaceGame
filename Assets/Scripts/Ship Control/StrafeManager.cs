using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EngineThruster))]
public class StrafeManager : MonoBehaviour {
	public Rigidbody rgb;

	public float force;

	private EngineThruster thruster;

	//*** INPUT VARIABLES; see ShipController.cs ***//
	[System.NonSerialized]
	public float xInput = 0f, yInput = 0f;//these are the values read from the controller, to be supplied by another script

	void Start(){
		thruster = GetComponent<EngineThruster>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		float inputX = xInput;
		if (inputX > 0) {
			inputX = (inputX + 1) * (inputX + 1);
		} else if (inputX < 0) {
			inputX = -(inputX - 1) * (inputX - 1);
		}
		float inputY = yInput;
		if (inputY > 0) {
			inputY = (inputY + 1) * (inputY + 1);
		} else if (inputY < 0) {
			inputY = -(inputY - 1) * (inputY - 1);
		}

		Vector3 forceVector = new Vector3(inputX,inputY,0)*force;

		rgb.AddRelativeForce(forceVector*thruster.ThrustMultiplier);
	}
}
