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

	public float maxFuel = 10000;
	private float fuel;
	public Material fuelMeter;

	[System.NonSerialized]
	public StrafeManager strafeManager;
	[System.NonSerialized]
	public RotationManager rotationManager;
	[System.NonSerialized]
	public WeaponsManager weaponsManager;
	[System.NonSerialized]
	public EngineThruster engineThruster;
	[System.NonSerialized]
	public CameraManager cameraManager;
	[System.NonSerialized]
	public HeadTurn headTurn;
	[System.NonSerialized]
	public Boost boost;
	private bool isMaximized;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().inertiaTensor = new Vector3(55015.5f, 55015.5f, 55015.5f);
//		gameView = EditorWindow.focusedWindow;
//		isMaximized = gameView.maximized;
		strafeManager = GetComponent<StrafeManager>();
		rotationManager = GetComponent<RotationManager>();
		weaponsManager = GetComponent<WeaponsManager>();
		engineThruster = GetComponent<EngineThruster>();
		cameraManager = GetComponent<CameraManager>();
		headTurn = GetComponentInChildren<HeadTurn>();
		engineThruster.throttle = 0f;
		boost = GetComponent<Boost>();
		fuel = maxFuel;
	}

	void OnApplicationFocus(bool focusStatus) {
		PlayerInput.ResetAllAxes();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		DebugHUD.setValue ("left trigger", PlayerInput.PollAnalogControl (playerNumber, Controls.THROTTLE));
//		if (EditorApplication.isPaused) {
//			ParsedInput.controller[0].ResetAllAxes();
//		}

		if(isAi)return;

		// Strafe Manager //
		if(strafeManager && cameraManager && !cameraManager.headControl && fuel > 0) {
			fuel = fuel - Mathf.Abs(PlayerInput.PollAnalogControl(playerNumber, Controls.STRAFE_X) + PlayerInput.PollAnalogControl(playerNumber, Controls.STRAFE_Y)) / 2; // I'll probably change this to be more accurate
			strafeManager.xInput = PlayerInput.PollAnalogControl(playerNumber, Controls.STRAFE_X);
			strafeManager.yInput = PlayerInput.PollAnalogControl(playerNumber, Controls.STRAFE_Y);

			if (strafeManager.xInput == 0f && strafeManager.yInput == 0f) {
				engineThruster.isStrafing = false;
			} else {
				engineThruster.isStrafing = true;
			}
		}

		if (headTurn && cameraManager && cameraManager.headControl) {
			headTurn.xInput = PlayerInput.PollAnalogControl(playerNumber, Controls.HEADTURN_X);
			headTurn.yInput = PlayerInput.PollAnalogControl (playerNumber, Controls.HEADTURN_Y);
		}

		// Rotation Manager //
		if(rotationManager && fuel > 0){
			float xInput = PlayerInput.PollAnalogControl(playerNumber, Controls.LOOK_X);
			float yInput = PlayerInput.PollAnalogControl(playerNumber, Controls.LOOK_Y);
			float rotationInput = PlayerInput.PollAnalogControl(playerNumber, Controls.ROLL);
			if (PlayerInput.PollDigitalControl(playerNumber, Controls.QUICKTURN)) {
				xInput *= rotationManager.quickMultiplier;
				yInput *= rotationManager.quickMultiplier;
				rotationInput *= rotationManager.quickMultiplier;
			}
			fuel = fuel - Mathf.Abs(xInput + yInput + rotationInput) / 2;
            rotationManager.xInput = xInput;
            rotationManager.yInput = yInput;
//			rotationManager.xRightInput = ParsedInput.controller [playerNumber].RightStickX;
//			rotationManager.yRightInput = ParsedInput.controller [playerNumber].RightStickY;
            rotationManager.rotationInput = rotationInput;
		}

		// Weapons Manager //
		if(weaponsManager){
            weaponsManager.primaryFire = PlayerInput.PollDigitalControl(playerNumber, Controls.FIRE);
            weaponsManager.secondaryFire = PlayerInput.PollDigitalControlPressed(playerNumber, Controls.ALT_FIRE);
		}

		// Engine Thruster //
		if(engineThruster && fuel > 0){
			fuel -= PlayerInput.PollAnalogControl(playerNumber, Controls.THROTTLE);
            engineThruster.throttle = PlayerInput.PollAnalogControl(playerNumber, Controls.THROTTLE);
            engineThruster.reverse = PlayerInput.PollDigitalControl(playerNumber, Controls.DAMPENERS);
		}
		DebugHUD.setValue("Fuel", fuel);
		if (fuel >= 0) {
			fuelMeter.SetFloat ("_ProgressH", fuel / maxFuel);
		}

		// Boost //
		if(boost){
            boost.activate = PlayerInput.PollDigitalControl(playerNumber, Controls.BOOST);
		}
	}
}
