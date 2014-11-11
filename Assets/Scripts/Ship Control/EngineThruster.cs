using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EngineThruster : MonoBehaviour {
	public float maxThrust;
	public float maxSpeed;
	public AnimationCurve thrustSpeedCurve;
	public float ThrustMultiplier{
		get{
			return speedMultiplier;
		}
	}

	private float speedMultiplier;
	private Rigidbody rgb;

	void Start(){
		rgb = rigidbody;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float speed = rgb.velocity.magnitude;
		DebugHUD.setValue("Speed",speed);
	    speedMultiplier = maxSpeed>0 ? thrustSpeedCurve.Evaluate(speed/maxSpeed) : 1f;
		DebugHUD.setValue("Speed Multiplier",speedMultiplier);

		if (Input.GetKey(KeyCode.LeftShift)) {
			rgb.AddRelativeForce(Vector3.forward * maxThrust * speedMultiplier);
		} else {
			float force = ParsedInput.controller[0].LeftTrigger * maxThrust * speedMultiplier;
			rgb.AddRelativeForce(Vector3.forward * force);
		}
	}
}
