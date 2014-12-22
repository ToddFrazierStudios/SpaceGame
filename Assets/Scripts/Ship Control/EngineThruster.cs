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
		Vector3 localVelocity = transform.InverseTransformVector(rgb.velocity);
		DebugHUD.setValue("Local Velocity",localVelocity);
		speed = rgb.velocity.magnitude;
	    speedMultiplier = maxSpeed>0 ? thrustSpeedCurve.Evaluate(speed/maxSpeed) : 1f;
		float localMaxThrust = maxThrust;

		if(localVelocity.z<0f){
			localMaxThrust = -rgb.mass*localVelocity.z*5;
			if(localMaxThrust<maxThrust){
				localMaxThrust = maxThrust;
			}
		}

		DebugHUD.setValue("LocalMaxThrust",localMaxThrust);
		DebugHUD.setValue("Speed Multiplier",speedMultiplier);

		rgb.AddRelativeForce(Vector3.forward * throttle * localMaxThrust * speedMultiplier);
	}
}
