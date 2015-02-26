using UnityEngine;
using System.Collections;

//needs to control:
// Strafe Manager
// Rotation Manager
// Weapons Manager
// Engine Thruster
// Boost

public class ShipController : MonoBehaviour {
	public bool isAi = false;
	[Range(0,3)]
	public int playerNumber = 0;
	public bool useKeyboard = true;
	public bool useController = true;

	[System.NonSerialized]
	public StrafeManager strafeManager;
	[System.NonSerialized]
	public RotationManager rotationManager;
	[System.NonSerialized]
	public WeaponsManager weaponsManager;
	[System.NonSerialized]
	public EngineThruster engineThruster;
	[System.NonSerialized]
	public Boost boost;
	private bool isMaximized;
	// Use this for initialization
	void Start () {
		rigidbody.inertiaTensor = new Vector3(55015.5f, 55015.5f, 55015.5f);
//		gameView = EditorWindow.focusedWindow;
//		isMaximized = gameView.maximized;
		strafeManager = GetComponent<StrafeManager>();
		rotationManager = GetComponent<RotationManager>();
		weaponsManager = GetComponent<WeaponsManager>();
		engineThruster = GetComponent<EngineThruster>();
		engineThruster.throttle = 0f;
		boost = GetComponent<Boost>();
	}

	void OnApplicationFocus(bool focusStatus) {
		PlayerInput.ResetAllAxes();
	}
	
	// Update is called once per frame
	void Update () {
		DebugHUD.setValue ("left trigger", PlayerInput.PollAnalogControl (playerNumber, Controls.THROTTLE));
//		if (EditorApplication.isPaused) {
//			ParsedInput.controller[0].ResetAllAxes();
//		}

		if(isAi)return;

		// Strafe Manager //
		if(strafeManager){
			strafeManager.xInput = PlayerInput.PollAnalogControl(playerNumber, Controls.STRAFE_X);
            strafeManager.yInput = PlayerInput.PollAnalogControl(playerNumber, Controls.STRAFE_Y);
			
			if (strafeManager.xInput == 0f && strafeManager.yInput == 0f) {
				engineThruster.isStrafing = false;
			} else {
				engineThruster.isStrafing = true;
			}
		}

		// Rotation Manager //
		if(rotationManager){
            rotationManager.xInput = PlayerInput.PollAnalogControl(playerNumber, Controls.LOOK_X);
            rotationManager.yInput = PlayerInput.PollAnalogControl(playerNumber, Controls.LOOK_Y);
//			rotationManager.xRightInput = ParsedInput.controller [playerNumber].RightStickX;
//			rotationManager.yRightInput = ParsedInput.controller [playerNumber].RightStickY;
            rotationManager.rotationInput = PlayerInput.PollAnalogControl(playerNumber, Controls.ROLL);
		}

		// Weapons Manager //
		if(weaponsManager){
            weaponsManager.primaryFire = PlayerInput.PollDigitalControl(playerNumber, Controls.FIRE);
            weaponsManager.secondaryFire = PlayerInput.PollDigitalControlPressed(playerNumber, Controls.ALT_FIRE);
		}

		// Engine Thruster //
		if(engineThruster){
            engineThruster.throttle = PlayerInput.PollAnalogControl(playerNumber, Controls.THROTTLE);
            engineThruster.reverse = PlayerInput.PollDigitalControl(playerNumber, Controls.DAMPENERS);
		}

		// Boost //
		if(boost){
            boost.activate = PlayerInput.PollDigitalControl(playerNumber, Controls.BOOST);
		}
	}
}
