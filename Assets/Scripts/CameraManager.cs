using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private bool thirdPerson;
	private GameObject currentCamera;
	public GameObject firstPersonCamera;
	public Vector3 firstPersonPosition;
	public LayerMask firstPersonLayers;
	public LayerMask thirdPersonLayers;
    public Transform cameraParent;

    public Transform radar;
    private Vector3 radarPosition;
    private Quaternion radarRotation;
    public Transform health;
    private Vector3 healthPosition;
    private Quaternion healthRotation;

    public RadarMount radarMount;
    public OurSmoothFollow sFollow;
    public HeadBob hBob;

	void Start () {
        
		Cursor.visible = false;
        radarPosition = radar.localPosition;
        radarRotation = radar.localRotation;
        healthPosition = health.localPosition;
        healthRotation = health.localRotation;
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
                currentCamera.transform.parent = null;
                radar.parent = currentCamera.transform;
                health.parent = currentCamera.transform;
				GetComponent<RadarMount>().enabled = false;
				sFollow.enabled = true;
                hBob.enabled = false;
				currentCamera.GetComponent<Camera>().cullingMask = thirdPersonLayers;
			} else {
                GetComponent<RadarMount>().enabled = true;
                radar.parent = transform;
                radar.localPosition = radarPosition;
                radar.localRotation = radarRotation;
                health.parent = transform;
                health.localPosition = healthPosition;
                health.localRotation = healthRotation;
                currentCamera.transform.parent = cameraParent;
				sFollow.enabled = false;
                hBob.enabled = true;
				currentCamera.transform.localPosition = Vector3.zero;
				currentCamera.transform.localRotation = Quaternion.identity;
				currentCamera.GetComponent<Camera>().cullingMask = firstPersonLayers;
			}
		}

//		if (ParsedInput.controller[0].RSDown || ParsedInput.controller[0].RSUp) {
//			currentCamera.transform.Rotate (Vector3.up, 180f, Space.Self);
//		}
	}
}
