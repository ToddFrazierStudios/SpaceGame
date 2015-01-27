/// <summary>
/// This class represents a controller, any device for recieving input.  
/// </summary>
public abstract class Controller {
	///the minimum absolute value required for an analog device to be considered "pressed" when used as a digital button
	public const float ANALOG_DIGITAL_THRESHOLD = 0.25f;

	///returns the current value of the given analog control
	public abstract float GetAnalogControl(Controls c);

	///returns the current value of the given digital control
	public abstract bool GetDigitalControl(Controls c);

	///returns wether or not the given control was pressed this frame...
	///returns the same as GetDigitalControl for analog controls being treated as digital
	///(except on XInput; on XInput it works as expected)
	public abstract bool GetDigitalControlPressed(Controls c);

	/// <summary>
	/// Determines if the specified Control is a digital control.
	/// </summary>
	/// <returns><c>true</c> if the specified control is a digital control; otherwise, <c>false</c>.</returns>
	/// <param name="c">C.</param>
	public static bool IsADigitalControl(Controls c){
		return c >= Controls.FIRE;
	}

	///Returns a string identifing the general type of the controller
	public abstract string GetControllerType();

	///Sets the current vibration level for the controller,
	///currently only supported on XInput.
	///NOTE: you MUST set both values back to zero when you're done!
	public abstract void SetVibration(float left, float right);

	///Returns the current binding for the given control
	public abstract string GetBindingForControl(Controls control);

	///Sets the binding for the given control
	public abstract void SetBindingForControl(Controls control, string newBinding);

	/// <summary>
	/// Resets the bindings to default.
	/// </summary>
	public abstract void ResetBindingsToDefault();

	/// <summary>
	/// Gets the controller number.
	/// NOTE: this is NOT the player number.  Some Controllers don't care about this; ie Keyboard/mouse always returns -1
	/// </summary>
	/// <returns>The controller number.</returns>
	public abstract int GetControllerNumber();
}
