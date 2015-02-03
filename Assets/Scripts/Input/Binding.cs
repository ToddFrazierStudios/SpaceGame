using System.Collections;

public class Binding {
	public string BindString;
	public BindType Type;
	public bool IsInverted;
	public bool IsATrigger;
	public Binding AlternateBinding;

	public Binding(string bindString, BindType bindType, Binding next = null, bool isInverted = false, bool isTrigger = false){
		AlternateBinding = next;
		this.Type = bindType;
		BindString = bindString;
		IsInverted = isInverted;
		IsATrigger = isTrigger;
	}

	public enum BindType{
		DIRECT_DIGIAL,
		ANALOG_TO_DIGITAL_POSITIVE,
		ANALOG_TO_DIGITAL_NEGATIVE,
		DIRECT_ANALOG,
		DIGITAL_TO_ANALOG_POSITIVE,
		DIGITAL_TO_ANALOG_NEGATIVE
	}
}
