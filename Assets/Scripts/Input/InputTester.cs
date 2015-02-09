using UnityEngine;
using System.Collections;
using System;

public class InputTester : MonoBehaviour {
    public InputUtils.Implementations controllerType;
    private InputUtils.Implementations prevType;
	[Range(0,3)]
	public int controllerNumber;
	[Range(0,3)]
	public int playerNumber;
	private int prevPlayerNumber;
	private int prevControllerNumber;
	[Range(0f,1f)]
	public float leftMotor,rightMotor;

	public bool readOnly = false;

	// Use this for initialization
	void Start () {
		if(!readOnly)init ();
	}

	private void init(){
		GlobalControllerManager.AssignControllerToPlayer(playerNumber,controllerType,controllerNumber);
		prevType = controllerType;
		prevControllerNumber = controllerNumber;
		prevPlayerNumber = playerNumber;
	}
	
	// Update is called once per frame
	void Update () {
//		DebugHUD.setValue("Frame",Time.frameCount);
		if(!readOnly && (prevType!=controllerType || prevControllerNumber!=controllerNumber || prevPlayerNumber!=playerNumber))init();
		PlayerPref controller = GlobalControllerManager.GetPlayer(playerNumber);
		DebugHUD.setValue("Controller Type",controller.GetControllerDescription());
		DebugHUD.setValue("Controller Number", controller.GetControllerNumber());
		for(int i = 0; i<(int)Controls.NUMBER_OF_CONTROLS; i++){
            if (InputUtils.IsADigitalControl((Controls)i))
            {
				DebugHUD.setValue(Enum.GetName(typeof(Controls),i),controller.GetDigitalControl((Controls)i));
			}else{
				DebugHUD.setValue(Enum.GetName(typeof(Controls),i),controller.GetAnalogControl((Controls)i));
			}
		}
		if(!readOnly)controller.SetVibration(leftMotor,rightMotor);
		DebugHUD.setValue("Joysticks",String.Join(", ",Input.GetJoystickNames()));
	}
}
