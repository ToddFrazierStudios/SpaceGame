#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class XInputController : Controller {
	private int lastFrame = 0;
	private PlayerIndex playerIndex;
	private GamePadState state;
	private GamePadState prevState;

	private Binding[] bindings = new Binding[(int)Controls.NUMBER_OF_CONTROLS];

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
	private Binding buildBinding(string bind, Binding previous){
		int meta = System.Int32.Parse(bind.Substring(0,1),System.Globalization.NumberStyles.AllowHexSpecifier);
		bool inverted = (meta & 8) != 0;//isolate the inversion
		meta = meta & 7;//strip out the inversion bit
		return new Binding(bind.Substring(1),(Binding.BindType)meta,previous,inverted,bind.Contains("Trigger"));
	}

	public override ControllerType GetControllerType ()
	{
		return ControllerType.JOYSTICK;
	}

	public override float GetAnalogControl(Controls c){
		poll ();
		Binding bind = bindings[(int)c];
		float val = 0.0f;
		while(bind!=null){
			val+=Mathf.Clamp(pollAnalog(bind), -1.0f, 1.0f);
			bind = bind.AlternateBinding;
		}
		return Mathf.Clamp(val, -1.0f, 1.0f);
	}
	private float pollAnalog(Binding bind){
		float value = 0.0f;
		switch(bind.Type){
		case Binding.BindType.DIRECT_ANALOG:
			value = lookupAnalog(state,bind.BindString);
			break;
		case Binding.BindType.DIGITAL_TO_ANALOG_NEGATIVE:
			if(lookupDigital(state,bind.BindString))
				value = -1.0f;
			else 
				value = 0.0f;
			break;
		case Binding.BindType.DIGITAL_TO_ANALOG_POSITIVE:
			if(lookupDigital(state,bind.BindString))
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

	private bool pollDigital(GamePadState s, Binding bind){
		bool value = false;
		switch(bind.Type){
		case Binding.BindType.DIRECT_DIGIAL:
			value = lookupDigital(s,bind.BindString);
			break;
		case Binding.BindType.ANALOG_TO_DIGITAL_NEGATIVE:
			value = lookupAnalog(s,bind.BindString) <= -Controller.ANALOG_DIGITAL_THRESHOLD;
			break;
		case Binding.BindType.ANALOG_TO_DIGITAL_POSITIVE:
			value = lookupAnalog(s,bind.BindString) >= Controller.ANALOG_DIGITAL_THRESHOLD;
			break;
		default:
			Debug.LogError("Invalid Binding! Bind "+bind.BindString+" was polled as a analog bind, but it is type "+bind.Type);
			return false;
		}
		if(bind.IsInverted)return !value;
		return value;
	}

	public override bool GetDigitalControl (Controls c){
		poll();
		return getDigitalForState(c,state);
	}

	public override bool GetDigitalControlPressed (Controls c){
		return GetDigitalControl(c) && getDigitalForState (c,prevState);
	}

	private bool getDigitalForState(Controls c, GamePadState s){
		Binding bind = bindings[(int) c];
		while(bind!=null){
			if(pollDigital(s,bind))return true;
			bind = bind.AlternateBinding;
		}
		return false;
	}

	public override string GetControllerDescription (){
		return "XInput Controller";
	}

	public override Implementations GetControllerImplementation()
	{
		return Implementations.XINPUT_CONTROLLER;
	}

	public override int GetControllerNumber ()
	{
		return (int)playerIndex;
	}

	public override void SetVibration (float left, float right){
		GamePad.SetVibration(playerIndex, left, right);
	}

	public override void ResetBindingsToDefault (){
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

	private static float absMax(float a, float b){
		if(Mathf.Abs(a)>Mathf.Abs(b))return a;
		return b;
	}

	private static float lookupAnalog(GamePadState state, string identifier){
		if(identifier == "NOBIND")return 0.0f;
		switch(identifier){
		case "Triggers.Right": return state.Triggers.Right;
		case "Triggers.Left": return state.Triggers.Left;
		case "ThumbSticks.Left.X": return state.ThumbSticks.Left.X;
		case "ThumbSticks.Left.Y": return state.ThumbSticks.Left.Y;
		case "ThumbSticks.Right.X": return state.ThumbSticks.Right.X;
		case "ThumbSticks.Right.Y": return state.ThumbSticks.Right.Y;
		default:return 0.0f;
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
		case "Buttons.Guide": return state.Buttons.Guide == ButtonState.Pressed;
		case "Buttons.RightShoulder": return state.Buttons.RightShoulder == ButtonState.Pressed;
		case "Buttons.LeftShoulder": return state.Buttons.LeftShoulder == ButtonState.Pressed;
		case "Buttons.RightStick": return state.Buttons.RightStick == ButtonState.Pressed;
		case "Buttons.LeftStick": return state.Buttons.LeftStick == ButtonState.Pressed;
		case "DPad.Up": return state.DPad.Up == ButtonState.Pressed;
		case "DPad.Down": return state.DPad.Down == ButtonState.Pressed;
		case "DPad.Left": return state.DPad.Left == ButtonState.Pressed;
		case "DPad.Right": return state.DPad.Right == ButtonState.Pressed;
		default: return false;
		}
	}


}
#endif
