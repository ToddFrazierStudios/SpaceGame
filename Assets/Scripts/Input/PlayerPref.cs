using System;
using UnityEngine;
using System.Collections.Generic;
public class PlayerPref
{
	private string[] bindings = new string[(int)Controls.NUMBER_OF_CONTROLS];

	private Controller controller;
	private int playerNumber;

	public PlayerPref(int playerNumber){
		this.playerNumber = playerNumber;
		controller = null;
	}

	public bool HasController(){
		return controller!=null;
	}

    public InputUtils.Implementations GetControllerImplementation(){
        if (controller == null) return InputUtils.Implementations.NONE;
		return controller.GetControllerImplementation();
	}

	public void ChangeController(Controller newController){
		if(controller!=null){
			saveToPrefs();
		}
		controller = newController;
		if(controller!=null)
			loadFromPrefs();
			writeBindingsToController();
	}

	public void RebindControl(Controls c, string newBind){
		if(controller==null)return;
		bindings[(int)c] = newBind;
		controller.SetBindingForControl(c,newBind);
		saveToPrefs();
	}

	public string GetBindingsForControl(Controls c){
		return bindings[(int)c];
	}

	public float GetAnalogControl(Controls c){
		if(controller==null)return 0.0f;
		return controller.GetAnalogControl(c);
	}

	public bool GetDigitalControl(Controls c){
		if(controller==null)return false;
		return controller.GetDigitalControl(c);
	}
	public bool GetDigitalControlPressed(Controls c){
		if(controller==null)return false;
		return controller.GetDigitalControlPressed(c);
	}
	public string GetControllerDescription(){
		if(controller==null)return "None";
		return controller.GetControllerDescription();
	}
	public int GetControllerNumber(){
		if(controller==null)return -2;
		return controller.GetControllerNumber();
	}
	public void SetVibration(float left, float right){
		if(controller==null)return;
		controller.SetVibration(left,right);
	}

	private void writeBindingsToController(){
		if(controller==null)return;
		for(int i = 0; i<(int)Controls.NUMBER_OF_CONTROLS; i++){
			controller.SetBindingForControl((Controls)i,bindings[i]);
		}
	}

	private void saveToPrefs(){
		string controllerType = "controller";
		if(controller.GetControllerType()==InputUtils.ControllerType.KEYBOARD) controllerType = "keyboard";
		for(int i = 0; i< (int)Controls.NUMBER_OF_CONTROLS; i++){
			PlayerPrefs.SetString("InputBindings.player"+playerNumber+"."+controllerType+":"+Enum.GetName(typeof(Controls),(Controls)i),bindings[i]);
		}
		PlayerPrefs.Save();
	}

	private void loadFromPrefs(){
        bool isKeyboard = controller.GetControllerType() == InputUtils.ControllerType.KEYBOARD;
		for(int i = 0; i< (int)Controls.NUMBER_OF_CONTROLS; i++){
			if(isKeyboard){
				bindings[i] = PlayerPrefs.GetString("InputBindings.player"+playerNumber+".keyboard:"+Enum.GetName(typeof(Controls),(Controls)i),defaultKeyboardBindings[i]);
			}else{
				bindings[i] = PlayerPrefs.GetString("InputBindings.player"+playerNumber+".controller:"+Enum.GetName(typeof(Controls),(Controls)i),defaultControllerBindings[i]);
			}
		}
	}

	public void ResetBindingsToDefault(){
        if (controller.GetControllerType() == InputUtils.ControllerType.KEYBOARD) {
			Array.Copy(defaultKeyboardBindings,bindings,(long)Controls.NUMBER_OF_CONTROLS);
		}else{
			Array.Copy(defaultControllerBindings,bindings,(long)Controls.NUMBER_OF_CONTROLS);
		}
		writeBindingsToController();
	}

	private static string[] defaultControllerBindings,defaultKeyboardBindings;

