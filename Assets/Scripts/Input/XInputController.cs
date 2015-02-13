#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class XInputController : Controller {
	private int lastFrame = 0;
	private PlayerIndex playerIndex;
	private GamePadState state;
	private GamePadState prevState;

	public XInputController(int controllerNumber){
		playerIndex = (PlayerIndex)controllerNumber;
		prevState = GamePad.GetState(playerIndex);
	}

	private void poll(){
		if(lastFrame<Time.frameCount){
			lastFrame = Time.frameCount;
			prevState = state;
			state = GamePad.GetState(playerIndex);
		}
	}

    public void ResetAllAxes() {
        lastFrame = 0;
        //the actual reset will hapen on the next call to poll();
        //... sure...
    }

    public InputUtils.ControllerType GetControllerType() {
        return InputUtils.ControllerType.JOYSTICK;
	}

    public float PollAnalog(string bind) {
        poll();
        return lookupAnalog(state, bind);
    }

    public bool PollDigital(string bind) {
        poll();
        return lookupDigital(state, bind);
    }

    public bool PollDigitalPressed(string bind) {
        return PollDigital(bind) && !lookupDigital(prevState, bind);
    }

	public string GetControllerDescription (){
		return "XInput Controller";
	}

    public InputUtils.Implementations GetControllerImplementation() {
        return InputUtils.Implementations.XINPUT_CONTROLLER;
	}

	public int GetControllerNumber () {
		return (int)playerIndex;
	}

	public void SetVibration (float left, float right){
		GamePad.SetVibration(playerIndex, left, right);
	}

    public string ConvertBindString(string s) {
        return s;
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
