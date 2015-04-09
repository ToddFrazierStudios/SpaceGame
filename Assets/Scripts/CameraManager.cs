using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private bool thirdPerson;
	private GameObject currentCamera;
	public GameObject firstPersonCamera;
	public Vector3 firstPersonPosition;
	public LayerMask firstPersonLayers;
	public LayerMask thirdPersonLayers;

    public RadarMount radarMount;
    public OurSmoothFollow sFollow;
    public HeadBob hBob;

	void Start () {
		Cursor.visible = false;
		thirdPerson = false;
        currentCamera = firstPersonCamera;
        radarMount = currentCamera.GetComponent<RadarMount>();
        sFollow = currentCamera.GetComponent<OurSmoothFollow>();
        hBob = currentCamera.GetComponent<HeadBob>();
		firstPersonCamera.GetComponent<Camera>().enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (PlayerInput.PollDigitalControlPressed(0, Controls.CAMERA_BUTTON)) {
			thirdPerson = !thirdPerson;
			if (thirdPerson) {
				GetComponent<RadarMount>().enabled = false;
				currentCamera.transform.parent = null;
				sFollow.enabled = true;
                hBob.enabled = false;
				currentCamera.GetComponent<Camera>().cullingMask = thirdPersonLayers;
			} else {
				GetComponent<RadarMount>().enabled = true;
				currentCamera.transform.parent = gameObject.transform;
				sFollow.enabled = false;
                hBob.enabled = true;
				currentCamera.transform.localPosition = firstPersonPosition;
				currentCamera.transform.localRotation = Quaternion.identity;
				currentCamera.GetComponent<Camera>().cullingMask = firstPersonLayers;
			}
		}

//		if (ParsedInput.controller[0].RSDown || ParsedInput.controller[0].RSUp) {
//			currentCamera.transform.Rotate (Vector3.up, 180f, Space.Self);
//		}
	}
}
