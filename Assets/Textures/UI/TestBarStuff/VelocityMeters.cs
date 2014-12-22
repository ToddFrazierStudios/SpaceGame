using UnityEngine;
using System.Collections;

public class VelocityMeters : MonoBehaviour {
	public Material verticalMeter,horizontalMeter,zAxisMeter;
	public EngineThruster thruster;
	public Rigidbody rgb;

	// Use this for initialization
	void Start () {
		verticalMeter.SetFloat("_ProgressH",0f);
		verticalMeter.SetFloat("_ProgressV",0f);
		horizontalMeter.SetFloat("_ProgressH",0f);
		horizontalMeter.SetFloat("_ProgressV",0f);
		zAxisMeter.SetFloat("_ProgressH",0f);
		zAxisMeter.SetFloat("_ProgressV",0f);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 localVelocity = transform.InverseTransformVector(rgb.velocity)/thruster.maxSpeed;
		verticalMeter.SetFloat("_ProgressV",localVelocity.y);
		horizontalMeter.SetFloat("_ProgressH",localVelocity.x);
		zAxisMeter.SetFloat ("_ProgressV",localVelocity.z);
	}
}
