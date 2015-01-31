using System;
using UnityEngine;
public class PlayerPref
{
	private string[] controllerBindings = new string[(int)Controls.NUMBER_OF_CONTROLS];
	private string[] keyboardBindings = new string[(int)Controls.NUMBER_OF_CONTROLS];
	private bool[] inversions = new bool[4];
	private Controller controller;
	private int playerNumber;

	public PlayerPref(int playerNumber){
		this.playerNumber = playerNumber;
		controller = null;
		loadFromPrefs();
	}

	public bool HasController(){
		return controller!=null;
	}


	public void ChangeController(Controller newController){
		if(controller!=null){
			saveToPrefs();
		}
		controller = newController;
		if(controller!=null)
			writeBindingsToController();
	}

	public void RebindControl(Controls c, string newBind){
		if(controller==null)return;
		controller.SetBindingForControl(c,newBind);
		loadBindingsFromController();
		saveToPrefs();
	}

	public float GetAnalogControl(Controls c){
		if(controller==null)return 0.0f;
		float fromController = controller.GetAnalogControl(c);
		//TODO modify value before it is returned!
		return fromController;
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

	private string readBindingFromController(Controls c){
		if(controller==null) return "NOBIND";
		return controller.GetBindingForControl(c);
	}

	private void loadBindingsFromController(){
		if(controller==null)return;
		bool isKeyboard = controller.GetControllerType()==Controller.ControllerType.KEYBOARD;
		for(int i = 0; i<(int)Controls.NUMBER_OF_CONTROLS; i++){
			if(isKeyboard){
				keyboardBindings[i] = readBindingFromController((Controls)i);
			}else{
				controllerBindings[i] = readBindingFromController((Controls)i);
			}
		}
	}

	private void writeBindingsToController(){
		if(controller==null)return;
		bool isKeyboard = controller.GetControllerType()==Controller.ControllerType.KEYBOARD;
		for(int i = 0; i<(int)Controls.NUMBER_OF_CONTROLS; i++){
			if(isKeyboard){
				controller.SetBindingForControl((Controls)i,keyboardBindings[i]);
			}else{
				controller.SetBindingForControl((Controls)i,controllerBindings[i]);
			}
		}
	}

	private void saveToPrefs(){
		for(int i = 0; i< (int)Controls.NUMBER_OF_CONTROLS; i++){
			PlayerPrefs.SetString("InputBindings.player"+playerNumber+".controller:"+Enum.GetName(typeof(Controls),(Controls)i),controllerBindings[i]);
			PlayerPrefs.SetString("InputBindings.player"+playerNumber+".keyboard:"+Enum.GetName(typeof(Controls),(Controls)i),keyboardBindings[i]);
		}
		PlayerPrefs.Save();
	}

	private void loadFromPrefs(){
		for(int i = 0; i< (int)Controls.NUMBER_OF_CONTROLS; i++){
			controllerBindings[i] = PlayerPrefs.GetString("InputBindins.player"+playerNumber+".controller:"+Enum.GetName(typeof(Controls),(Controls)i),defaultControllerBindings[i]);
			keyboardBindings[i] = PlayerPrefs.GetString("InputBindins.player"+playerNumber+".keyboard:"+Enum.GetName(typeof(Controls),(Controls)i),defaultKeyboardBindings[i]);
		}
	}

	public void ResetKeyboardBindingsToDefault(){
		Array.Copy(defaultKeyboardBindings,keyboardBindings,(long)Controls.NUMBER_OF_CONTROLS);
		writeBindingsToController();
	}
	public void ResetControllerBindingsToDefault(){
		Array.Copy(defaultControllerBindings,controllerBindings,(long)Controls.NUMBER_OF_CONTROLS);
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
		defaultControllerBindings[(int)Controls.LOOK_X] = "ThumbSticks.Left.X";
		defaultControllerBindings[(int)Controls.LOOK_Y] = "ThumbSticks.Left.Y";
		defaultControllerBindings[(int)Controls.STRAFE_X] = "ThumbSticks.Right.X";
		defaultControllerBindings[(int)Controls.STRAFE_Y] = "ThumbSticks.Right.Y";
		defaultControllerBindings[(int)Controls.ROLL] = "Buttons.Y_Buttons.X;Buttons.RightShoulder_Buttons.LeftShoulder";
		defaultControllerBindings[(int)Controls.THROTTLE] = "Triggers.Left";
		defaultControllerBindings[(int)Controls.FIRE] = "Triggers.Right";
		defaultControllerBindings[(int)Controls.ALT_FIRE] = "Buttons.B";
		defaultControllerBindings[(int)Controls.DAMPENERS] = "Buttons.LeftStick";
		defaultControllerBindings[(int)Controls.BOOST] = "Buttons.RightStick";
		defaultControllerBindings[(int)Controls.RADAR_BUTTON] = "DPad.Down";
		defaultControllerBindings[(int)Controls.CAMERA_BUTTON] = "Buttons.Back";
		defaultControllerBindings[(int)Controls.PAUSE_BUTTON] = "Buttons.Start";
  		defaultControllerBindings[(int)Controls.TARGET_BUTTON] = "Buttons.A";
		defaultControllerBindings[(int)Controls.NEXT_WEAPON] = "DPad.Up";

		defaultKeyboardBindings[(int)Controls.LOOK_X] = "Mouse X";
		defaultKeyboardBindings[(int)Controls.LOOK_Y] = "Mouse Y";
		defaultKeyboardBindings[(int)Controls.STRAFE_X] = "a_d";
		defaultKeyboardBindings[(int)Controls.STRAFE_Y] = "s_w";
		defaultKeyboardBindings[(int)Controls.ROLL] = "e_q";
		defaultKeyboardBindings[(int)Controls.THROTTLE] = "left shift";
		defaultKeyboardBindings[(int)Controls.FIRE] = "mouse 0";
		defaultKeyboardBindings[(int)Controls.ALT_FIRE] = "mouse 1";
		defaultKeyboardBindings[(int)Controls.DAMPENERS] = "f";
		defaultKeyboardBindings[(int)Controls.BOOST] = "space";
		defaultKeyboardBindings[(int)Controls.RADAR_BUTTON] = "r";
		defaultKeyboardBindings[(int)Controls.CAMERA_BUTTON] = "tab";
		defaultKeyboardBindings[(int)Controls.PAUSE_BUTTON] = "escape";
		defaultKeyboardBindings[(int)Controls.TARGET_BUTTON] = "mouse 2";
		defaultKeyboardBindings[(int)Controls.NEXT_WEAPON] = "Mouse Wheel Up";
		defaultKeyboardBindings[(int)Controls.PREVIOUS_WEAPON] = "Mouse Wheel Down";
		defaultKeyboardBindings[(int)Controls.SELECT_WEAPON_1] = "1";
		defaultKeyboardBindings[(int)Controls.SELECT_WEAPON_2] = "2";
		defaultKeyboardBindings[(int)Controls.SELECT_WEAPON_3] = "3";
		defaultKeyboardBindings[(int)Controls.SELECT_WEAPON_4] = "4";
	}
}

