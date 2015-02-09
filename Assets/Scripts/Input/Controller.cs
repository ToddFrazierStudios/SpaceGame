/// <summary>
/// This class represents a controller, any device for recieving input.  
/// </summary>
public interface Controller {
	///the minimum absolute value required for an analog device to be considered "pressed" when used as a digital button
	public const float ANALOG_DIGITAL_THRESHOLD = 0.25f;

	public static char[] BIND_SEPERATOR = {';'};
	public static char[] DIGITAL_TO_ANALOG_SEPERATOR = {'_'};

	///returns the current value of the given analog control
	public float GetAnalogControl(Controls c);

	///returns the current value of the given digital control
	public bool GetDigitalControl(Controls c);

	///returns wether or not the given control was pressed this frame...
	///returns the same as GetDigitalControl for analog controls being treated as digital
	///(except on XInput; on XInput it works as expected)
	public bool GetDigitalControlPressed(Controls c);

	///Returns a string identifing the general type of the controller
	public string GetControllerDescription();

	///Sets the current vibration level for the controller,
	///currently only supported on XInput.
	///NOTE: you MUST set both values back to zero when you're done!
	public void SetVibration(float left, float right);

	///Sets the binding for the given control
	public void SetBindingForControl(Controls control, string newBinding);

	/// <summary>
	/// Resets the bindings to default.
	/// </summary>
	public void ResetBindingsToDefault();

	/// <summary>
	/// Gets the controller number.
	/// NOTE: this is NOT the player number.  Some Controllers don't care about this; ie Keyboard/mouse always returns -1
	/// </summary>
	/// <returns>The controller number.</returns>
	public int GetControllerNumber();

	public InputUtils.ControllerType GetControllerType();

	public InputUtils.Implementations GetControllerImplementation();
	
}
