using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	private enum State{
		IDLE,TRACKING,FIRE,EVADE
	}
	private State state;

	public enum IdleMode{
		NONE,FOLLOW,PATROL
	}
	public IdleMode idleMode;
	

	public Transform[] patrolNodes;

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
}
