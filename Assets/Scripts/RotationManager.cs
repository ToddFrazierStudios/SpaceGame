using UnityEngine;
using System.Collections;

public class RotationManager : MonoBehaviour {
	public Rigidbody rgb;
	
	public Vector3 force;
	public Vector3 barrelForce;

	void Awake() {
		
//		networkView.RPC("playerSetup", RPCMode.AllBuffered, Network.player);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float inputX = 0;
		float inputY = 0;
		float inputZ = 0;
		float inputRightX = 0;
		float inputRightY = 0;
		if (Input.GetKey(KeyCode.D)) {
			inputX = force.x;
		} else if (Input.GetKey (KeyCode.A)) {
			inputX = -force.x;
		} else {
			inputX = ParsedInput.controller[0].LeftStickX * force.x;
			if (inputX > 0) {
				inputX = (inputX + 1) * (inputX + 1);
			} else if (inputX < 0) {
				inputX = - (inputX - 1) * (inputX - 1);
			}
		}
		if (Input.GetKey(KeyCode.W)) {
			inputY = force.y;
		} else if (Input.GetKey (KeyCode.S)) {
			inputY = -force.y;
		} else {
			inputY = ParsedInput.controller[0].LeftStickY * force.y;
			if (inputY > 0) {
				inputY = (inputY + 1) * (inputY + 1);
			} else if (inputY < 0) {
				inputY = - (inputY - 1) * (inputY - 1);
			}
		}
		inputRightX = ParsedInput.controller[0].RightStickX * barrelForce.x;
		inputRightY = ParsedInput.controller[0].RightStickY * barrelForce.y;
		inputZ = 0f;
		if(ParsedInput.controller[0].LeftBumper || Input.GetKey(KeyCode.Q)) {
//			if (ParsedInput.controller[0].LeftBumperDown) {
//				if (inputRightX > 0 || inputRightY > 0) {
//					Vector3 strafeVector = new Vector3(inputRightX,inputRightY,0);
//					rgb.AddRelativeForce(strafeVector);
//				}
//			}

			inputZ += force.z;
		}
		if(ParsedInput.controller[0].RightBumper || Input.GetKey(KeyCode.E)) {
			inputZ-=force.z;
		}

		Vector3 forceVector = new Vector3(inputY,inputX,inputZ);
		
		rgb.AddRelativeTorque(forceVector);
	}
}
