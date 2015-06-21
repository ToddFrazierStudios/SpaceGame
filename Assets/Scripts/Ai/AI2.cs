using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI2 : MonoBehaviour {

	private enum State{
		IDLE,TRACKING,FIRE,EVADE
	}
	private State state;

	public enum IdleMode{
		NONE,FOLLOW,PATROL
	}
	public IdleMode idleMode;
	
	[Range(0f,180f)]
	public float coneAngle = 45;
	
	public Transform[] patrolNodes;
	
	private GameObject target;
	
	private List<GameObject> objectsInRadius;
	private List<GameObject> visibleObjects;
	
	private StrafeManager strafeManager;
	private RotationManager rotationManager;
	//private WeaponsManager weaponsManager;
	private EngineThruster thruster;

	private int currentPatrolNode = 0;
	
	void Awake(){
		objectsInRadius = new List<GameObject>();
		visibleObjects = new List<GameObject>();
	}

	// Use this for initialization
	void Start () {
		strafeManager = GetComponent<StrafeManager>();
		rotationManager = GetComponent<RotationManager>();
		//weaponsManager = GetComponent<WeaponsManager>();
		thruster = GetComponent<EngineThruster>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
