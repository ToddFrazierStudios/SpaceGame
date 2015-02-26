using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
public class PlayerPref
{
	private string[] originalBindings = new string[(int)Controls.NUMBER_OF_CONTROLS];
    private Binding[] bindings = new Binding[(int)Controls.NUMBER_OF_CONTROLS];

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
        if (controller != null) {
            //Debug.Log("A new controller was set!");
            loadFromPrefs();
        }
	}

	public void RebindControl(Controls c, string newBind){
        //Debug.Log(String.Format("Rebinding from {0} to {1}", originalBindings[(int)c], newBind));
		originalBindings[(int)c] = newBind;
        bindings[(int)c] = Binding.BuildBindingChain(newBind, controller.ConvertBindString);
		saveBindingToPrefs (c, newBind);
	}

	public string GetBindingsForControl(Controls c){
		return originalBindings[(int)c];
	}

	public float GetAnalogControl(Controls c){
		if(controller==null)return 0.0f;
        Binding bind = bindings[(int)c];
        float val = 0.0f;
        while (bind != null) {
            val += Mathf.Clamp(pollControllerForSingleAnalogBind(bind), -1.0f, 1.0f);
            bind = bind.AlternateBinding;
        }
        return Mathf.Clamp(val, -1.0f, 1.0f);
	}

    public void ResetAllAxes() {
        if (controller != null) {
            controller.ResetAllAxes();
        }
        foreach (Binding b in bindings) {
            Binding cur = b;
            while (cur != null) {
                cur.previousFrame = 0;
                cur.previousValue = false;
                cur = cur.AlternateBinding;
            }
        }
    }

    private float pollControllerForSingleAnalogBind(Binding bind) {
        if (bind == null || controller == null) return 0.0f;
        float value = 0.0f;
        switch (bind.Type) {
            case Binding.BindType.DIRECT_ANALOG:
                value = controller.PollAnalog(bind.BindString);
                break;
            case Binding.BindType.DIGITAL_TO_ANALOG_NEGATIVE:
                if (controller.PollDigital(bind.BindString))
                    value = -1.0f;
                else
                    value = 0.0f;
                break;
            case Binding.BindType.DIGITAL_TO_ANALOG_POSITIVE:
                if (controller.PollDigital(bind.BindString))
                    value = 1.0f;
                else
                    value = 0.0f;
                break;
            default:
                Debug.LogError("Invalid Binding! Bind " + bind.BindString + " was polled as an analog bind, but it is type " + bind.Type);
                return 0.0f;
        }
        if (bind.IsATrigger && bind.IsInverted) return 1.0f - value;
        if (bind.IsInverted) return -value;
        return value;
    }

	public bool GetDigitalControl(Controls c){
        return getDigitalControlFromBinds(c, false);
	}

    private bool getDigitalControlFromBinds(Controls c, bool down) {
        if (controller == null) return false;
        Binding bind = bindings[(int)c];
        while (bind != null) {
            if (pollControllerForSingleDigitalBind(bind, down)) return true;
            bind = bind.AlternateBinding;
        }
        return false;
    }

    private bool pollControllerForSingleDigitalBind(Binding bind, bool down) {
        bool value = false;
        switch (bind.Type) {
            case Binding.BindType.DIRECT_DIGIAL:
                if (down)
                    value = controller.PollDigitalPressed(bind.BindString);
                else
                    value = controller.PollDigital(bind.BindString);
                break;
            case Binding.BindType.ANALOG_TO_DIGITAL_NEGATIVE:
                bool thisFrame = controller.PollAnalog(bind.BindString) <= -InputUtils.ANALOG_DIGITAL_THRESHOLD;
                if (down) {
                    value = thisFrame && !bind.previousValue;
                } else {
                    value = thisFrame;
                }
                break;
            case Binding.BindType.ANALOG_TO_DIGITAL_POSITIVE:
                bool thisFrame2 = controller.PollAnalog(bind.BindString) >= InputUtils.ANALOG_DIGITAL_THRESHOLD;
                if (down) {
                    value = thisFrame2 && !bind.previousValue;
                } else {
                    value = thisFrame2;
                }
                break;
            default:
                Debug.LogError("Invalid Binding! Bind " + bind.BindString + " was polled as a digital bind, but it is type " + bind.Type);
                return false;
        }
        if (bind.IsInverted) return !value;
        if (Time.frameCount>bind.previousFrame) {
            bind.previousValue = value;
            bind.previousFrame = Time.frameCount;
        }
        return value;
    }
	public bool GetDigitalControlPressed(Controls c){
        return getDigitalControlFromBinds(c, true);
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

	private void saveToPrefs(){
		for(int i = 0; i< (int)Controls.NUMBER_OF_CONTROLS; i++){
            //Debug.Log ("saving binding "+originalBindings[i]);
			saveBindingToPrefs((Controls)i,originalBindings[i]);
		}
		PlayerPrefs.Save();
	}

	private void saveBindingToPrefs(Controls c, string bind){
		PlayerPrefs.SetString (generatePrefString (c), bind);
	}

	private void loadFromPrefs(){
        //Debug.Log("Loading from prefs!");
        bool isKeyboard = controller.GetControllerType() == InputUtils.ControllerType.KEYBOARD;
        string[] defaultTable = defaultControllerBindings;
        if (isKeyboard) {
            defaultTable = defaultKeyboardBindings;
        }

		for(int i = 0; i< (int)Controls.NUMBER_OF_CONTROLS; i++){
            //Debug.Log(String.Format("Loading using default {0}", defaultTable[i]));
			string prefString = generatePrefString((Controls)i);
            //Debug.Log (String.Format ("PlayerPrefs.HasKey({0}) = {1}", prefString, PlayerPrefs.HasKey(prefString)));
            string loaded = PlayerPrefs.GetString(prefString, defaultTable[i]);
            //Debug.Log("Loaded value: " + loaded);
            if (loaded == "") loaded = defaultTable[i];
			RebindControl((Controls)i, loaded);
		}
	}

    private string generatePrefString(Controls c) {
        StringBuilder sb = new StringBuilder("InputBindings.");
        if (controller.GetControllerType() == InputUtils.ControllerType.KEYBOARD) {
            sb.Append("keyboard:");
        } else {
            sb.Append(string.Format("player{0}.controller:", playerNumber));
        }
        sb.Append(Enum.GetName(typeof(Controls), c));
        return sb.ToString();
    }

	public void ResetBindingsToDefault(){
        string[] defaults = defaultControllerBindings;
        if (controller.GetControllerType() == InputUtils.ControllerType.KEYBOARD) {
            defaults = defaultKeyboardBindings;
		}
        for (int i = 0; i < (int)Controls.NUMBER_OF_CONTROLS; i++) {
            RebindControl((Controls)i, defaults[i]);
        }
        saveToPrefs();
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

		defaultKeyboardBindings[(int)Controls.LOOK_X] = "3Mouse X";
		defaultKeyboardBindings[(int)Controls.LOOK_Y] = "3Mouse Y";
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

    public bool ProfilesSupportedByController() {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (controller is XInputController) return true;
#endif

        return controller is ProfiledController;
    }

    public string GetProfileName() {
        ProfiledController pc = controller as ProfiledController;
        if (pc == null) return null;
        if (!pc.HasProfile()) return "No Profile";
        return pc.GetProfileName();
    }
}

