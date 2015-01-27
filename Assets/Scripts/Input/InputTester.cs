using UnityEngine;
using System.Collections;
using System;

public class InputTester : MonoBehaviour {

	public enum ControllerTypes{
		KeyboardController,
		UnityController
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		,XInputController
		#endif
	}
	private Controller controller;
	public ControllerTypes controllerType;
	private ControllerTypes prevType;
	[Range(0,3)]
	public int playerNumber;
	private int prevPlayerNumber;
	[Range(0f,1f)]
	public float leftMotor,rightMotor;

	// Use this for initialization
	void Start () {
		init ();
	}

	private void init(){
		prevType = controllerType;
		prevPlayerNumber = playerNumber;
		switch(controllerType){
		case ControllerTypes.KeyboardController: controller = new KeyboardController();break;
			#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		case ControllerTypes.XInputController: controller = new XInputController(playerNumber);break;
			#endif
		case ControllerTypes.UnityController: controller = new UnityController(playerNumber);break;
		}
	}
	
	// Update is called once per frame
	void Update () {
//		DebugHUD.setValue("Frame",Time.frameCount);
		if(prevType!=controllerType || prevPlayerNumber!=playerNumber)init();
		DebugHUD.setValue("Controller Type",controller.GetControllerType());
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
