using UnityEngine;
using System.Collections;
using System;

public class KeyboardController : Controller {
	private Binding[] bindings = new Binding[(int)Controls.NUMBER_OF_CONTROLS];
	private const float MOUSE_ABS_DEADZONE = 0.1f;

	public KeyboardController(){
		ResetBindingsToDefault();
	}

	public override float GetAnalogControl (Controls c){
		Binding bind = bindings[(int)c];
		float val = 0.0f;
		while(bind!=null){
			val+=Mathf.Clamp(pollAnalog(bind), -1.0f, 1.0f);
			bind = bind.AlternateBinding;
		}
		return Mathf.Clamp(val, -1.0f, 1.0f);
	}

	public override void SetVibration (float left, float right){
		//NOP
	}
	
	public override bool GetDigitalControl (Controls c){
		return getDigitalFromBind (c,false);
	}
	
	public override bool GetDigitalControlPressed (Controls c){
		return getDigitalFromBind(c,true);
	}

	private bool getDigitalFromBind(Controls control, bool down){
		Binding bind = bindings[(int) control];
		while(bind!=null){
			if(pollDigital(bind,down))return true;
			bind = bind.AlternateBinding;
		}
		return false;
	}
	
	public override string GetControllerDescription (){
		return "Keyboard/Mouse";
	}
    public override InputUtils.Implementations GetControllerImplementation()
	{
        return InputUtils.Implementations.KEYBOARD_CONTROLLER;
	}
	public override int GetControllerNumber ()
	{
		return -1;
	}

	public override void SetBindingForControl (Controls control, string newBinding){
		if(newBinding=="NOBIND"){
			bindings[(int)control] = null;
		} else {
			Binding b = null;
			string[] alternates = newBinding.Split (BIND_SEPERATOR,System.StringSplitOptions.RemoveEmptyEntries);
			foreach(string s in alternates){
				b = buildBinding(s,b);//each new binding is built and linked to the previous one
			}
			bindings[(int)control] = b;
		}
	}
	//Builds a Binding based on the raw single bind string
	private Binding buildBinding(string bind, Binding previous){
		int meta = System.Int32.Parse(bind.Substring(0,1),System.Globalization.NumberStyles.AllowHexSpecifier);
		bool inverted = (meta & 8) != 0;//isolate the inversion
		meta = meta & 7;//strip out the inversion bit
		return new Binding(bind.Substring(1),(Binding.BindType)meta,previous,inverted,false);
	}

	public override void ResetBindingsToDefault (){
		SetBindingForControl(Controls.LOOK_X, "3Mouse X");
		SetBindingForControl(Controls.LOOK_Y, "3Mouse Y");
		SetBindingForControl(Controls.STRAFE_X, "5a;4d");
		SetBindingForControl(Controls.STRAFE_Y, "5s;4w");
		SetBindingForControl(Controls.ROLL, "5e;4q");
		SetBindingForControl(Controls.THROTTLE, "4left shift");
		SetBindingForControl(Controls.FIRE, "0mouse 0");
		SetBindingForControl(Controls.ALT_FIRE, "0mouse 1");
		SetBindingForControl(Controls.DAMPENERS, "0f");
		SetBindingForControl(Controls.BOOST, "0space");
		SetBindingForControl(Controls.RADAR_BUTTON, "0r");
		SetBindingForControl(Controls.CAMERA_BUTTON, "0tab");
		SetBindingForControl(Controls.PAUSE_BUTTON, "0escape");
		SetBindingForControl(Controls.TARGET_BUTTON, "0mouse 2");
		SetBindingForControl(Controls.NEXT_WEAPON, "1Mouse Wheel");
		SetBindingForControl(Controls.PREVIOUS_WEAPON, "2Mouse Wheel");
		SetBindingForControl(Controls.SELECT_WEAPON_1, "01");
		SetBindingForControl(Controls.SELECT_WEAPON_2, "02");
		SetBindingForControl(Controls.SELECT_WEAPON_3, "03");
		SetBindingForControl(Controls.SELECT_WEAPON_4, "04");
	}

    public override InputUtils.ControllerType GetControllerType()
	{
        return InputUtils.ControllerType.KEYBOARD;
	}

	private float pollAnalog(Binding bind){
		float value = 0.0f;
		switch(bind.Type){
		case Binding.BindType.DIRECT_ANALOG:
			if(bind.BindString=="MouseAbs X"){
				float mouseX = Input.mousePosition.x;
				mouseX = (mouseX-(Screen.width/2));
				value = mouseX/(Screen.width/2);
				if(value<MOUSE_ABS_DEADZONE && value>-MOUSE_ABS_DEADZONE) value = 0f;
			}else if(bind.BindString=="MouseAbs Y"){
				float mouseY = Input.mousePosition.y;
				mouseY = (mouseY-(Screen.height/2));
				value = mouseY/(Screen.height/2);
				if(value<MOUSE_ABS_DEADZONE && value>-MOUSE_ABS_DEADZONE) value = 0f;
			}else{
				value = Input.GetAxis(bind.BindString);
			}
			break;
		case Binding.BindType.DIGITAL_TO_ANALOG_NEGATIVE:
			if(Input.GetKey(bind.BindString))
				value = -1.0f;
			else 
				value = 0.0f;
			break;
		case Binding.BindType.DIGITAL_TO_ANALOG_POSITIVE:
			if(Input.GetKey(bind.BindString))
				value = 1.0f;
			else 
				value = 0.0f;
			break;
		default:
			Debug.LogError("Invalid Binding! Bind "+bind.BindString+" was polled as an analog bind, but it is type "+bind.Type);
			return 0.0f;
		}
		if(bind.IsATrigger && bind.IsInverted)return 1.0f - value;
		if(bind.IsInverted) return -value;
		return value;
	}

	private bool pollDigital(Binding bind, bool down){
		bool value = false;
		switch(bind.Type){
		case Binding.BindType.DIRECT_DIGIAL:
			if(down)
				value = Input.GetKeyDown(bind.BindString);
			else 
				value = Input.GetKey(bind.BindString);
			break;
		case Binding.BindType.ANALOG_TO_DIGITAL_NEGATIVE:
			value = Input.GetAxis(bind.BindString) <= -Controller.ANALOG_DIGITAL_THRESHOLD;
			break;
		case Binding.BindType.ANALOG_TO_DIGITAL_POSITIVE:
			value = Input.GetAxis(bind.BindString) >= Controller.ANALOG_DIGITAL_THRESHOLD;
			break;
		default:
			Debug.LogError("Invalid Binding! Bind "+bind.BindString+" was polled as a analog bind, but it is type "+bind.Type);
			return false;
		}
		if(bind.IsInverted)return !value;
		return value;
	}
	private static float absMax(float a, float b){
		if(Mathf.Abs(a)>Mathf.Abs(b))return a;
		return b;
	}
}
