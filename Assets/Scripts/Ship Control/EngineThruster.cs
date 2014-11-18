using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EngineThruster : MonoBehaviour {
	public float maxThrust;
	public float maxSpeed;
	public AnimationCurve thrustSpeedCurve;

	private float speedMultiplier;
	public float ThrustMultiplier{
		get{
			return speedMultiplier;
		}
	}
	private float speed;
	public float Speed{
		get{return speed;}
	}

	private Rigidbody rgb;

	//*** INPUT VARIABLES; see ShipController.cs ***//
	[System.NonSerialized]
	public float throttle = 0f; //0f-1f

	void Start(){
		rgb = rigidbody;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		speed = rgb.velocity.magnitude;
//		DebugHUD.setValue("Speed",speed);
	    speedMultiplier = maxSpeed>0 ? thrustSpeedCurve.Evaluate(speed/maxSpeed) : 1f;
//		DebugHUD.setValue("Speed Multiplier",speedMultiplier);

		rgb.AddRelativeForce(Vector3.forward * throttle * maxThrust * speedMultiplier);
	}
}
