using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour {
	[Tooltip("The Rigidbody that the target transform (the parent transform) is attached to.")]
	public Rigidbody rgb;
//	[Tooltip("The Transform object that represents the target position of the Camera.")]
//	public Transform target;//I will just use the parent
	[Tooltip("The maximum distance the Camera can be from the target Transform.")]
	public float maxDistance = 0.25f;
	[Tooltip("The amount of acceleration that the target must undergo in order for the Camera to be at the max distance.")]
	public float maxAcceleration = 0.5f;
	[Tooltip("Set to true in order to forcibly keep the Camera within the max distance.")]
	public bool clampToMax = true;

	[Tooltip("The maximum angle allowed between the target and the Camera.")]
	public float maxAngleDelta = 15.0f;
	[Tooltip("The angular acceleration needed to make the Camera be at the maxAngleDelta.")]
	public float maxAnglularAcceleration;
	[Tooltip("Set to true in order to forcibly keep the Camera within the max angle.")]
	public bool clampToMaxAngle = true;




	public float smoothTime = 0.5f;
	public float maxSpeed = 3.0f;

//	public bool clampRotation;//??

	//the velocity of the target the last time we checked
	private Vector3 previousVelocity = Vector3.zero;
	private Vector3 previousAngVelocity = Vector3.zero;

	private Vector3 cameraVelocity = Vector3.zero;
	private Vector3 cameraAngVelocity = Vector3.zero;

	private Transform target;

	//NOTES TO SELF:
	//-should the camera "bounce off" when it hits its bounds?

	//Current implementation:
	//the camera's position is solely based on the acceleration of the target.  

	// Use this for initialization
	void Start () {
		target = transform.parent;
		if(target == null){
			Debug.LogError("HeadBob requires a parent object to act as a target!");
			enabled = false;
		}
	}
	
	// Fun Fact: Update is called once per frame
	void FixedUpdate () {
		if (target.transform) {
			Vector3 currentVelocity = rgb.GetPointVelocity(target.position);
			Vector3 accel = currentVelocity-previousVelocity;
			Vector3 positionOffset = accel / maxAcceleration * maxDistance;

			Vector3 currentAngVelocity = rgb.angularVelocity;
			Vector3 angAccel = currentAngVelocity-previousAngVelocity;
			Vector3 angleOffset = angAccel / maxAnglularAcceleration * maxAngleDelta;

			if(clampToMax){
				positionOffset = Vector3.ClampMagnitude(positionOffset,maxDistance);
			}
			if(clampToMaxAngle){
				angleOffset.x = Mathf.Clamp(angleOffset.x, -maxAngleDelta, maxAngleDelta);
				angleOffset.y = Mathf.Clamp(angleOffset.y, -maxAngleDelta, maxAngleDelta);
				angleOffset.z = Mathf.Clamp(angleOffset.z, -maxAngleDelta, maxAngleDelta);
			}
			Vector3 targetPosition = target.position+positionOffset;//where the camera wants to be
			transform.position = Vector3.SmoothDamp(transform.position,targetPosition,ref cameraVelocity,smoothTime,maxSpeed,Time.fixedDeltaTime);

			Vector3 targetRotation = target.eulerAngles + angleOffset;//how the camera wants to be pointed
			transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(targetRotation),Time.fixedDeltaTime);

			previousVelocity = currentVelocity;
			previousAngVelocity = currentAngVelocity;
			DebugHUD.setValue(gameObject.name+".HeadBob.Acceleration",accel);
			DebugHUD.setValue(gameObject.name+".HeadBob.AngAcceleration",angAccel);
		}
	}
}
