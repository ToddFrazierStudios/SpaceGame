using UnityEngine;
using System.Collections;

public static class InputUtils {
	public const Controls FIRST_DIGITAL_CONTROL = Controls.FIRE;

    ///the minimum absolute value required for an analog device to be considered "pressed" when used as a digital button
    public const float ANALOG_DIGITAL_THRESHOLD = 0.25f;

    public static char[] BIND_SEPERATOR = { ';' };
    public static char[] DIGITAL_TO_ANALOG_SEPERATOR = { '_' };

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
        KEYBOARD_CONTROLLER = -1,
		NONE = 0,
		UNITY_CONTROLLER = 1
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		,XINPUT_CONTROLLER = 2
		#endif
	};

    public delegate string ConvertBindString(string bindString);

    public static float AbsMax(float a, float b) {
        if (Mathf.Abs(a) > Mathf.Abs(b)) return a;
        return b;
    }
}
