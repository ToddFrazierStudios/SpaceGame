using UnityEngine;
using System.Collections;

public class InputLister : MonoBehaviour {
    [Range(1,4)]
    public int controllerNumber = 1;
	
	// Update is called once per frame
	void Update () {
        DebugHUD.setValue("Connected Controllers", string.Join(",", Input.GetJoystickNames()));
        DebugHUD.setValue("Selected Controller", controllerNumber);
        //Buttons
        for (int i = 0; i < 20; i++) {
            DebugHUD.setValue("Button " + i, Input.GetKey("joystick " + controllerNumber + " button " + i));
        }
        //Axis
        DebugHUD.setValue("Axis X", Input.GetAxis("joystick " + controllerNumber + " axis X"));
        DebugHUD.setValue("Axis Y", Input.GetAxis("joystick " + controllerNumber + " axis Y"));
        for (int i = 3; i <= 10; i++) {
            DebugHUD.setValue("Axis " + i, Input.GetAxis("joystick " + controllerNumber + " axis " + i));
        }
	}
}
