using UnityEngine;
using System.Collections;

public class PassiveBrake : MonoBehaviour {

	public float brakeThrust;
	Vector3 brakeVector;
	
	// Update is called once per frame
	void FixedUpdate () {
		brakeVector = Vector3.Cross (GetComponent<Rigidbody>().velocity, transform.forward);
		brakeVector = Vector3.Cross (brakeVector, transform.forward);
		GetComponent<Rigidbody>().AddForce (brakeVector * brakeThrust);
		Debug.DrawRay(transform.position, brakeVector, Color.red);
		Debug.DrawRay (transform.position, transform.forward);
		Debug.DrawRay (transform.position, GetComponent<Rigidbody>().velocity, Color.green);
	}
}
