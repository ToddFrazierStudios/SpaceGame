﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Boost : MonoBehaviour {
	public int controllerNumber;
	public float maxBoost;
	public AnimationCurve thrustCurve;
	public AnimationCurve rotationCurve;
	public float regenRate,consumeRate;
	public Material meter;

	public float brakeThrust;
	Vector3 brakeVector;

	private float boostRemaining = 1.0f;

	
	// Update is called once per frame
	void FixedUpdate () {
		DebugHUD.setValue("Boost",boostRemaining);
		meter.SetFloat("_ProgressH",boostRemaining);
		if(boostRemaining > 0f && ParsedInput.controller[controllerNumber].LS){
			float curveMultiplier = thrustCurve.Evaluate(boostRemaining);
			float angle = Vector3.Angle(transform.forward,rigidbody.velocity)/180f;
			float angleMultiplier = rotationCurve.Evaluate(angle);

			//Booster
			rigidbody.AddForce(transform.forward*curveMultiplier*maxBoost*angleMultiplier);

			//Active stabilizers
			brakeVector = Vector3.Cross (rigidbody.velocity, transform.forward);
			brakeVector = Vector3.Cross (brakeVector, transform.forward);
			rigidbody.AddForce (brakeVector * brakeThrust * curveMultiplier);
			Debug.DrawRay(transform.position, brakeVector, Color.red);
			Debug.DrawRay (transform.position, transform.forward);
			Debug.DrawRay (transform.position, rigidbody.velocity, Color.green);

			boostRemaining-=consumeRate*angleMultiplier;
		}else{
			if(boostRemaining<1.0f)
				boostRemaining+=regenRate;
			else boostRemaining = 1.0f;
		}
	}
}
