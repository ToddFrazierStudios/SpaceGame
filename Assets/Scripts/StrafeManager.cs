using UnityEngine;
using System.Collections;

public class StrafeManager : MonoBehaviour {
	public Rigidbody rgb;

	public float force;

	// Update is called once per frame
	void FixedUpdate () {
		float inputX = Input.GetAxis("RSx");
		float inputY = -Input.GetAxis("RSy");

		Vector3 forceVector = new Vector3(inputX,inputY,0)*force;

		rgb.AddRelativeForce(forceVector);
	}
}
