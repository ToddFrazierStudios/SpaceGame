using System.Collections;

public class Binding {
	public string BindString;
	public BindType Type;
	public bool IsInverted;
	public bool IsATrigger;
	public Binding AlternateBinding;

	private Binding(string bindString, BindType bindType, Binding next = null, bool isInverted = false, bool isTrigger = false){
		AlternateBinding = next;
		this.Type = bindType;
		BindString = bindString;
		IsInverted = isInverted;
		IsATrigger = isTrigger;
	}
    /// <summary>
    /// Builds a Binding using the given bind string and an optional next binding
    /// </summary>
    /// <param name="fullBindStringWithMetaChar"></param>
    /// <param name="next">(optional) the next binding in the chain, or null if not specified</param>
    /// <returns></returns>
    public static Binding BuildBinding(string fullBindStringWithMetaChar, Binding next = null) {
        if (fullBindStringWithMetaChar == "NOBIND" || fullBindStringWithMetaChar.Substring(1) == "NOBIND") return null;
        int meta = System.Int32.Parse(fullBindStringWithMetaChar.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
        bool inverted = (meta & 8) != 0;//isolate the inversion
        meta = meta & 7;//strip out the inversion bit
        string bindString = fullBindStringWithMetaChar.Substring(1);
        bool isATrigger = bindString.Contains("Trigger");
        BindType type = (BindType)meta;
        return new Binding(bindString, type, next, inverted, isATrigger);
    }
    /// <summary>
    /// Builds a full bind Linked List from the given ';'-seperated list of full bindings (with meta chars).  
    /// The returned linked list of bindings should be in the same order as the input string.  
    /// If fullBindList == "NOBIND", null is returned.  
    /// </summary>
    /// <param name="fullBindList">the ';'-seperated list of bindings</param>
    /// <returns>The head of the list of linked Bindings, or null</returns>
    public static Binding BuildBindingChain(string fullBindList) {
        if (fullBindList == "NOBIND") return null;
        string[] split = fullBindList.Split(InputUtils.BIND_SEPERATOR, System.StringSplitOptions.RemoveEmptyEntries);
        Binding next = null;
        foreach (string bind in split) {
            next = BuildBinding(bind, next);
        }
        return next;
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
