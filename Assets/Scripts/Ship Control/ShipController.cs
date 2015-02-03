using UnityEngine;
using System.Collections;
//using UnityEditor;

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
	private bool resetNextFrame = false;
//	private EditorWindow gameView;
	private bool isMaximized;
	private PlayerPref controller;
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
		controller = GlobalControllerManager.GetPlayer(playerNumber);
	}

//	void OnApplicationFocus(bool focusStatus) {
//		if (focusStatus = true) {
//			ParsedInput.controller[0].ResetAllAxes();
//		}
//	}
	
	// Update is called once per frame
	void Update () {
//		if (EditorApplication.isPaused) {
//			ParsedInput.controller[0].ResetAllAxes();
//		}

		if(isAi)return;

		if(!networkView.isMine)return;

		// Strafe Manager //
		if(strafeManager){
			strafeManager.xInput = controller.GetAnalogControl(Controls.STRAFE_X);
			strafeManager.yInput = controller.GetAnalogControl(Controls.STRAFE_Y);
			
			if (strafeManager.xInput == 0f && strafeManager.yInput == 0f) {
				engineThruster.isStrafing = false;
			} else {
				engineThruster.isStrafing = true;
			}
		}

		// Rotation Manager //
		if(rotationManager){
			rotationManager.xInput = controller.GetAnalogControl(Controls.LOOK_X);
			rotationManager.yInput = controller.GetAnalogControl(Controls.LOOK_Y);
//			rotationManager.xRightInput = ParsedInput.controller [playerNumber].RightStickX;
//			rotationManager.yRightInput = ParsedInput.controller [playerNumber].RightStickY;
			rotationManager.rotationInput = controller.GetAnalogControl(Controls.ROLL);
		}

		// Weapons Manager //
		if(weaponsManager){
			weaponsManager.primaryFire = controller.GetDigitalControl(Controls.FIRE);
			weaponsManager.secondaryFire = controller.GetDigitalControlPressed(Controls.ALT_FIRE);
		}

		// Engine Thruster //
		if(engineThruster){
			engineThruster.throttle = controller.GetAnalogControl(Controls.THROTTLE);
			engineThruster.reverse = controller.GetDigitalControl(Controls.DAMPENERS);
		}

		// Boost //
		if(boost){
			boost.activate = controller.GetDigitalControl(Controls.BOOST);
		}
	}
}
