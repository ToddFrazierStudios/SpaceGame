using UnityEngine;
using System.Collections;

public class KeyboardController : Controller {
	private string[] bindings = new string[(int)Controls.NUMBER_OF_CONTROLS];

	public KeyboardController(){
		ResetBindingsToDefault();
	}

	public override float GetAnalogControl (Controls c){
		string[] binds = bindings[(int)c].Split(';');
		float val = 0.0f;
		foreach (string s in binds){
			val = absMax(val, pollAnalog(s));
		}
		return val;
	}

	public override void SetVibration (float left, float right){
		//NOP
	}

	public override string GetBindingForControl (Controls control){
		return bindings[(int)control];
	}
	public override bool GetDigitalControl (Controls c){
		return getDigitalFromBind (c,false);
	}
	
	public override bool GetDigitalControlPressed (Controls c){
		return getDigitalFromBind(c,true);
	}

	private bool getDigitalFromBind(Controls control, bool down){
		string[] binds = bindings[(int) control].Split(';');
		foreach(string bind in binds){
			if(pollDigital(bind,down))return true;
		}
		return false;
	}
	
	public override string GetControllerType (){
		return "Keyboard/Mouse";
	}
	public override int GetControllerNumber ()
	{
		return -1;
	}

	public override void SetBindingForControl (Controls control, string newBinding){
		bindings[(int)control] = newBinding;
	}

	public override void ResetBindingsToDefault (){
		bindings[(int)Controls.LOOK_X] = "Mouse X";
		bindings[(int)Controls.LOOK_Y] = "Mouse Y";
		bindings[(int)Controls.STRAFE_X] = "a_d";
		bindings[(int)Controls.STRAFE_Y] = "s_w";
		bindings[(int)Controls.ROLL] = "e_q";
		bindings[(int)Controls.THROTTLE] = "left shift";
		bindings[(int)Controls.FIRE] = "mouse 0";
		bindings[(int)Controls.ALT_FIRE] = "mouse 1";
		bindings[(int)Controls.DAMPENERS] = "f";
		bindings[(int)Controls.BOOST] = "space";
		bindings[(int)Controls.RADAR_BUTTON] = "r";
		bindings[(int)Controls.CAMERA_BUTTON] = "tab";
		bindings[(int)Controls.PAUSE_BUTTON] = "escape";
		bindings[(int)Controls.TARGET_BUTTON] = "mouse 2";
		bindings[(int)Controls.NEXT_WEAPON] = "Mouse Wheel Up";
		bindings[(int)Controls.PREVIOUS_WEAPON] = "Mouse Wheel Down";
		bindings[(int)Controls.SELECT_WEAPON_1] = "1";
		bindings[(int)Controls.SELECT_WEAPON_2] = "2";
		bindings[(int)Controls.SELECT_WEAPON_3] = "3";
		bindings[(int)Controls.SELECT_WEAPON_4] = "4";
	}

	private float pollAnalog(string identifier){
		if(identifier=="NOBIND")return 0.0f;
 		if (identifier == "Mouse X" || identifier == "Mouse Y"){
			return Input.GetAxis(identifier);
		}else{
			if(identifier.Contains ("_")){
				string[] split = identifier.Split('_');
				string minusButton = split[0];
				string plusButton = split[1];
				float val = 0.0f;
				if(pollDigital(minusButton,false)) val-=1.0f;
				if(pollDigital(plusButton,false)) val+=1.0f;
				return val;
			}else{
				if(pollDigital(identifier,false))
					return 1.0f;
				return 0.0f;
			}
		}
	}

	private bool pollDigital(string identifier, bool down){
		if(identifier=="NOBIND")return false;
		if(identifier == "Mouse Wheel Up"){
			return Input.GetAxis("Mouse Wheel") > 0.0f;
		}
		if(identifier == "Mouse Wheel Down"){
			return Input.GetAxis("Mouse Wheel") < 0.0f;
		}
		if(down){
			return Input.GetKeyDown(identifier);
		}else{
			return Input.GetKey(identifier);
		}
	}
	private static float absMax(float a, float b){
		if(Mathf.Abs(a)>Mathf.Abs(b))return a;
		return b;
	}
}
