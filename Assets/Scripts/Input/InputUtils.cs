using UnityEngine;
using System.Collections;

public static class InputUtils {
	public const Controls FIRST_DIGITAL_CONTROL = Controls.FIRE;

	/// <summary>
	/// Determines if the specified Control is a digital control.
	/// </summary>
	/// <returns><c>true</c> if the specified control is a digital control; otherwise, <c>false</c>.</returns>
	/// <param name="c">C.</param>
	public static bool IsADigitalControl(Controls c){
		return c >= FIRST_DIGITAL_CONTROL;
	}
	public static int GetNumberOfAnalogControls(){
		return ((int)FIRST_DIGITAL_CONTROL);
	}

	public enum ControllerType{
		JOYSTICK,KEYBOARD
	}
	
	public enum Implementations {
		NONE,
		KEYBOARD_CONTROLLER,
		UNITY_CONTROLLER
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		,XINPUT_CONTROLLER
		#endif
	};
}
