using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class RadarMount : MonoBehaviour {
	public Transform topRight,bottomLeft;
	public Camera cameraToPosition,mainCamera;
	
	// Update is called once per frame
//	void Update () {
//		if(animation){
//			if(animation.isPlaying){
//				cameraToPosition.enabled = false;
//				return;
//			}else{
//				cameraToPosition.enabled = true;
//			}
//		}
//		Vector3 topRightScreenPoint = mainCamera.WorldToViewportPoint(topRight.position);
//		Vector3 bottomLeftScreenPoint = mainCamera.WorldToViewportPoint(bottomLeft.position);
//		float x = bottomLeftScreenPoint.x;
//		float y = bottomLeftScreenPoint.y;
//		float width = topRightScreenPoint.x-x;
//		float height = topRightScreenPoint.y-y;
//
//		Rect newView = new Rect(x,y,width,height);
//
//		DebugHUD.setValue("Radar Rect",newView);
//
//		foreach(Camera cam in cameraToPosition.GetComponentsInChildren<Camera>())
//			cam.rect = newView;
//	}
}
