using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class AI : MonoBehaviour {

	private enum State{
		IDLE,TRACKING,FIRE,EVADE
	}
	private State state;

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

	void Awake(){
		objectsInRadius = new List<GameObject>();
		visibleObjects = new List<GameObject>();
	}

	// Update is called once per frame
	void Update () {
		switch(state){
		case State.IDLE:
			switch(idleMode){
			case IdleMode.NONE:

				break;
			case IdleMode.FOLLOW:

				break;
			case IdleMode.PATROL:

				break;
			}
			//CONE OF DOOM
			foreach (GameObject obj in objectsInRadius){
				float angle = Vector3.Angle(transform.forward,obj.transform.position-transform.position);
				if(angle<=coneAngle){
					if(!visibleObjects.Contains(obj)){
						visibleObjects.Add (obj);
					}
				}
			}


			break;
		case State.TRACKING:
			//DON'T TURN OFF YOUR TARGETING COMPUTER
			break;
		case State.FIRE:
			//FIRE EVERYTHING!
			break;
		case State.EVADE:
			//DODGE DUCK DIP DIVE AND DODGE
			break;
		}
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
}
