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
		if(!cameraToPosition.enabled){//super hack time!
			Debug.LogWarning("Radar camera is disabled! searching for child...");
			Camera newCam = null;
			foreach(Transform child in cameraToPosition.transform){//this code
				newCam = cameraToPosition.transform.GetChild(0).GetComponent<Camera>();
				if(newCam)break;
			}
			if(newCam && newCam.enabled){
				Debug.LogWarning("Radar camera changed from "+cameraToPosition.name+" to its child, "+newCam.name+".");
				cameraToPosition = newCam;
			}else{
				Debug.LogWarning("No valid child camera found!");
			}
		}

		Vector3 topRightScreenPoint = mainCamera.WorldToViewportPoint(topRight.position);
		Vector3 bottomLeftScreenPoint = mainCamera.WorldToViewportPoint(bottomLeft.position);
		float x = bottomLeftScreenPoint.x;
		float y = bottomLeftScreenPoint.y;
		float width = topRightScreenPoint.x-x;
		float height = topRightScreenPoint.y-y;

		Rect newView = new Rect(x,y,width,height);

		DebugHUD.setValue("Radar Rect",newView);

		cameraToPosition.rect = newView;
	}
}
