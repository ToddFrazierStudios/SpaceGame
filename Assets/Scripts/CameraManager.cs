using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private bool thirdPerson;
	private GameObject currentCamera;
	public GameObject firstPersonCamera;
	public GameObject thirdPersonCamera;

	void Start () {
		thirdPerson = false;
		currentCamera = firstPersonCamera;
		thirdPersonCamera.camera.enabled = false;
		firstPersonCamera.camera.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (ParsedInput.controller[0].BackDown) {
			thirdPerson = !thirdPerson;
			if (thirdPerson) {
				currentCamera = thirdPersonCamera;
				firstPersonCamera.camera.enabled = false;
				thirdPersonCamera.camera.enabled = true;
			} else {
				currentCamera = firstPersonCamera;
				thirdPersonCamera.camera.enabled = false;
				firstPersonCamera.camera.enabled = true;
			}
		}

		if (ParsedInput.controller[0].RSDown || ParsedInput.controller[0].RSUp) {
			currentCamera.transform.Rotate (Vector3.up, 180f, Space.Self);
		}
	}
}