	static PlayerPref(){
		defaultControllerBindings = new string[(int)Controls.NUMBER_OF_CONTROLS];
		defaultKeyboardBindings = new string[(int)Controls.NUMBER_OF_CONTROLS];
		for(int i = 0; i<(int)Controls.NUMBER_OF_CONTROLS;i++){
			defaultControllerBindings[i] = "NOBIND";
			defaultKeyboardBindings[i] = "NOBIND";
		}
		defaultControllerBindings[(int)Controls.LOOK_X] = "3ThumbSticks.Left.X";
		defaultControllerBindings[(int)Controls.LOOK_Y] = "3ThumbSticks.Left.Y";
		defaultControllerBindings[(int)Controls.STRAFE_X] = "3ThumbSticks.Right.X";
		defaultControllerBindings[(int)Controls.STRAFE_Y] = "3ThumbSticks.Right.Y";
		defaultControllerBindings[(int)Controls.ROLL] = "5Buttons.Y;4Buttons.X;5Buttons.RightShoulder;4Buttons.LeftShoulder";
		defaultControllerBindings[(int)Controls.THROTTLE] = "3Triggers.Left";
		defaultControllerBindings[(int)Controls.FIRE] = "1Triggers.Right";
		defaultControllerBindings[(int)Controls.ALT_FIRE] = "0Buttons.B";
		defaultControllerBindings[(int)Controls.DAMPENERS] = "0Buttons.LeftStick";
		defaultControllerBindings[(int)Controls.BOOST] = "0Buttons.RightStick";
		defaultControllerBindings[(int)Controls.RADAR_BUTTON] = "0DPad.Down";
		defaultControllerBindings[(int)Controls.CAMERA_BUTTON] = "0Buttons.Back";
		defaultControllerBindings[(int)Controls.PAUSE_BUTTON] = "0Buttons.Start";
  		defaultControllerBindings[(int)Controls.TARGET_BUTTON] = "0Buttons.A";
		defaultControllerBindings[(int)Controls.NEXT_WEAPON] = "0DPad.Up";

		defaultKeyboardBindings[(int)Controls.LOOK_X] = "3MouseAbs X";
		defaultKeyboardBindings[(int)Controls.LOOK_Y] = "3MouseAbs Y";
		defaultKeyboardBindings[(int)Controls.STRAFE_X] = "5a;4d";
		defaultKeyboardBindings[(int)Controls.STRAFE_Y] = "5s;4w";
		defaultKeyboardBindings[(int)Controls.ROLL] = "5e;4q";
		defaultKeyboardBindings[(int)Controls.THROTTLE] = "4left shift";
		defaultKeyboardBindings[(int)Controls.FIRE] = "0mouse 0";
		defaultKeyboardBindings[(int)Controls.ALT_FIRE] = "0mouse 1";
		defaultKeyboardBindings[(int)Controls.DAMPENERS] = "0f";
		defaultKeyboardBindings[(int)Controls.BOOST] = "0space";
		defaultKeyboardBindings[(int)Controls.RADAR_BUTTON] = "0r";
		defaultKeyboardBindings[(int)Controls.CAMERA_BUTTON] = "0tab";
		defaultKeyboardBindings[(int)Controls.PAUSE_BUTTON] = "0escape";
		defaultKeyboardBindings[(int)Controls.TARGET_BUTTON] = "0mouse 2";
		defaultKeyboardBindings[(int)Controls.NEXT_WEAPON] = "1Mouse Wheel";
		defaultKeyboardBindings[(int)Controls.PREVIOUS_WEAPON] = "2Mouse Wheel";
		defaultKeyboardBindings[(int)Controls.SELECT_WEAPON_1] = "01";
		defaultKeyboardBindings[(int)Controls.SELECT_WEAPON_2] = "02";
		defaultKeyboardBindings[(int)Controls.SELECT_WEAPON_3] = "03";
		defaultKeyboardBindings[(int)Controls.SELECT_WEAPON_4] = "04";
	}
}

