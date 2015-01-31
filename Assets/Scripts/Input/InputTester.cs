using UnityEngine;
using System.Collections;
using System;

public class InputTester : MonoBehaviour {	
	public Controller.Implementations controllerType;
	private Controller.Implementations prevType;
	[Range(0,3)]
	public int controllerNumber;
	private int prevControllerNumber;
	[Range(0f,1f)]
	public float leftMotor,rightMotor;

	// Use this for initialization
	void Start () {
		init ();
	}

	private void init(){
		GlobalControllerManager.AssignControllerToPlayer(0,controllerType,controllerNumber);
		prevType = controllerType;
		prevControllerNumber = controllerNumber;
	}
	
	// Update is called once per frame
	void Update () {
//		DebugHUD.setValue("Frame",Time.frameCount);
		if(prevType!=controllerType || prevControllerNumber!=controllerNumber)init();
		PlayerPref controller = GlobalControllerManager.GetPlayer(0);
		DebugHUD.setValue("Controller Type",controller.GetControllerDescription());
		DebugHUD.setValue("Controller Number", controller.GetControllerNumber());
		for(int i = 0; i<(int)Controls.NUMBER_OF_CONTROLS; i++){
			if(Controller.IsADigitalControl((Controls)i)){
				DebugHUD.setValue(Enum.GetName(typeof(Controls),i),controller.GetDigitalControl((Controls)i));
			}else{
				DebugHUD.setValue(Enum.GetName(typeof(Controls),i),controller.GetAnalogControl((Controls)i));
			}
		}
		controller.SetVibration(leftMotor,rightMotor);
	}
}
