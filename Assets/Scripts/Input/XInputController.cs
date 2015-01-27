#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class XInputController : Controller {
	private int lastFrame = 0;
	private PlayerIndex playerIndex;
	private GamePadState state;
	private GamePadState prevState;

	private string[] bindings = new string[(int)Controls.NUMBER_OF_CONTROLS];

	public XInputController(int controllerNumber){
		playerIndex = (PlayerIndex)controllerNumber;
		prevState = GamePad.GetState(playerIndex);
		ResetBindingsToDefault();
	}

	private void poll(){
		if(lastFrame<Time.frameCount){
			lastFrame = Time.frameCount;
			prevState = state;
			state = GamePad.GetState(playerIndex);
		}
	}

	public override string GetBindingForControl (Controls control){
		return bindings[(int)control];
	}

	public override void SetBindingForControl (Controls control, string newBinding){
		bindings[(int)control] = newBinding;
	}

	public override float GetAnalogControl(Controls c){
		poll ();
		string[] binds = bindings[(int)c].Split(';');
		float val = 0.0f;
		foreach (string s in binds){
			val = absMax(val, lookupAnalog(state, s));
		}
		return val;
	}

	public override bool GetDigitalControl (Controls c){
		poll();
		return getDigitalForState(c,state);
	}

	public override bool GetDigitalControlPressed (Controls c){
		return GetDigitalControl(c) && getDigitalForState (c,prevState);
	}

	private bool getDigitalForState(Controls c, GamePadState s){
		string[] binds = bindings[(int)c].Split(';');
		foreach(string subBind in binds){
			if(lookupDigital(s,subBind)) return true;
		}
		return false;
	}

	public override string GetControllerType (){
		return "XInput Controller";
	}

	public override int GetControllerNumber ()
	{
		return (int)playerIndex;
	}

	public override void SetVibration (float left, float right){
		GamePad.SetVibration(playerIndex, left, right);
	}

	public override void ResetBindingsToDefault (){
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

	private static float absMax(float a, float b){
		if(Mathf.Abs(a)>Mathf.Abs(b))return a;
		return b;
	}

	private static float lookupAnalog(GamePadState state, string identifier){
		if(identifier == "NOBIND")return 0.0f;
		if(!identifier.Contains("_")){
			switch(identifier){
			case "Triggers.Right": return state.Triggers.Right;
			case "Triggers.Left": return state.Triggers.Left;
			case "ThumbSticks.Left.X": return state.ThumbSticks.Left.X;
			case "ThumbSticks.Left.Y": return state.ThumbSticks.Left.Y;
			case "ThumbSticks.Right.X": return state.ThumbSticks.Right.X;
			case "ThumbSticks.Right.Y": return state.ThumbSticks.Right.Y;
			default:
				if(lookupDigital(state,identifier))
					return 1.0f;
				return 0.0f;
			}
		}else{
			string[] split = identifier.Split('_');
			string minusButton = split[0];
			string plusButton = split[1];
			float val = 0.0f;
			if(lookupDigital(state,minusButton)) val-=1.0f;
			if(lookupDigital(state,plusButton)) val+=1.0f;
			return val;
		}
		
	}

	private static bool lookupDigital(GamePadState state, string identifier){
		if(identifier == "NOBIND")return false;
		switch(identifier){
		case "Buttons.A": return state.Buttons.A == ButtonState.Pressed;
		case "Buttons.B": return state.Buttons.B == ButtonState.Pressed;
		case "Buttons.X": return state.Buttons.X == ButtonState.Pressed;
		case "Buttons.Y": return state.Buttons.Y == ButtonState.Pressed;
		case "Buttons.Start": return state.Buttons.Start == ButtonState.Pressed;
		case "Buttons.Back": return state.Buttons.Back == ButtonState.Pressed;
		case "Buttons.RightShoulder": return state.Buttons.RightShoulder == ButtonState.Pressed;
		case "Buttons.LeftShoulder": return state.Buttons.LeftShoulder == ButtonState.Pressed;
		case "Buttons.RightStick": return state.Buttons.RightStick == ButtonState.Pressed;
		case "Buttons.LeftStick": return state.Buttons.LeftStick == ButtonState.Pressed;
		case "DPad.Up": return state.DPad.Up == ButtonState.Pressed;
		case "DPad.Down": return state.DPad.Down == ButtonState.Pressed;
		case "DPad.Left": return state.DPad.Left == ButtonState.Pressed;
		case "DPad.Right": return state.DPad.Right == ButtonState.Pressed;
			//Analog to digital conversions
		case "Triggers.Right": return state.Triggers.Right >= Controller.ANALOG_DIGITAL_THRESHOLD;
		case "Triggers.Left": return state.Triggers.Left >= Controller.ANALOG_DIGITAL_THRESHOLD;
		case "ThumbSticks.Left.X+": return state.ThumbSticks.Left.X >= Controller.ANALOG_DIGITAL_THRESHOLD;
		case "ThumbSticks.Left.X-": return state.ThumbSticks.Left.X <= -Controller.ANALOG_DIGITAL_THRESHOLD;
		case "ThumbSticks.Left.Y+": return state.ThumbSticks.Left.Y >= Controller.ANALOG_DIGITAL_THRESHOLD;
		case "ThumbSticks.Left.Y-": return state.ThumbSticks.Left.Y <= -Controller.ANALOG_DIGITAL_THRESHOLD;
		case "ThumbSticks.Right.X+": return state.ThumbSticks.Right.X >= Controller.ANALOG_DIGITAL_THRESHOLD;
		case "ThumbSticks.Right.X-": return state.ThumbSticks.Right.X <= -Controller.ANALOG_DIGITAL_THRESHOLD;
		case "ThumbSticks.Right.Y+": return state.ThumbSticks.Right.Y >= Controller.ANALOG_DIGITAL_THRESHOLD;
		case "ThumbSticks.Right.Y-": return state.ThumbSticks.Right.Y <= -Controller.ANALOG_DIGITAL_THRESHOLD;
			
		default: return false;
		}
	}


}
#endif
