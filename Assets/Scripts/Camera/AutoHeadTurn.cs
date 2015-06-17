using UnityEngine;
using System.Collections;

public class AutoHeadTurn : MonoBehaviour {

    private Quaternion startRotation;
    public float multiplier = 2f;
    private Quaternion rotation;
    Rigidbody rgb;

	// Use this for initialization
	void Start () {
        startRotation = transform.rotation;
        rotation = startRotation;
        rgb = transform.parent.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 angularVelocity = rgb.angularVelocity;
        rotation = Quaternion.Euler(angularVelocity * multiplier);
        transform.rotation = rotation * transform.parent.rotation * startRotation;
	}
}
