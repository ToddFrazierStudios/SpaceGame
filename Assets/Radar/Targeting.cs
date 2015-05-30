using UnityEngine;
using System.Collections;

public class Targeting : MonoBehaviour {

	public Camera playerCamera;
	public float range;
    public float radius = 5;
	public float coneAngle;
	public int layerMask;
	public GameObject targetObject;
	public float targetTimer;
	public Transform target;
	public Vector3 hitPosition;


	// Use this for initialization
	void Start () {
		layerMask = 1 << 13;
		layerMask = ~layerMask;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public Transform getTarget() {
		RaycastHit hit;
		target = null;
		if (Physics.SphereCast (transform.position, radius, transform.forward, out hit, range, layerMask)) {
			if (Vector3.Angle (transform.forward, hit.point - transform.position) < coneAngle) {
				target = hit.transform;
				hitPosition = hit.point;
			}
		}
		return target;
	}
}
