using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnityController : Controller {
	private static Dictionary<string,string> platformLookupTable;
	private string[] bindings = new string[(int)Controls.NUMBER_OF_CONTROLS];
	private string[] originalBindings = new string[(int) Controls.NUMBER_OF_CONTROLS];
	private int controllerNumber;
	private string prefix;

	public UnityController(int controllerNumber){
		this.controllerNumber = controllerNumber;
		ResetBindingsToDefault();
		prefix = "joystick "+(controllerNumber+1)+" ";
	}

	public override float GetAnalogControl (Controls c)
	{
		string[] binds = bindings[(int)c].Split(';');
		float val = 0.0f;
		foreach (string s in binds){
			val = absMax(val, lookupAnalog(s));
		}
		return val;
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
		string[] binds = bindings[(int) control].Split(';');
		foreach(string bind in binds){
			if(lookupDigital(bind,down))return true;
		}
		return false;
	}

	public override string GetControllerType ()
	{
		return "Unity Controller";
	}

	public override void SetVibration (float left, float right)
	{
		//NOP
	}

	public override string GetBindingForControl (Controls control)
	{
		return originalBindings[(int)control];
	}

	public override void SetBindingForControl (Controls control, string newBinding)
	{
		Debug.Log ("Binding "+newBinding+" to control "+Enum.GetName(typeof(Controls),control));
		originalBindings[(int)control] = newBinding;//for saving and stuff
		string[] binds = newBinding.Split(';');
		for(int i = 0; i<binds.Length;i++){
			binds[i] = translateBind(binds[i]);
		}
		bindings[(int)control] = string.Join(";",binds);

	}

	private string translateBind(string bind){
		Debug.Log("Translating bind "+bind);
		if(bind.Contains("_")){
			string[] split = bind.Split('_');
			return platformLookupTable[split[0]]+"_"+platformLookupTable[split[1]];
		}else{
			return platformLookupTable[bind];
		}
	}

	public override void ResetBindingsToDefault ()
	{
		SetBindingForControl(Controls.LOOK_X,"ThumbSticks.Left.X");
		SetBindingForControl(Controls.LOOK_Y,"ThumbSticks.Left.Y");
		SetBindingForControl(Controls.STRAFE_X,"ThumbSticks.Right.X");
		SetBindingForControl(Controls.STRAFE_Y,"ThumbSticks.Right.Y");
		SetBindingForControl(Controls.ROLL,"Buttons.Y_Buttons.X;Buttons.RightShoulder_Buttons.LeftShoulder");
		SetBindingForControl(Controls.THROTTLE,"Triggers.Left");
		SetBindingForControl(Controls.FIRE,"Triggers.Right");
		SetBindingForControl(Controls.ALT_FIRE,"Buttons.B");
		SetBindingForControl(Controls.DAMPENERS,"Buttons.LeftStick");
		SetBindingForControl(Controls.BOOST,"Buttons.RightStick");
		SetBindingForControl(Controls.RADAR_BUTTON,"DPad.Down");
		SetBindingForControl(Controls.CAMERA_BUTTON,"Buttons.Back");
		SetBindingForControl(Controls.PAUSE_BUTTON,"Buttons.Start");
		SetBindingForControl(Controls.TARGET_BUTTON,"Buttons.A");
		SetBindingForControl(Controls.NEXT_WEAPON,"DPad.Up");
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

	private float lookupAnalog(string identifier){
		if(identifier=="NOBIND")return 0.0f;
		if(identifier.Contains("axis")){
			return Input.GetAxis(prefix+identifier);
		}else if(identifier.Contains("_")){
			string[] split = identifier.Split('_');
			string minusButton = split[0];
			string plusButton = split[1];
			float val = 0.0f;
			if(lookupDigital(minusButton,false)) val-=1.0f;
			if(lookupDigital(plusButton,false)) val+=1.0f;
			return val;
		}else{
			if(lookupDigital(identifier,false))
				return 1.0f;
			return 0.0f;
		}
	}

	private bool lookupDigital(string identifier, bool down){
		if(identifier=="NOBIND")return false;
		if(identifier.Contains("button")){
			if(down)return Input.GetKeyDown(prefix+identifier);
			return Input.GetKey(prefix+identifier);
		}else if(identifier.EndsWith("+")){
			return Input.GetAxis(prefix+identifier.Remove(identifier.Length-1))>=Controller.ANALOG_DIGITAL_THRESHOLD;
		}else if(identifier.EndsWith("-")){
			return Input.GetAxis(prefix+identifier.Remove(identifier.Length-1))<=-Controller.ANALOG_DIGITAL_THRESHOLD;
		}else{
			return Input.GetAxis(prefix+identifier)>=Controller.ANALOG_DIGITAL_THRESHOLD;
		}
	}

	private static float absMax(float a, float b){
		if(Mathf.Abs(a)>Mathf.Abs(b))return a;
		return b;
	}


	static UnityController(){
		Debug.Log ("init");
		platformLookupTable = new Dictionary<string,string>();
		platformLookupTable.Add("NOBIND","NOBIND");
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		Debug.Log ("Loading Windows platformLookupTable...");
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
		platformLookupTable.Add("DPad.Up", "axis 7+");
		platformLookupTable.Add("DPad.Down", "axis 7-");
		platformLookupTable.Add("DPad.Right", "axis 6+");
		platformLookupTable.Add("DPad.Left", "axis 6-");
		//analog-to-digital
		platformLookupTable.Add("ThumbSticks.Left.X+", "axis X+");
		platformLookupTable.Add("ThumbSticks.Left.Y+", "axis Y+");
		platformLookupTable.Add("ThumbSticks.Right.X+", "axis 5+");
		platformLookupTable.Add("ThumbSticks.Right.Y+", "axis 6+");
		platformLookupTable.Add("ThumbSticks.Left.X-", "axis X-");
		platformLookupTable.Add("ThumbSticks.Left.Y-", "axis Y-");
		platformLookupTable.Add("ThumbSticks.Right.X-", "axis 5-");
		platformLookupTable.Add("ThumbSticks.Right.Y-", "axis 6-");
#elif UNITY_STANDALONE_OSX | UNITY_EDITOR_OSX
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
		platformLookupTable.Add("Thumbsticks.Left.X", "axis X");
		platformLookupTable.Add("Thumbsticks.Left.Y", "axis Y");
		platformLookupTable.Add("Thumbsticks.Right.X", "axis 3");
		platformLookupTable.Add("Thumbsticks.Right.Y", "axis 4");
		platformLookupTable.Add("Triggers.Left", "axis 5");
		platformLookupTable.Add("Triggers.Right", "axis 6");
		//analog-to-digital
		platformLookupTable.Add("Thumbsticks.Left.X+", "axis X+");
		platformLookupTable.Add("Thumbsticks.Left.Y+", "axis Y+");
		platformLookupTable.Add("Thumbsticks.Right.X+", "axis 3+");
		platformLookupTable.Add("Thumbsticks.Right.Y+", "axis 4+");
		platformLookupTable.Add("Thumbsticks.Left.X-", "axis X-");
		platformLookupTable.Add("Thumbsticks.Left.Y-", "axis Y-");
		platformLookupTable.Add("Thumbsticks.Right.X-", "axis 3-");
		platformLookupTable.Add("Thumbsticks.Right.Y-", "axis 4-");
#endif
	}
}
