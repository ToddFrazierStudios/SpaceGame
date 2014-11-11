using UnityEngine;
using System.Collections;

public class PassiveBrake : MonoBehaviour {

	public float brakeThrust;
	Vector3 brakeVector;
	
	// Update is called once per frame
	void FixedUpdate () {
		brakeVector = Vector3.Cross (rigidbody.velocity, transform.forward);
		brakeVector = Vector3.Cross (brakeVector, transform.forward);
		rigidbody.AddForce (brakeVector * brakeThrust);
		Debug.DrawRay(transform.position, brakeVector, Color.red);
		Debug.DrawRay (transform.position, transform.forward);
		Debug.DrawRay (transform.position, rigidbody.velocity, Color.green);
	}
}
