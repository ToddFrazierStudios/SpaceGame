using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider), typeof(EngineThruster), typeof(Rigidbody))]
[RequireComponent(typeof(Boost), typeof(StrafeManager), typeof(RotationManager))]
public class AI : MonoBehaviour {

	private enum State{
		IDLE,TRACKING,FIRE,EVADE
	}
	private State state;

	public bool debugMode;

	public enum IdleMode{
		NONE,FOLLOW,PATROL
	}
	public IdleMode idleMode;

	[Range(0f,180f)]
	public float coneAngle;
	

	public Transform[] patrolNodes;

	private GameObject target;

	private List<GameObject> objectsInRadius;
	private List<GameObject> visibleObjects;

	private StrafeManager strafeManager;
	private RotationManager rotationManager;
//	private WeaponsManager weaponsManager;
	private EngineThruster thruster;
	private Boost boost;

	private int currentPatrolNode = 0;

	void Awake(){
		objectsInRadius = new List<GameObject>();
		visibleObjects = new List<GameObject>();
	}

	void Start(){
		strafeManager = GetComponent<StrafeManager>();
		rotationManager = GetComponent<RotationManager>();
//		weaponsManager = GetComponent<WeaponsManager>();
		thruster = GetComponent<EngineThruster>();
		boost = GetComponent<Boost>();
	}

	// Update is called once per frame
	void Update () {
		searchForContacts();
		doFlightControls();
	}

	void OnTriggerEnter(Collider other){
		objectsInRadius.Add(other.gameObject);
	}
	void OnTriggerExit(Collider other){
		if(other.gameObject == target){
			target = null;
			state = State.IDLE;
		}
		objectsInRadius.Remove(other.gameObject);
		visibleObjects.Remove(other.gameObject);
	}

	private void searchForContacts(){
		//CONE OF DOOM
		foreach (GameObject obj in objectsInRadius){
			float angle = Vector3.Angle(transform.forward,obj.transform.position-transform.position);
			if(angle<=coneAngle){
				if(!visibleObjects.Contains(obj)){
					visibleObjects.Add (obj);
				}
			}
		}
	}

	private Vector3 calculateDesiredVelocity(){
		if(debugMode){
			GameObject target = GameObject.Find ("Debug Target");
			if(target){
				return target.transform.position - this.transform.position;
			}
		}
		if(state==State.IDLE){
			if(idleMode == IdleMode.PATROL){
				Transform node = patrolNodes[currentPatrolNode];
				if(Vector3.Distance(node.position,transform.position)<=10f){
					currentPatrolNode++;
					if(currentPatrolNode>=patrolNodes.Length)currentPatrolNode = 0;
					node = patrolNodes[currentPatrolNode];
				}
				return node.position-transform.position;
			}
		}
		return Vector3.zero;
	}

	private void doFlightControls(){
		Vector3 desiredVelocity = calculateDesiredVelocity();
//		transform.LookAt(transform.position+desiredVelocity);
		Debug.DrawRay(transform.position,desiredVelocity,Color.green);
		Vector3 currentVelocity = rigidbody.velocity;
		Vector3 desiredDeltaV = desiredVelocity-currentVelocity;
		Vector3 desiredDeltaVRelative = transform.InverseTransformDirection(desiredDeltaV);
		float mass = rigidbody.mass;

		//Thrust and stabilization
		float dDeltaVRz = desiredDeltaVRelative.z;
		boost.activate = false;
		thruster.throttle = 0f;
		if(dDeltaVRz<0f){
			boost.activate = true;
		}else if(dDeltaVRz>0f){
			float thrustForce = thruster.maxThrust*thruster.ThrustMultiplier;
			float thrusterDeltaV = thrustForce/mass;
			if(thrusterDeltaV<=dDeltaVRz){//if the thruster can't do as much as is needed, floor it!
				thruster.throttle = 1f;
			}else{//thruster is more powerful than needed, we need to throttle it down
				thruster.throttle = dDeltaVRz/thrusterDeltaV;//DIV0?
			}
		}
		DebugHUD.setValue("Throttle", thruster.throttle);

		//Strafe control
		float strafeForce = strafeManager.force*thruster.ThrustMultiplier;
		float strafeDeltaV = strafeForce/mass;
		//X-axis
		float dDeltaVRx = desiredDeltaVRelative.x;
		strafeManager.xInput = Mathf.Clamp(dDeltaVRx/strafeDeltaV,-1f,1f);
		//Y-axis
		float dDeltaVRy = desiredDeltaVRelative.y;
		strafeManager.yInput = -Mathf.Clamp(dDeltaVRy/strafeDeltaV,-1f,1f);

		DebugHUD.setValue("strafeDeltaV",strafeDeltaV);
		DebugHUD.setValue("xStrafe",strafeManager.xInput);
		DebugHUD.setValue("yStrafe",strafeManager.yInput);
		DebugHUD.setValue("dDeltaVR","("+dDeltaVRx+","+dDeltaVRy+","+dDeltaVRz+")");

		//Rotation/Steering control
		rotationManager.xInput = Mathf.Clamp01(XAngle(transform.forward,desiredVelocity)/60f);
		rotationManager.yInput = Mathf.Clamp01(YAngle(transform.forward,desiredVelocity)/60f);

//		float deltaAngleX = XAngle(transform.forward,desiredVelocity);
//		float deltaAngleY = YAngle(transform.forward,desiredVelocity);
//		float maxDeltaAngVelX = rotationManager.force.x/rigidbody.inertiaTensor.x;
//		float maxDeltaAngVelY = rotationManager.force.y/rigidbody.inertiaTensor.y;

		DebugHUD.setValue("rotationManagerX",rotationManager.xInput);
		DebugHUD.setValue("rotationManagerY",rotationManager.yInput);

	}

	private float XAngle(Vector3 from, Vector3 to){
		float basicAngle =  Vector3.Angle (new Vector3(0f,from.y,from.z),new Vector3(0f,to.y,to.z));
		return basicAngle*Mathf.Sign(Vector3.Cross(from,to).x);
	}
	private float YAngle(Vector3 from, Vector3 to){
		float basicAngle =  Vector3.Angle (new Vector3(from.x,0f,from.z),new Vector3(to.x,0f,to.z));
		return basicAngle*Mathf.Sign(Vector3.Cross(from,to).y);
	}

}
