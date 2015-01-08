using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EngineThruster : MonoBehaviour {
	public float maxThrust;
	public float maxSpeed;
	public AnimationCurve thrustSpeedCurve;
	public bool useStabilizers = true;
	public float brakeThrust = 25000;
	[System.NonSerialized]
	public bool isStrafing = false;
	private Vector3 brakeVector;
	
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
	
	void Awake() {
		throttle = 0f;
	}
	
	void Start(){
		rgb = rigidbody;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		DebugHUD.setValue ("LT", ParsedInput.controller[0].LeftTrigger);
		DebugHUD.setValue ("RT", ParsedInput.controller[0].RightTrigger);
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
		
		if (useStabilizers && !isStrafing) {
			brakeVector = Vector3.Cross (rigidbody.velocity, transform.forward);
			brakeVector = Vector3.Cross (brakeVector, transform.forward);
			rigidbody.AddForce (brakeVector * brakeThrust * throttle);
		}
		
		DebugHUD.setValue("LocalMaxThrust",localMaxThrust);
		DebugHUD.setValue("Speed Multiplier",speedMultiplier);
		
		rgb.AddRelativeForce(Vector3.forward * throttle * localMaxThrust * speedMultiplier);
	}
}
