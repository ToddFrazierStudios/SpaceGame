using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private bool thirdPerson;
	private GameObject currentCamera;
	public GameObject firstPersonCamera;
	public Vector3 firstPersonPosition;
	public LayerMask firstPersonLayers;
	public LayerMask thirdPersonLayers;

	void Start () {
		Screen.showCursor = false;
		thirdPerson = false;
		currentCamera = firstPersonCamera;
		firstPersonCamera.camera.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (PlayerInput.PollDigitalControlPressed(0, Controls.CAMERA_BUTTON)) {
			thirdPerson = !thirdPerson;
			if (thirdPerson) {
				GetComponent<RadarMount>().enabled = false;
				currentCamera.transform.parent = null;
				currentCamera.GetComponent<OurSmoothFollow>().enabled = true;
				currentCamera.camera.cullingMask = thirdPersonLayers;
			} else {
				GetComponent<RadarMount>().enabled = true;
				currentCamera.transform.parent = gameObject.transform;
				currentCamera.GetComponent<OurSmoothFollow>().enabled = false;
				currentCamera.transform.localPosition = firstPersonPosition;
				currentCamera.transform.localRotation = Quaternion.identity;
				currentCamera.camera.cullingMask = firstPersonLayers;
			}
		}

//		if (ParsedInput.controller[0].RSDown || ParsedInput.controller[0].RSUp) {
//			currentCamera.transform.Rotate (Vector3.up, 180f, Space.Self);
//		}
	}
}
