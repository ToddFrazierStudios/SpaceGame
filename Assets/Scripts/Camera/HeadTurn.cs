using UnityEngine;
using System.Collections;

public class HeadTurn : MonoBehaviour {

	private Quaternion startRotation;
	public Vector2 force;
	private Quaternion rotation;
	[System.NonSerialized]
	public float xInput;
	[System.NonSerialized]
	public float yInput;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		float inputX = 0;
		float inputY = 0;
		float inputZ = 0;
		
		inputX = xInput * force.x;
		if (inputX > 0) {
			inputX = (inputX + 1) * (inputX + 1);
		} else if (inputX < 0) {
			inputX = - (inputX - 1) * (inputX - 1);
		}
		
		inputY = yInput * force.y;
		if (inputY > 0) {
			inputY = (inputY + 1) * (inputY + 1);
		} else if (inputY < 0) {
			inputY = - (inputY - 1) * (inputY - 1);
		}
		transform.Rotate(transform.parent.up, inputX, Space.World);
		transform.Rotate(transform.right, inputY, Space.World);
	}
	
}
