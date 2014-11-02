using UnityEngine;
using System.Collections;

public class RotationManager : MonoBehaviour {
	public Rigidbody rgb;
	
	public Vector3 force;
	
	// Update is called once per frame
	void FixedUpdate () {
		float inputX = Input.GetAxis("LSx") * force.x;
		float inputY = Input.GetAxis("LSy") * force.y;
		float inputZ = -Input.GetAxis("Bumpers") * force.z;

		Vector3 forceVector = new Vector3(inputY,inputX,inputZ);
		
		rgb.AddRelativeTorque(forceVector);
	}
}
