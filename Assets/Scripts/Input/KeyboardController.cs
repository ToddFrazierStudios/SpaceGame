using UnityEngine;
using System.Collections;
using System;

public class KeyboardController : Controller {
	private const float MOUSE_ABS_DEADZONE = 0.1f;

	public KeyboardController(){
		//NOP
	}

	public void SetVibration (float left, float right){
		//NOP
	}
	
	public string GetControllerDescription () {
		return "Keyboard/Mouse";
	}
    public InputUtils.Implementations GetControllerImplementation() {
        return InputUtils.Implementations.KEYBOARD_CONTROLLER;
	}

	public int GetControllerNumber () {
		return -1;
	}

    public InputUtils.ControllerType GetControllerType() {
        return InputUtils.ControllerType.KEYBOARD;
	}

    public void ResetAllAxes() {
        Input.ResetInputAxes();
    }

    public float PollAnalog(string bind) {
        float value = 0f;
        if (bind == "MouseAbs X") {
            float mouseX = Input.mousePosition.x;
            mouseX = (mouseX - (Screen.width / 2));
            value = mouseX / (Screen.width / 2);
            if (value < MOUSE_ABS_DEADZONE && value > -MOUSE_ABS_DEADZONE) value = 0f;
        } else if (bind == "MouseAbs Y") {
            float mouseY = Input.mousePosition.y;
            mouseY = (mouseY - (Screen.height / 2));
            value = mouseY / (Screen.height / 2);
            if (value < MOUSE_ABS_DEADZONE && value > -MOUSE_ABS_DEADZONE) value = 0f;
        } else {
            value = Input.GetAxis(bind);
        }
        return value;
    }

    public bool PollDigital(string bind) {
        return Input.GetKey(bind);
    }

    public bool PollDigitalPressed(string bind) {
        return Input.GetKeyDown(bind);
    }

    public string ConvertBindString(string s) { 
        return s;
    }
}
