using UnityEngine;
using System.Collections;

public class EngineThruster : MonoBehaviour {
	public float maxThrust;
	public string axis;
	public bool negatetAxis = true;

	public Rigidbody rgb;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.LeftShift) || ParsedInput.controller[0].LeftTrigger > 0) {
			rgb.AddRelativeForce(Vector3.forward * maxThrust);
		}
//		float input = Input.GetAxis(axis);
//		Vector3 force = maxThrust*transform.up*input;
//		if(negatetAxis) force = -force;
//		rgb.AddForceAtPosition(force,transform.position);
//		Debug.DrawRay(transform.position,force,Color.red,0.0f,false);
//		DebugHUD.setValue("Thrust ("+axis+")",input+":"+force);
	}
}
