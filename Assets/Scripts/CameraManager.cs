using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public Transform targetTransform;
	public Vector3 offset = new Vector3(0, 1.710403f, -6.9091f);
	// Use this for initialization
	void Start () {
	
	}



	// Update is called once per frame
	void Update () {
		this.transform.position = targetTransform.position + offset;
		this.transform.rotation = targetTransform.rotation;
	}
}
