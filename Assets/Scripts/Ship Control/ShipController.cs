using UnityEngine;
using System.Collections;
using UnityEditor;

//needs to control:
// Strafe Manager
// Rotation Manager
// Weapons Manager
// Engine Thruster
// Boost

public class ShipController : MonoBehaviour {
	public bool isAi = false;
	[Range(0,3)]
	public int controllerNumber = 0;
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
	private EditorWindow gameView;
	// Use this for initialization
	void Start () {
		gameView = EditorWindow.focusedWindow;
		strafeManager = GetComponent<StrafeManager>();
		rotationManager = GetComponent<RotationManager>();
		weaponsManager = GetComponent<WeaponsManager>();
		engineThruster = GetComponent<EngineThruster>();
		engineThruster.throttle = 0f;
		boost = GetComponent<Boost>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isAi)return;
		if (EditorApplication.isPaused || EditorWindow.focusedWindow != gameView) {
			resetNextFrame = true;
		}
		if (resetNextFrame) {
			resetNextFrame = false;
			ParsedInput.controller[0].ResetAllAxes();
		}

		// Strafe Manager //
		if(strafeManager){
			doStrafeManagerInput();
		}

		// Rotation Manager //
		if(rotationManager){
			doRotationManagerInput();
		}

		// Weapons Manager //
		if(weaponsManager){
			doWeaponsManagerInput();
		}

		// Engine Thruster //
		if(engineThruster){
			doEngineThrusterInput();
		}

		// Boost //
		if(boost){
			doBoostInput();
		}
	}

	private void doStrafeManagerInput(){
		//reset input variables
		strafeManager.xInput = 0f;
		strafeManager.yInput = 0f;
		
		//controller
		if (useController) {
			strafeManager.xInput = ParsedInput.controller[controllerNumber].RightStickX;
			strafeManager.yInput = ParsedInput.controller[controllerNumber].RightStickY;
		}
		
		//keyboard
		if(useKeyboard){
			if (Input.GetKey(KeyCode.RightArrow)) {
				strafeManager.xInput = 1;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				strafeManager.xInput = -1;
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				strafeManager.yInput = -1;
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				strafeManager.yInput = 1;
			}
		}

		if (strafeManager.xInput == 0f && strafeManager.yInput == 0f) {
			engineThruster.isStrafing = false;
		} else {
			engineThruster.isStrafing = true;
		}
	}

	void doRotationManagerInput (){
		//reset input variables
		rotationManager.rotateRight = false;
		rotationManager.rotateLeft = false;
		rotationManager.xInput = 0f;
		rotationManager.yInput = 0f;
		rotationManager.xRightInput = 0f;
		rotationManager.yRightInput = 0f;
		//controller
		if (useController) {
			rotationManager.xInput = ParsedInput.controller [controllerNumber].LeftStickX;
			rotationManager.yInput = ParsedInput.controller [controllerNumber].LeftStickY;
			rotationManager.xRightInput = ParsedInput.controller [controllerNumber].RightStickX;
			rotationManager.yRightInput = ParsedInput.controller [controllerNumber].RightStickY;
			if (ParsedInput.controller [controllerNumber].LeftBumper)
				rotationManager.rotateLeft = true;
			if (ParsedInput.controller [controllerNumber].RightBumper)
				rotationManager.rotateRight = true;
		}
		//keyboard
		if (useKeyboard) {
			if (Input.GetKey (KeyCode.D)) {
				rotationManager.xInput = 1f;
			}
			else
				if (Input.GetKey (KeyCode.A)) {
					rotationManager.xInput = -1f;
				}
			if (Input.GetKey (KeyCode.W)) {
				rotationManager.yInput = 1f;
			}
			else
				if (Input.GetKey (KeyCode.S)) {
					rotationManager.yInput = -1f;
				}
			if (Input.GetKey (KeyCode.Q))
				rotationManager.rotateLeft = true;
			if (Input.GetKey (KeyCode.E))
				rotationManager.rotateRight = true;
		}
	}

	private void doWeaponsManagerInput(){
		//reset input variables
		weaponsManager.primaryFire = false;
		weaponsManager.secondaryFire = false;
		
		//controller
		if (useController) {
			if(ParsedInput.controller[controllerNumber].RightTrigger > 0)weaponsManager.primaryFire = true;
			if(ParsedInput.controller[controllerNumber].Xdown)weaponsManager.secondaryFire = true;
		}
		
		//keyboard
		if(useKeyboard){
			if(Input.GetKey (KeyCode.Space))weaponsManager.primaryFire = true;
			if(Input.GetKeyDown (KeyCode.X))weaponsManager.secondaryFire = true;
		}
	}

	private void doEngineThrusterInput(){
		//reset input variables
			engineThruster.throttle = 0f;
		//controller
		if (useController) {
			engineThruster.throttle = ParsedInput.controller[controllerNumber].LeftTrigger;
		}
		
		//keyboard
		if(useKeyboard){
			if(Input.GetKey(KeyCode.LeftShift))engineThruster.throttle = 1f;
		}
	}

	private void doBoostInput(){
		//reset input variables
		boost.activate = false;

		//controller
		if(useController){
			if(ParsedInput.controller[controllerNumber].LS) boost.activate = true;
		}

		//keyboard
		if(useKeyboard){
			if (Input.GetKey(KeyCode.R)) boost.activate = true;
		}
	}
}
