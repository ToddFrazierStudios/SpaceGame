using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody)/*,typeof(RigidbodyInfo)*/)]
public class WobbleGenerator : MonoBehaviour {
	private Rigidbody body;
	public Transform cameraTransform;
//	private RigidbodyInfo bodyInfo;
	public float forceMultiplier=1f,torqueMultiplier=1f;
	public float velocityThreshold = 1f;

	// Use this for initialization
	void Start () {
		body = rigidbody;
//		bodyInfo = GetComponent<RigidbodyInfo>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		DebugHUD.setValue ("Veclocity", body.velocity.magnitude);
		if(body.velocity.magnitude<velocityThreshold)return;
		cameraTransform.position += new Vector3(1, 1, 0) * ((Random.value * 2) - 1) * forceMultiplier;
//		cameraRigidbody.AddTorque(Random.onUnitSphere*torqueMultiplier);
		Debug.Log("added force");
	}
}
