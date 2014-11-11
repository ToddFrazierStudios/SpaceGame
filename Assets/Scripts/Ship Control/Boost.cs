using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {
	public int controllerNumber;
	public float maxBoost;
	public AnimationCurve thrustCurve;
	public float regenRate,consumeRate;

	private float boostRemaining = 1.0f;
	
	// Update is called once per frame
	void FixedUpdate () {
		DebugHUD.setValue("Boost",boostRemaining);
		if(boostRemaining > 0f && ParsedInput.controller[controllerNumber].LS){
			rigidbody.AddForce(transform.forward*thrustCurve.Evaluate(boostRemaining)*maxBoost);
			boostRemaining-=consumeRate;
		}else{
			boostRemaining+=regenRate;
		}
	}
}
