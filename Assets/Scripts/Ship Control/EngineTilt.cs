using UnityEngine;
using System.Collections;

public class EngineTilt : MonoBehaviour {
	public string controllerAxis;
	public Axis axis;
	public float maxSpeed = 100f;
	public float angleOffset = 0f, angleRange = 90f;
	void Start () {
	
	}
	
	void Update () {
		float targetAngle = Input.GetAxis(controllerAxis)*angleRange+angleOffset;
		float currentAngle = 0.0f;
		switch(axis){
		case Axis.X: currentAngle = transform.eulerAngles.x;break;
		case Axis.Y: currentAngle = transform.eulerAngles.y;break;
		case Axis.Z: currentAngle = transform.eulerAngles.z;break;
		}
		float newAngle = Mathf.MoveTowardsAngle(currentAngle,targetAngle,maxSpeed*Time.deltaTime);
		Vector3 eulerAngles = transform.eulerAngles;
		switch(axis){
		case Axis.X: eulerAngles.x = newAngle;break;
		case Axis.Y: eulerAngles.y = newAngle;break;
		case Axis.Z: eulerAngles.z = newAngle;break;
		}
		transform.eulerAngles = eulerAngles;
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0.0f, transform.localEulerAngles.z);
	}

	public enum Axis{
		X,Y,Z
	}
}
