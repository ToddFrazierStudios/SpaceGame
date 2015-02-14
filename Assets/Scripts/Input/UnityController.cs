using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnityController : Controller, ProfiledController {
	private static string rightThumbStickYAxisName;
	private int controllerNumber;
    private ControllerProfile profile;

    private Dictionary<string, bool> triggerTable;



	public UnityController(int controllerNumber, ControllerProfile profile){
		this.controllerNumber = controllerNumber;
        this.profile = profile;
		triggerTable = new Dictionary<string, bool> ();
	}

	public string GetControllerDescription ()
	{
		return "Unity Controller";
	}

	public void SetVibration (float left, float right)
	{
		//NOP
	}

    public string ConvertBindString(string bind) {
        if (profile == null) return bind;
        string platformBindString = convertFromProfile(bind.Substring(1));
        string convertedBindString = bind.Substring(0, 1) + platformBindString;
        if (bind.Contains("ThumbStick.Right.Y")) rightThumbStickYAxisName = platformBindString;

        //build trigger table
        if (profile.OSXTriggerCompNeeded && bind.Contains("Trigger")) {
			if(!triggerTable.ContainsKey(platformBindString))
            	triggerTable.Add(platformBindString, false);
        }

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        //special fixes for windows where the DPad is an axis instead of buttons
        if (bind.Contains("DPad")) {
            Binding.BindType bindType = Binding.GetBindTypeFromMetaChar(bind);
            Binding.BindType newBindType = bindType;
            bool shouldBeInverted = Binding.GetIsInvertedFromMetaChar(bind);
            switch (bindType) {
                case Binding.BindType.DIRECT_DIGIAL:
                    if (bind.Contains("Up") || bind.Contains("Right")) {
                        newBindType = Binding.BindType.ANALOG_TO_DIGITAL_POSITIVE;
                    } else {
                        newBindType = Binding.BindType.ANALOG_TO_DIGITAL_NEGATIVE;
                    }
                    break;
                case Binding.BindType.DIGITAL_TO_ANALOG_POSITIVE:
                    newBindType = Binding.BindType.DIRECT_ANALOG;
                    if (bind.Contains("Down") || bind.Contains("Left")) {
                        shouldBeInverted = !shouldBeInverted;
                    }
                    break;
                case Binding.BindType.DIGITAL_TO_ANALOG_NEGATIVE:
                    newBindType = Binding.BindType.DIRECT_ANALOG;
                    if (bind.Contains("Up") || bind.Contains("Right")) {
                        shouldBeInverted = !shouldBeInverted;
                    }
                    break;
            }
            convertedBindString = Binding.BuildMetaChar(newBindType, shouldBeInverted) + convertedBindString.Substring(1);
        }
#endif

        return convertedBindString;
    }

	public int GetControllerNumber ()
	{
		return controllerNumber;
	}

    private string convertFromProfile(string bind) {
        if (profile == null) return bind;
        return String.Format(profile[bind], controllerNumber + 1);
    }

    public float PollAnalog(string bind) {
        return GetAxis(bind);
    }

    public bool PollDigital(string bind) {
        return Input.GetKey(bind);
    }

    public void ResetAllAxes() {
        Input.ResetInputAxes();

        if (profile != null && profile.OSXTriggerCompNeeded) {
            List<string> keylist = new List<string>();
            keylist.AddRange(triggerTable.Keys);
            foreach (string key in keylist) {
                triggerTable[key] = false;
            }
        }

    }

    public bool PollDigitalPressed(string bind) {
        return Input.GetKeyDown(bind);
    }

	//Special GetAxis function, replaces Input.GetAxis for this class,
	//includes compensation for the upside-down right thubstick and for macmamac trigger issues
	private float GetAxis(string identifier){
		float value = Input.GetAxis(identifier);
		if(profile!=null && profile.InvertedRightStick && identifier==rightThumbStickYAxisName)value = -value;

        //mac trigger compensation
		if(profile!=null && profile.OSXTriggerCompNeeded && triggerTable.ContainsKey(identifier)){
			value = Input.GetAxisRaw(identifier);
			if(triggerTable[identifier])return (value+1f)/2f;
			if(value!=0.0f)triggerTable[identifier] = true;
			return 0.0f;
		}

		return value;
	}

    public InputUtils.ControllerType GetControllerType() {
        return InputUtils.ControllerType.JOYSTICK;
	}

    public InputUtils.Implementations GetControllerImplementation() {
        return InputUtils.Implementations.UNITY_CONTROLLER;
	}

    public string GetProfileName() {
        if (HasProfile()) return profile.DisplayName;
        return null;
    }

    public bool HasProfile() {
        return profile != null;
    }
}
