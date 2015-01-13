using UnityEngine;
using System.Collections;

public class Targeting : MonoBehaviour {
	
	public GameObject targetObject;
	public float targetTimer;
	public Transform target;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			targetObject.transform.position = target.position;
			targetObject.transform.LookAt (camera.transform);
			targetObject.transform.localRotation = transform.rotation;
			targetObject.transform.localScale = new Vector3(12f, 10f, 10f) * Vector3.Distance (targetObject.transform.position, transform.position) / 100f;
		} else {
			targetObject.renderer.enabled = false;
		}

	}
}
