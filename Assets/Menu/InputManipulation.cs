using UnityEngine;
using System;
using UnityEngine.UI;

public class InputManipulation : MonoBehaviour {

	public Text[] controlText;
	public Toggle lookInversion;
	private bool newInverted;

	void Start() {
		string bind = PlayerInput.GetBindingsForControl(0, Controls.LOOK_Y);
		string metaChar = bind.Substring(0,1);
		int meta = System.Int32.Parse(metaChar,System.Globalization.NumberStyles.AllowHexSpecifier);//parse the meta char
		lookInversion.isOn = !((meta & 8) != 0);//isolate the inversion
//		Debug.Log (PlayerInput.GetBindingsForControl(0, (Controls)7));
		SetText();
//		for (int i = 0; i < Enum.GetNames (typeof(Controls)).Length; i++) {
//			Debug.Log (PlayerInput.GetBindingsForControl(0, (Controls)i));
//		}
	}

	public void SetText() {
		for (int i = 0; i < Enum.GetNames (typeof(Controls)).Length - 1; i++) {
			for (int j = 0; j < controlText.Length; j++) {
				if (PlayerInput.GetBindingsForControl(0, (Controls) i).Substring (1).Contains (controlText[j].name)) {
					if (controlText[j].text.Equals ("")) {
						controlText[j].text = ((Controls) i).ToString();
					} else if (controlText[j].text != "ALT_FIRE") {
						controlText[j].text =((Controls) i).ToString().Substring (0, ((Controls) i).ToString ().Length - 2);
					}
				}
			}
		}
	}

	public void invertLookY() {
		string bind = PlayerInput.GetBindingsForControl(0, Controls.LOOK_Y);
		string metaChar = bind.Substring(0,1);
		int meta = System.Int32.Parse(metaChar,System.Globalization.NumberStyles.AllowHexSpecifier);//parse the meta char
		bool inverted = (meta & 8) != 0;//isolate the inversion
		meta = meta & 7;//strip out the inversion bit
		string bindString = bind.Substring(1);//strip out the meta char
		int newMeta = meta;
		newInverted = !inverted;//reinsert the inversion flag
		if(newInverted){
			newMeta = newMeta | 8;
		}else{
			newMeta = newMeta & 7;
		}
		//convert that to the proper char
		metaChar = newMeta.ToString("X1");
		PlayerInput.RebindControl(0, Controls.LOOK_Y, metaChar + bindString);
	}
}
