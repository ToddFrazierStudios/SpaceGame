using UnityEngine;
using System.Collections;

public class AutoLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 curRot = transform.localEulerAngles;
		DebugHUD.setValue("Angular Velocity",curRot);
		float zRot = curRot.z;
		if(zRot>180f)zRot-=360f;
		if(Mathf.Abs (zRot)>85f){
			DebugHUD.setValue("Upside Down","<color=red>TRUE</color>");
			zRot = 0;
		}else{
			DebugHUD.setValue("Upside Down","<color=green>FALSE</color>");
		}
		DebugHUD.setValue("zRot",zRot);
		rigidbody.AddTorque(new Vector3(0f,0f,-zRot*zRot*zRot));
	}
}
