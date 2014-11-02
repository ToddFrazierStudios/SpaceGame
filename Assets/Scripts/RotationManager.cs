using UnityEngine;
using System.Collections;

public class RotationManager : MonoBehaviour {
	public Rigidbody rgb;
	
	public Vector3 force;
	
	// Update is called once per frame
	void FixedUpdate () {
		float inputX = ParsedInput.controller[0].LeftStickX * force.x;
		float inputY = ParsedInput.controller[0].LeftStickY * force.y;
		float inputZ = 0f;
		if(ParsedInput.controller[0].LeftBumper)inputZ-=force.z;
		if(ParsedInput.controller[0].RightBumper)inputZ+=force.z;

		Vector3 forceVector = new Vector3(inputY,inputX,inputZ);
		
		rgb.AddRelativeTorque(forceVector);
	}
}
