using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnityController : Controller {
	private static Dictionary<string,string> platformLookupTable;
	private static string rightThumbStickYAxisName;
	private Binding[] bindings = new Binding[(int)Controls.NUMBER_OF_CONTROLS];
	private int controllerNumber;
	private string prefix;

	#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
	private bool rightTriggerUsed = false;
	private bool leftTriggerUsed = false;
	private static string leftTriggerAxis,rightTriggerAxis;
	#endif


	public UnityController(int controllerNumber){
		this.controllerNumber = controllerNumber;
		prefix = "joystick "+(controllerNumber+1)+" ";
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

	public override bool GetDigitalControl (Controls c)
	{
		return getDigitalFromBind(c,false);
	}

	public override bool GetDigitalControlPressed (Controls c)
	{
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

	public override string GetControllerDescription ()
	{
		return "Unity Controller";
	}

	public override void SetVibration (float left, float right)
	{
		//NOP
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
		string bindString = bind.Substring(1);

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
		//special fixes for windows where the DPad is an axis instead of buttons
		if(bindString.Contains("DPad")){
			switch((Binding.BindType)meta){
			case Binding.BindType.DIRECT_DIGIAL:
				if(bindString.Contains("Up")||bindString.Contains("Right")){
					meta = (int)Binding.BindType.ANALOG_TO_DIGITAL_POSITIVE;
				}else{
					meta = (int)Binding.BindType.ANALOG_TO_DIGITAL_NEGATIVE;
				}
				break;
			case Binding.BindType.DIGITAL_TO_ANALOG_POSITIVE:
				meta = (int)Binding.BindType.DIRECT_ANALOG;
				if(bindString.Contains("Down")||bindString.Contains("Left")){
					inverted = !inverted;
				}
				break;
			case Binding.BindType.DIGITAL_TO_ANALOG_NEGATIVE:
				meta = (int)Binding.BindType.DIRECT_ANALOG;
				if(bindString.Contains("Up")||bindString.Contains("Right")){
					inverted = !inverted;
				}
				break;
			}
		}
#endif

		return new Binding(platformLookupTable[bindString],(Binding.BindType)meta,previous,inverted,bind.Contains("Trigger"));
	}

	public override void ResetBindingsToDefault ()
	{
		SetBindingForControl(Controls.LOOK_X,"3ThumbSticks.Left.X");
		SetBindingForControl(Controls.LOOK_Y,"3ThumbSticks.Left.Y");
		SetBindingForControl(Controls.STRAFE_X,"3ThumbSticks.Right.X");
		SetBindingForControl(Controls.STRAFE_Y,"3ThumbSticks.Right.Y");
		SetBindingForControl(Controls.ROLL,"5Buttons.Y;4Buttons.X;5Buttons.RightShoulder;4Buttons.LeftShoulder");
		SetBindingForControl(Controls.THROTTLE,"3Triggers.Left");
		SetBindingForControl(Controls.FIRE,"1Triggers.Right");
		SetBindingForControl(Controls.ALT_FIRE,"0Buttons.B");
		SetBindingForControl(Controls.DAMPENERS,"0Buttons.LeftStick");
		SetBindingForControl(Controls.BOOST,"0Buttons.RightStick");
		SetBindingForControl(Controls.RADAR_BUTTON,"0DPad.Down");
		SetBindingForControl(Controls.CAMERA_BUTTON,"0Buttons.Back");
		SetBindingForControl(Controls.PAUSE_BUTTON,"0Buttons.Start");
		SetBindingForControl(Controls.TARGET_BUTTON,"0Buttons.A");
		SetBindingForControl(Controls.NEXT_WEAPON,"0DPad.Up");
		SetBindingForControl(Controls.PREVIOUS_WEAPON,"NOBIND");
		SetBindingForControl(Controls.SELECT_WEAPON_1,"NOBIND");
		SetBindingForControl(Controls.SELECT_WEAPON_2,"NOBIND");
		SetBindingForControl(Controls.SELECT_WEAPON_3,"NOBIND");
		SetBindingForControl(Controls.SELECT_WEAPON_4,"NOBIND");
	}

	public override int GetControllerNumber ()
	{
		return controllerNumber;
	}

	private float pollAnalog(Binding bind){
		float value = 0.0f;
		switch(bind.Type){
		case Binding.BindType.DIRECT_ANALOG:
			value = GetAxis(bind.BindString);
			break;
		case Binding.BindType.DIGITAL_TO_ANALOG_NEGATIVE:
			if(Input.GetKey(prefix+bind.BindString))
				value = -1.0f;
			else 
				value = 0.0f;
			break;
		case Binding.BindType.DIGITAL_TO_ANALOG_POSITIVE:
			if(Input.GetKey(prefix+bind.BindString))
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
				value = Input.GetKeyDown(prefix+bind.BindString);
			else 
				value = Input.GetKey(prefix+bind.BindString);
			break;
		case Binding.BindType.ANALOG_TO_DIGITAL_NEGATIVE:
			value = GetAxis(bind.BindString) <= -Controller.ANALOG_DIGITAL_THRESHOLD;
			break;
		case Binding.BindType.ANALOG_TO_DIGITAL_POSITIVE:
			value = GetAxis(bind.BindString) >= Controller.ANALOG_DIGITAL_THRESHOLD;
			break;
		default:
			Debug.LogError("Invalid Binding! Bind "+bind.BindString+" was polled as a analog bind, but it is type "+bind.Type);
			return false;
		}
		if(bind.IsInverted)return !value;
		return value;
	}
	//Special GetAxis function, replaces Input.GetAxis for this class,
	//includes compensation for the upside-down right thubstick and for macmamac trigger issues
	private float GetAxis(string identifier){
		float value = Input.GetAxis(prefix+identifier);
		if(identifier==rightThumbStickYAxisName)value = -value;
		#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
		DebugHUD.setValue("LeftUsed",leftTriggerUsed);
		DebugHUD.setValue("RightUsed",rightTriggerUsed);
		if(identifier == leftTriggerAxis){
			value = Input.GetAxisRaw(prefix+identifier);
			if(leftTriggerUsed)return (value+1f)/2f;
			if(value!=0.0f)leftTriggerUsed = true;
			return 0.0f;
		}
		if(identifier == rightTriggerAxis){
			value = Input.GetAxisRaw(prefix+identifier);
			if(rightTriggerUsed)return (value+1f)/2f;
			if(value!=0.0f)rightTriggerUsed = true;
			return 0.0f;
		}
		#endif
		return value;
	}

    public override InputUtils.ControllerType GetControllerType()
	{
        return InputUtils.ControllerType.JOYSTICK;
	}

    public override InputUtils.Implementations GetControllerImplementation()
	{
        return InputUtils.Implementations.UNITY_CONTROLLER;
	}

	private static float absMax(float a, float b){
		if(Mathf.Abs(a)>Mathf.Abs(b))return a;
		return b;
	}


	static UnityController(){
		Debug.Log ("init");
		platformLookupTable = new Dictionary<string,string>();
		platformLookupTable.Add("NOBIND","NOBIND");
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
		///WINDOWS!
		//digital
		platformLookupTable.Add("Buttons.A", "button 0");
		platformLookupTable.Add("Buttons.B", "button 1");
		platformLookupTable.Add("Buttons.X", "button 2");
		platformLookupTable.Add("Buttons.Y", "button 3");
		platformLookupTable.Add("Buttons.LeftShoulder", "button 4");
		platformLookupTable.Add("Buttons.RightShoulder", "button 5");
		platformLookupTable.Add("Buttons.Back", "button 6");
		platformLookupTable.Add("Buttons.Start", "button 7");
		platformLookupTable.Add("Buttons.LeftStick", "button 8");
		platformLookupTable.Add("Buttons.RightStick", "button 9");
		//analog
		platformLookupTable.Add("ThumbSticks.Left.X", "axis X");
		platformLookupTable.Add("ThumbSticks.Left.Y", "axis Y");
		platformLookupTable.Add("ThumbSticks.Right.X", "axis 4");
		platformLookupTable.Add("ThumbSticks.Right.Y", "axis 5");
		platformLookupTable.Add("Triggers.Left", "axis 9");
		platformLookupTable.Add("Triggers.Right", "axis 10");
		platformLookupTable.Add("DPad.Up", "axis 7");
		platformLookupTable.Add("DPad.Down", "axis 7");
		platformLookupTable.Add("DPad.Right", "axis 6");
		platformLookupTable.Add("DPad.Left", "axis 6");
#elif (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
		//MACMAMAC!
		//digital
		platformLookupTable.Add("Buttons.A", "button 16");
		platformLookupTable.Add("Buttons.B", "button 17");
		platformLookupTable.Add("Buttons.X", "button 18");
		platformLookupTable.Add("Buttons.Y", "button 19");
		platformLookupTable.Add("Buttons.LeftShoulder", "button 13");
		platformLookupTable.Add("Buttons.RightShoulder", "button 14");
		platformLookupTable.Add("Buttons.Back", "button 10");
		platformLookupTable.Add("Buttons.Start", "button 9");
		platformLookupTable.Add("Buttons.LeftStick", "button 11");
		platformLookupTable.Add("Buttons.RightStick", "button 12");
		platformLookupTable.Add("DPad.Up", "button 5");
		platformLookupTable.Add("DPad.Down", "button 6");
		platformLookupTable.Add("DPad.Left", "button 7");
		platformLookupTable.Add("DPad.Right", "button 8");
		//analog
		platformLookupTable.Add("ThumbSticks.Left.X", "axis X");
		platformLookupTable.Add("ThumbSticks.Left.Y", "axis Y");
		platformLookupTable.Add("ThumbSticks.Right.X", "axis 3");
		platformLookupTable.Add("ThumbSticks.Right.Y", "axis 4");
		platformLookupTable.Add("Triggers.Left", "axis 5");
		platformLookupTable.Add("Triggers.Right", "axis 6");

		rightTriggerAxis = platformLookupTable["Triggers.Right"];
		leftTriggerAxis = platformLookupTable["Triggers.Left"];
#endif
		rightThumbStickYAxisName = platformLookupTable["ThumbSticks.Right.Y"];
	}
}
