using UnityEngine;
using System.Collections;

public class RotationManager : MonoBehaviour {
	public Rigidbody rgb;
	
	public Vector3 force;
	public Vector3 barrelForce;
	
	// Update is called once per frame
	void FixedUpdate () {
		float inputX = ParsedInput.controller[0].LeftStickX * force.x;
		float inputY = ParsedInput.controller[0].LeftStickY * force.y;
		float inputRightX = ParsedInput.controller[0].RightStickX * barrelForce.x;
		float inputRightY = ParsedInput.controller[0].RightStickY * barrelForce.y;
		float inputZ = 0f;
		if(ParsedInput.controller[0].LeftBumper){
			if (ParsedInput.controller[0].LeftBumperDown) {
				if (inputRightX > 0 || inputRightY > 0) {
					Vector3 strafeVector = new Vector3(inputRightX,inputRightY,0);
					rgb.AddRelativeForce(strafeVector);
				}
			}

			inputZ += force.z;
		}
		if(ParsedInput.controller[0].RightBumper)inputZ-=force.z;

		Vector3 forceVector = new Vector3(inputY,inputX,inputZ);
		
		rgb.AddRelativeTorque(forceVector);
	}
}
