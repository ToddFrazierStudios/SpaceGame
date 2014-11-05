using UnityEngine;
using System.Collections;

public class RadarMount : MonoBehaviour {
	public Transform topRight,bottomLeft;
	public Camera cameraToPosition,mainCamera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 topRightScreenPoint = mainCamera.WorldToViewportPoint(topRight.position);
		Vector3 bottomLeftScreenPoint = mainCamera.WorldToViewportPoint(bottomLeft.position);
		float x = bottomLeftScreenPoint.x;
		float y = bottomLeftScreenPoint.y;
		float width = topRightScreenPoint.x-x;
		float height = topRightScreenPoint.y-y;

		Rect newView = new Rect(x,y,width,height);

		DebugHUD.setValue("Radar Rect",newView);

		foreach(Camera cam in cameraToPosition.GetComponentsInChildren<Camera>())
			cam.rect = newView;
	}
}
