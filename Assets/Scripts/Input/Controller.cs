/// <summary>
/// This class represents a controller, any device for recieving input.  
/// </summary>
public interface Controller {
	///returns the current value of the given analog control
	float GetAnalogControl(Controls c);

	///returns the current value of the given digital control
	bool GetDigitalControl(Controls c);

	///returns wether or not the given control was pressed this frame...
	///returns the same as GetDigitalControl for analog controls being treated as digital
	///(except on XInput; on XInput it works as expected)
	bool GetDigitalControlPressed(Controls c);

	///Returns a string identifing the general type of the controller
	string GetControllerDescription();

	///Sets the current vibration level for the controller,
	///currently only supported on XInput.
	///NOTE: you MUST set both values back to zero when you're done!
	void SetVibration(float left, float right);

	///Sets the binding for the given control
	void SetBindingForControl(Controls control, string newBinding);

	/// <summary>
	/// Resets the bindings to default.
	/// </summary>
	void ResetBindingsToDefault();

	/// <summary>
	/// Gets the controller number.
	/// NOTE: this is NOT the player number.  Some Controllers don't care about this; ie Keyboard/mouse always returns -1
	/// </summary>
	/// <returns>The controller number.</returns>
	int GetControllerNumber();

	InputUtils.ControllerType GetControllerType();

	InputUtils.Implementations GetControllerImplementation();
	
}
