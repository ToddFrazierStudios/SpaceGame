@CustomEditor (FX_Input_Mgr)

class FX_Input_M_Editor extends Editor {
	
	function OnInspectorGUI () {
		EditorGUILayout.LabelField("ForceX 3D Radar EX: Version 1.2.1b", EditorStyles.boldLabel);
		EditorGUILayout.Space ();
		target.InputBindings = EditorGUILayout.EnumPopup ("Inputs:", target.InputBindings);
		
		if(target.InputBindings == 0){
			
		}else if(target.InputBindings == 1){
			FlightControls();
		}else if(target.InputBindings == 2){
			RadarInput();
		}
	}
}

function RadarInput(){
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Targeting Key Bindings", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");

EditorGUILayout.Space ();
EditorGUILayout.LabelField("All Targets", EditorStyles.boldLabel);
target.TargetNext = EditorGUILayout.EnumPopup ("Target Next:", target.TargetNext);
target.TargetNextKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetNextKM);
EditorGUILayout.Space ();

target.TargetPrev = EditorGUILayout.EnumPopup ("Target Previous:", target.TargetPrev);
target.TargetPrevKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetPrevKM);
EditorGUILayout.Space ();

target.TargetClosest = EditorGUILayout.EnumPopup ("Target Closest:", target.TargetClosest);
target.TargetClosestKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetClosestKM);
EditorGUILayout.Space ();		

EditorGUILayout.Space ();	
EditorGUILayout.LabelField("Hostile Targets", EditorStyles.boldLabel);
target.TargetNextH = EditorGUILayout.EnumPopup ("Target Next:", target.TargetNextH);
target.TargetNextHKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetNextHKM);
EditorGUILayout.Space ();

target.TargetPrevH = EditorGUILayout.EnumPopup ("Target Previous:", target.TargetPrevH);
target.TargetPrevHKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetPrevHKM);
EditorGUILayout.Space ();

target.TargetClosestH = EditorGUILayout.EnumPopup ("Target Closest:", target.TargetClosestH);
target.TargetClosestHKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetClosestHKM);
EditorGUILayout.Space ();

EditorGUILayout.Space ();	
EditorGUILayout.LabelField("Sub Components", EditorStyles.boldLabel);
target.TargetNextS = EditorGUILayout.EnumPopup ("Target Next:", target.TargetNextS);
target.TargetNextSKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetNextSKM);
EditorGUILayout.Space ();

target.TargetPrevS = EditorGUILayout.EnumPopup ("Target Previous:", target.TargetPrevS);
target.TargetPrevSKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetPrevSKM);
EditorGUILayout.Space ();

EditorGUILayout.Space ();	
EditorGUILayout.LabelField("Target Clearing", EditorStyles.boldLabel);
target.ClearTarget = EditorGUILayout.EnumPopup ("Clear Target:", target.ClearTarget);
target.ClearTargetKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.ClearTargetKM);
EditorGUILayout.Space ();

target.ClearSubC = EditorGUILayout.EnumPopup ("Clear Sub Comp:", target.ClearSubC);
target.ClearSubCKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.ClearSubCKM);

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();

EditorGUILayout.Space ();
EditorGUILayout.LabelField("Targeting List & Filters Key Bindings", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();
target.NAVList = EditorGUILayout.EnumPopup ("Display NAV List:", target.NAVList);
target.NAVListKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.NAVListKM);
EditorGUILayout.Space ();

target.TargetList = EditorGUILayout.EnumPopup ("Display Target List:", target.TargetList);
target.TargetListKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.TargetListKM);
EditorGUILayout.Space ();

target.FilterHostile = EditorGUILayout.EnumPopup ("Filter Hostile:", target.FilterHostile);
target.FilterHostileKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.FilterHostileKM);

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();

EditorGUILayout.Space ();
EditorGUILayout.LabelField("Display Key Bindings", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();		
target.Switch3D2D = EditorGUILayout.EnumPopup ("Switch 3D/2D:", target.Switch3D2D);
target.Switch3D2DKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.Switch3D2DKM);
EditorGUILayout.Space ();

target.ToggleIndicators = EditorGUILayout.EnumPopup ("Display Indicators:", target.ToggleIndicators);
target.ToggleIndicatorsKM = EditorGUILayout.EnumPopup ("Input Modifier:", target.ToggleIndicatorsKM);

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function FlightControls(){
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Flight Controls Input", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");

EditorGUILayout.Space ();
target.FlightControls = EditorGUILayout.EnumPopup ("Input:", target.FlightControls);
EditorGUILayout.Space ();

EditorGUILayout.EndVertical();

	if(target.FlightControls == 1){
		JoystickSettings();
	}else{

	}
}

function JoystickSettings(){
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Joystick Axis Settings", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

target.PitchE = EditorGUILayout.EnumPopup ("Pitch:", target.PitchE);
target.InvertPitch = EditorGUILayout.Toggle ("Invert:", target.InvertPitch);

EditorGUILayout.Space ();
target.YawE = EditorGUILayout.EnumPopup ("Yaw:", target.YawE);
target.InvertYaw = EditorGUILayout.Toggle ("Invert:", target.InvertYaw);

EditorGUILayout.Space ();
target.RollE = EditorGUILayout.EnumPopup ("Roll:", target.RollE);
target.InvertRoll = EditorGUILayout.Toggle ("Invert:", target.InvertRoll);

EditorGUILayout.Space ();
target.ThrottleE = EditorGUILayout.EnumPopup ("Throttle:", target.ThrottleE);
target.InvertThrottle = EditorGUILayout.Toggle ("Invert:", target.InvertThrottle);

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}