using UnityEngine;
using System.Collections;

public class RotationManager : MonoBehaviour {
	public Rigidbody rgb;
	
	public Vector3 force;
	public Vector3 barrelForce;


	//*** INPUT VARIABLES; see ShipController.cs ***//
	[System.NonSerialized]
	public float xInput = 0f,yInput = 0f, xRightInput = 0f, yRightInput = 0f;

	[System.NonSerialized]
	public bool rotateLeft = false, rotateRight = false;

	void Awake() {
		Vector3 inert = rgb.inertiaTensor;
		Debug.Log (inert);
		inert = Vector3.one*((inert.x+inert.y+inert.z)/3);
		rgb.inertiaTensor = inert;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float inputX = 0;
		float inputY = 0;
		float inputZ = 0;
		float inputRightX = 0;
		float inputRightY = 0;

		inputX = xInput * force.x;
		if (inputX > 0) {
			inputX = (inputX + 1) * (inputX + 1);
		} else if (inputX < 0) {
			inputX = - (inputX - 1) * (inputX - 1);
		}

		inputY = yInput * force.y;
		if (inputY > 0) {
			inputY = (inputY + 1) * (inputY + 1);
		} else if (inputY < 0) {
			inputY = - (inputY - 1) * (inputY - 1);
		}

		inputRightX = xRightInput * barrelForce.x;
		inputRightY = yRightInput * barrelForce.y;
		inputZ = 0f;
		if(rotateLeft) {
//			if (ParsedInput.controller[0].LeftBumperDown) {
//				if (inputRightX > 0 || inputRightY > 0) {
//					Vector3 strafeVector = new Vector3(inputRightX,inputRightY,0);
//					rgb.AddRelativeForce(strafeVector);
//				}
//			}

			inputZ += force.z;
		}
		if(rotateRight) {
			inputZ-=force.z;
		}

		Vector3 forceVector = new Vector3(inputY,inputX,inputZ);
		
		rgb.AddRelativeTorque(forceVector);
	}
}
