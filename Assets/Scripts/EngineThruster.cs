using UnityEngine;
using System.Collections;

public class EngineThruster : MonoBehaviour {
	public float maxThrust;
	public float boostThrust;
	public float reverseThrust;
	public string axis;
	public bool negatetAxis = true;

	public Rigidbody rgb;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
			DebugHUD.setValue("RT", ParsedInput.controller[0].RightTrigger);
			if (Input.GetKey(KeyCode.LeftShift)) {
				rgb.AddRelativeForce(Vector3.forward * maxThrust);
			} else {
				float force = ParsedInput.controller[0].LeftTrigger * maxThrust;
				rgb.AddRelativeForce(Vector3.forward * force);
			}
			if (Input.GetKeyDown (KeyCode.Z) || ParsedInput.controller[0].Xdown) {
				rgb.AddRelativeForce(Vector3.forward * boostThrust, ForceMode.Impulse);
			}
			if (Input.GetKey (KeyCode.X) || ParsedInput.controller[0].B) {
				rgb.AddRelativeForce(Vector3.forward * -boostThrust);
			}
//		float input = Input.GetAxis(axis);
//		Vector3 force = maxThrust*transform.up*input;
//		if(negatetAxis) force = -force;
//		rgb.AddForceAtPosition(force,transform.position);
//		Debug.DrawRay(transform.position,force,Color.red,0.0f,false);
//		DebugHUD.setValue("Thrust ("+axis+")",input+":"+force);
	}
}
