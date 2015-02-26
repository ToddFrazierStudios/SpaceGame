using System.Collections;
using System.Text;

public class Binding {
	public string BindString;
	public BindType Type;
	public bool IsInverted;
	public bool IsATrigger;
	public Binding AlternateBinding;

    public bool previousValue = false;
    public int previousFrame = 0;

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
    public static Binding BuildBinding(string fullBindStringWithMetaChar, Binding next = null, bool isATrigger = false) {
        if (fullBindStringWithMetaChar == "NOBIND" || fullBindStringWithMetaChar.Substring(1) == "NOBIND") return null;
        int meta = GetMetaFromString(fullBindStringWithMetaChar);
        bool inverted = GetIsInvertedFromMeta(meta);
        BindType type = GetBindTypeFromMeta(meta);
        string bindString = fullBindStringWithMetaChar.Substring(1);
        return new Binding(bindString, type, next, inverted, isATrigger);
    }
    /// <summary>
    /// Builds a full bind Linked List from the given ';'-seperated list of full bindings (with meta chars).  
    /// The returned linked list of bindings should be in the same order as the input string.  
    /// If fullBindList == "NOBIND", null is returned.  
    /// </summary>
    /// <param name="fullBindList">the ';'-seperated list of bindings</param>
    /// <returns>The head of the list of linked Bindings, or null</returns>
    /// 
    public static Binding BuildBindingChain(string fullBindList) {
        return BuildBindingChain(fullBindList, DefaultBindingConverter);
    }

    public static Binding BuildBindingChain(string fullBindList, InputUtils.ConvertBindString converter) {
        if (fullBindList == "NOBIND") return null;
        string[] split = fullBindList.Split(InputUtils.BIND_SEPERATOR, System.StringSplitOptions.RemoveEmptyEntries);
        Binding next = null;
        foreach (string bind in split) {
            next = BuildBinding(converter(bind), next, bind.Contains("Trigger"));
        }
        return next;
    }

    public static BindType GetBindTypeFromMetaChar(string metaChar){
        return GetBindTypeFromMeta(GetMetaFromString(metaChar));
    }
    private static BindType GetBindTypeFromMeta(int meta) {
        return (BindType)(meta & 7);
    }
    private static int GetMetaFromString(string metaChar) {
        return System.Int32.Parse(metaChar.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier);
    }

    public static string BuildMetaChar(BindType type, bool isInverted) {
        int meta = (int)type;
        if (isInverted) {
            if (isInverted) {
                meta = meta | 8;
            } else {
                meta = meta & 7;
            }
        }
        return meta.ToString("X1");
    }
    public static bool GetIsInvertedFromMetaChar(string metaChar) {
        return GetIsInvertedFromMeta(GetMetaFromString(metaChar));
    }
    private static bool GetIsInvertedFromMeta(int meta) {
        return (meta & 8) != 0;
    }

    public string ConvertBindChainToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(BuildMetaChar(Type, IsInverted));
        sb.Append(BindString);
        if (AlternateBinding != null) {
            sb.Append(";");
            sb.Append(AlternateBinding.ConvertBindChainToString());
        }
        return sb.ToString();
    }

    public static string DefaultBindingConverter(string str) { return str; }

	public enum BindType{
		DIRECT_DIGIAL,
		ANALOG_TO_DIGITAL_POSITIVE,
		ANALOG_TO_DIGITAL_NEGATIVE,
		DIRECT_ANALOG,
		DIGITAL_TO_ANALOG_POSITIVE,
		DIGITAL_TO_ANALOG_NEGATIVE
	}
}
