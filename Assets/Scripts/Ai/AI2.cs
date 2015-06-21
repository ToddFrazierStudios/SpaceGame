using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI2 : MonoBehaviour {

	private enum State{
		POSITIONING, FIRING, EVADING
	}
	private State state;
	
	private StrafeManager strafeManager;
	private RotationManager rotationManager;
	private WeaponsManager weaponsManager;
	private EngineThruster thruster;

	public GameObject target;

	//public float minfiringDist;
	public float maxFiringDist;
	public float accelerationDist;
	public float maxEvadeTime;
	public float maxFiringTime;

	private float stateStarted;

	public float xGain = -4;
	public float yGain = 4;
	
	void Awake(){
	}

	// Use this for initialization
	void Start () {
		strafeManager = GetComponent<StrafeManager>();
		rotationManager = GetComponent<RotationManager>();
		weaponsManager = GetComponent<WeaponsManager>();
		thruster = GetComponent<EngineThruster>();
		state = State.POSITIONING;
		stateStarted = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		// update state
		float dist = (gameObject.transform.position - target.transform.position).magnitude;
		switch (state) {
		case State.EVADING:
			if (stateTime() > maxEvadeTime || dist > maxFiringDist + accelerationDist) {
				state = State.POSITIONING;
			}
			break;
		case State.POSITIONING:
			if (dist < maxFiringDist) {
				state = State.FIRING;
			}
			break;
		case State.FIRING:
			if (stateTime() > maxFiringTime || dist > maxFiringDist) {
				state = State.EVADING;
			}
			break;
		}
		// perform actions based on state
		Vector3 proj = gameObject.transform.InverseTransformPoint(target.transform.position);
		proj = proj / proj.magnitude;
		switch (state) {
		case State.EVADING:
			proj = -proj;
			if (proj.z > 0) {
				rotationManager.xInput = Mathf.Clamp(-xGain * proj.x, -1, 1);
				rotationManager.yInput = Mathf.Clamp(-yGain * proj.y, -1, 1);
				thruster.throttle = 1;
			} else {
				if (proj.x > 0) {
					rotationManager.xInput = Mathf.Sign(-xGain);
				} else {
					rotationManager.xInput = Mathf.Sign(xGain);
				}
				if (proj.y > 0) {
					rotationManager.yInput = Mathf.Sign(-yGain);
				} else {
					rotationManager.yInput = Mathf.Sign(yGain);
				}
				thruster.throttle = 0;
			}
			break;
		case State.POSITIONING:
			if (proj.z > 0) {
				rotationManager.xInput = Mathf.Clamp(-xGain * proj.x, -1, 1);
				rotationManager.yInput = Mathf.Clamp(-yGain * proj.y, -1, 1);
				thruster.throttle = 1;
			} else {
				if (proj.x > 0) {
					rotationManager.xInput = Mathf.Sign(-xGain);
				} else {
					rotationManager.xInput = Mathf.Sign(xGain);
				}
				if (proj.y > 0) {
					rotationManager.yInput = Mathf.Sign(-yGain);
				} else {
					rotationManager.yInput = Mathf.Sign(yGain);
				}
				thruster.throttle = 0;
			}
			break;
		case State.FIRING:
			if (proj.z > 0) {
				rotationManager.xInput = Mathf.Clamp(-xGain * proj.x, -1, 1);
				rotationManager.yInput = Mathf.Clamp(-yGain * proj.y, -1, 1);
			} else {
				if (proj.x > 0) {
					rotationManager.xInput = Mathf.Sign(-xGain);
				} else {
					rotationManager.xInput = Mathf.Sign(xGain);
				}
				if (proj.y > 0) {
					rotationManager.yInput = Mathf.Sign(-yGain);
				} else {
					rotationManager.yInput = Mathf.Sign(yGain);
				}
			}
			thruster.throttle = 0;
			weaponsManager.primaryFire = true;
			break;
		}
	}

	private float stateTime() {
		return Time.time - stateStarted;
	}
}
