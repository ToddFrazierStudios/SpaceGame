#if false
using UnityEngine;
using System.Collections;

public class ControlLayout : MonoBehaviour {

	public bool invertX;
	public bool invertY;

	public enum OS {
		MAC, WINDOWS
	}
	public enum CONTROLLERTYPE {
		XBOXONE, XBOX360, CUSTOM
	}
	public enum LAYOUT {
		DEFAULT, CUSTOM
	}

	private OS os;
	public CONTROLLERTYPE controllerType;
	public LAYOUT layout;


	void Start() {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		os = OS.MAC;
#else
		os = OS.WINDOWS;
#endif
	}

	void Update() {
		if (controllerType == CONTROLLERTYPE.XBOX360) {
			ParsedInput.controller[0].controllerType = ParsedInput.Controller.CONTROLLERTYPE.XBOX360;
		} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
			ParsedInput.controller[0].controllerType = ParsedInput.Controller.CONTROLLERTYPE.XBOXONE;
		} else {
			ParsedInput.controller[0].controllerType = ParsedInput.Controller.CONTROLLERTYPE.OTHER;
		}
	}

//	private float getFire() {
//
//	}
//
//	private float getEngine() {
//
//	}
//
//	private float getRotate() {
//
//	}
//
//	private float getStrafe() {
//
//	}
//
//	private float getRoll() {
//
//	}

}
#endif
