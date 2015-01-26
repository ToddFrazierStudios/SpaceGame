public abstract class Controller {
	//the minimum value required for an analog device to be considered "pressed" when used for a digital button
	public const float ANALOG_DIGITAL_THRESHOLD = 0.25f;

	public abstract float GetAnalogControl(Controls c);

	public abstract bool GetDigitalControl(Controls c);

	public abstract bool GetDigitalControlPressed(Controls c);

	public static bool IsADigitalControl(Controls c){
		return c >= Controls.FIRE;
	}

	public abstract string GetControllerType();
	
	public abstract void SetVibration(float left, float right);

	public abstract string GetBindingForControl(Controls control);

	public abstract void SetBindingForControl(Controls control, string newBinding);

	public abstract void ResetBindingsToDefault();

	public abstract int GetControllerNumber();
}
