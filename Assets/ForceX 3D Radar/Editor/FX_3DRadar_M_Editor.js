@CustomEditor (FX_3DRadar_Mgr)

class FX_3DRadar_M_Editor extends Editor {
var Help_PS : boolean;
var Help_RSFX : boolean;
var Help_BLR : boolean;
var Help_RCS : boolean;
var Help_RLF : boolean;
var Help_RRS : boolean;
var Help_RAP : boolean;
var Help_RRTT : boolean;
var Help_RRTT1 : boolean;
var Help_HPad : boolean;
var Help_RRHS : boolean;
var Help_TIS : boolean;
var Help_TLI : boolean;
var Help_NAV : boolean;
var Help_DIA : boolean;
var Help_TB : boolean;
var Help_SB : boolean;
var Help_SSB : boolean;
var Help_CRL : boolean;

function OnInspectorGUI () {
EditorGUILayout.LabelField("ForceX 3D Radar EX: Version 1.2.1b", EditorStyles.boldLabel);

EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("* = Will be applied during Awake() / Start()", EditorStyles.boldLabel);
EditorGUILayout.LabelField("! = Information that must be entered manually", EditorStyles.boldLabel);
EditorGUILayout.Space ();
EditorGUILayout.Space ();

EditorGUILayout.LabelField("3D Radar Menu", EditorStyles.boldLabel);
target.RSI = EditorGUILayout.EnumPopup ("", target.RSI);

EditorGUILayout.Space ();
EditorGUILayout.Space ();

	switch(target.RSI){
		case 0:
			ProjectScale();
			CustomRenderingLayers();
			OculusRift();
			Components();
			SoundFX();
			RadarCS();
			RadarListFilters();
			BlindRadar();
		break;

		case 1:
			RadarRender();
		break;
		
		case 2:
			RadarIDSize();
		break;

		case 3:
			StatusBars();
		break;
	}
EditorGUILayout.Space ();
EditorGUILayout.Space ();
}

function ProjectScale(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Project Scale (Meters)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Project Scale : Default 1 unit = 1m", EditorStyles.boldLabel);
target.GameScale = EditorGUILayout.FloatField ("* Scale:", target.GameScale);

EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_PS = EditorGUILayout.Foldout(Help_PS," ?");
	if(Help_PS){
		EditorStyles.textField.wordWrap = true;
		var Help_PS_txt : String = 	"Project Scale is used to define the unit of measurement used in your project. \n\nDefault is 1 Unit = 1 meter\n";
		Help_PS_txt = EditorGUILayout.TextArea(Help_PS_txt);
		
	}
EditorGUILayout.Space ();

EditorGUILayout.EndVertical();
}

function CustomRenderingLayers(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Custom Rendering Layer Mask ", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");

	CRLInt = target.CRL.length;
	CRLInt = EditorGUILayout.IntField( "Custom Layers: ", CRLInt);

		if (CRLInt != target.CRL.length){//resize the array
			var arr0 : Array = new Array (target.CRL);
			var arr1 : Array = new Array (target.CustomRenderingL);
			arr0.length = CRLInt;
			arr1.length = CRLInt;
			target.CRL = arr0.ToBuiltin(int);
			target.CustomRenderingL = arr1.ToBuiltin(customRenderingL);
		}


		for (i = 0; i < target.CRL.length; i++){//display  all the elements of the array
			EditorGUILayout.Space ();
			target.CRL[i] = EditorGUILayout.LayerField ("Layer (" + (i+1).ToString() + ")", target.CRL[i]);
			target.CustomRenderingL[i] = EditorGUILayout.EnumPopup ("Layer Status (" + (i+1).ToString() + ")", target.CustomRenderingL[i]);
			EditorGUILayout.Space ();
		}

EditorGUILayout.Space ();

EditorGUI.indentLevel = 1;
Help_CRL = EditorGUILayout.Foldout(Help_CRL," ?");
	if(Help_CRL){
		EditorStyles.textField.wordWrap = true;
		var Help_CRL_txt : String = 	"Custom Rendering Layers can be used to assign any extra layers to be either included or excluded in rendering by the players main camera.\n";
		Help_CRL_txt = EditorGUILayout.TextArea(Help_CRL_txt);
		
	}
	
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function OculusRift(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Oculus Rift (Experimental)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("* Enable Oculus Rift:");
GUILayout.FlexibleSpace();
target.SetStatus[1] = EditorGUILayout.Toggle ("", target.SetStatus[1], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();


	if(target.SetStatus[1]){
		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 1;
		var Help_OVR_txt : String = 	"Oculus Rift support is experimental and not all features are supported. \n\nUse at your own discretion.";
		Help_OVR_txt = EditorGUILayout.TextArea(Help_OVR_txt);
	}
	
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function Components(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Radar Components", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("* Assign Custom Assets:");
GUILayout.FlexibleSpace();
target.SetStatus[18] = EditorGUILayout.Toggle ("", target.SetStatus[18], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

	if(target.SetStatus[18]){
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("* Radar Components", EditorStyles.boldLabel);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Atlas Material:");
		target.RadarAtlasMaterial = EditorGUILayout.ObjectField ("", target.RadarAtlasMaterial, Material, true, GUILayout.MaxWidth(140));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		
		EditorGUILayout.Space ();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Player:");
		target.Transforms[0] = EditorGUILayout.ObjectField ("", target.Transforms[0], Transform, true, GUILayout.MaxWidth(140));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
			if(target.SetStatus[1] == false){
			
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Player Camera:");
				target.Cameras[4] = EditorGUILayout.ObjectField ("", target.Cameras[4], Camera, true, GUILayout.MaxWidth(140));
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
	
			}else{
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Player Camera Right:");
				target.Cameras[4] = EditorGUILayout.ObjectField ("", target.Cameras[4], Camera, true, GUILayout.MaxWidth(140));
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Player Camera Left:");
				target.Cameras[5] = EditorGUILayout.ObjectField ("", target.Cameras[5], Camera, true, GUILayout.MaxWidth(140));
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
	
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Radar Camera:");
		target.Cameras[0] = EditorGUILayout.ObjectField ("", target.Cameras[0], Camera, true, GUILayout.MaxWidth(140));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Radar Object:");
		target.Transforms[1] = EditorGUILayout.ObjectField ("", target.Transforms[1], Transform, true, GUILayout.MaxWidth(140));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();		
		


}
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function SoundFX(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Sound Effects", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();
EditorGUILayout.LabelField("For best results, disable 3D Sound", EditorStyles.label);
EditorGUILayout.Space ();

//target.SetStatus[19] = EditorGUILayout.Toggle ("Assign Custom Sounds:", target.SetStatus[19]);
//EditorGUILayout.Space ();
	//if(target.SetStatus[19]){
EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Select Target Sound Effect:");
target.RadarSounds[0] = EditorGUILayout.ObjectField ("", target.RadarSounds[0], AudioClip, true, GUILayout.MaxWidth(140));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Clear Target Sound Effect:");
target.RadarSounds[1] = EditorGUILayout.ObjectField ("", target.RadarSounds[1], AudioClip, true, GUILayout.MaxWidth(140));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
		
EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Warning Sound Effect:");
target.RadarSounds[2] = EditorGUILayout.ObjectField ("", target.RadarSounds[2], AudioClip, true, GUILayout.MaxWidth(140));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.Space ();
	//}
	
EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Play Warning At Start:");
GUILayout.FlexibleSpace();
target.SetStatus[5] = EditorGUILayout.Toggle ("", target.SetStatus[5], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_RSFX = EditorGUILayout.Foldout(Help_RSFX," ?");
	if(Help_RSFX){
		EditorStyles.textField.wordWrap = true;
		var Help_RSFX_txt : String = 	"Play Warning At Start : Determines if the warning sound will be played at the start of the program if a hostile contact is in the players radar range. \n";
		Help_RSFX_txt = EditorGUILayout.TextArea(Help_RSFX_txt);
		
	}
EditorGUILayout.Space ();

EditorGUILayout.EndVertical();
EditorGUILayout.Space ();
}

function BlindRadar(){
EditorGUI.indentLevel = 0;
EditorGUILayout.LabelField("Blind Radar Settings", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");

EditorGUILayout.Space ();
EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Enable Blind Radar:");
GUILayout.FlexibleSpace();
target.SetStatus[2] = EditorGUILayout.Toggle ("", target.SetStatus[2], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.Space ();	
	if(target.SetStatus[2] == true){

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Radar Refresh (sec):");
		target.Timers[0] = EditorGUILayout.FloatField ("", target.Timers[0], GUILayout.MaxWidth(110));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Auto Reacquire Time (sec):");
		target.Timers[1] = EditorGUILayout.FloatField ("", target.Timers[1], GUILayout.MaxWidth(110));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		target.Layers[6] = EditorGUILayout.LayerField ("*Obstruction Layer:", target.Layers[6]);
		target.Layers[7] = EditorGUILayout.LayerField ("*Obstruction Layer:", target.Layers[7]);
		target.Layers[8] = EditorGUILayout.LayerField ("*Obstruction Layer:", target.Layers[8]);
	}

EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_BLR = EditorGUILayout.Foldout(Help_BLR," ?");
	if(Help_BLR){
		EditorStyles.textField.wordWrap = true;
		var Help_BLR_txt : String = 	"Blind Radar will allow radar contacts to be blocked from the players radar when there line of site is obstructed by an object. \n\nRadar Refresh : How often the radar will check for radar contacts. \n\nAuto Reacquire Time : If the players selected target is lost due to being obstructed by an object, this will determine how long the radar will wait to reacquire the target if it returns as a radar contact. \n\nObstruction Layers : Obstruction Layers are used to assign certain objects as radar obstructions that will block radar contacts from the players radar.\n\n";
		Help_BLR_txt = EditorGUILayout.TextArea(Help_BLR_txt);
		
	}
EditorGUILayout.Space ();

EditorGUILayout.EndVertical();
EditorGUILayout.Space ();
}

function RadarIDSize(){

IFFColors();
HUDPadding();
HUD_Display();
TargetIndicators();
TLI();
NAV();
DIA();
TargetBounds();
EditorGUILayout.Space ();
EditorGUILayout.Space ();
}

function RadarCS(){
EditorGUI.indentLevel = 0;
EditorGUILayout.LabelField("Radar Current State", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Radar Range (Meters)", EditorStyles.boldLabel);

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("* Radar Range:");
target.RadarRange[0] = EditorGUILayout.FloatField ("", target.RadarRange[0], GUILayout.MaxWidth(110));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();

EditorGUILayout.Space ();
EditorGUILayout.Space ();

target.RadarZoom = EditorGUILayout.EnumPopup ("Radar Zoom Level:", target.RadarZoom);

EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_RCS = EditorGUILayout.Foldout(Help_RCS," ?");
	if(Help_RCS){
		
		EditorStyles.textField.wordWrap = true;
		var Help_RCS_txt : String = 	"Radar Range : Defines how far from the player the radar can detect targets. \n\nRadar Zoom Level : This can be used to extend or reduce the effective range of the radar. \n";
		Help_RCS_txt = EditorGUILayout.TextArea(Help_RCS_txt);
		
	}
EditorGUILayout.Space ();

EditorGUILayout.EndVertical();
EditorGUILayout.Space ();
}

function RadarListFilters(){
EditorGUI.indentLevel = 0;
EditorGUILayout.LabelField("Radar List & Filters", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Target List Settings", EditorStyles.boldLabel);

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Display NAV List:");
GUILayout.FlexibleSpace();
target.EnableNAVList = EditorGUILayout.Toggle ("", target.EnableNAVList, GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Display Target List:");
GUILayout.FlexibleSpace();
target.EnableTargetList = EditorGUILayout.Toggle ("", target.EnableTargetList, GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Display Hostile Only:");
GUILayout.FlexibleSpace();
target.FilterHostile = EditorGUILayout.Toggle ("", target.FilterHostile, GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();



EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_RLF = EditorGUILayout.Foldout(Help_RLF," ?");
	if(Help_RLF){
		EditorStyles.textField.wordWrap = true;
		var Help_RLF_txt : String = 	"Display NAV List : This will enable a scrollable list that contains all available NAV points. \n\nDisplay Target List : This will enable a scrollable list that contains all available targets. \n\nDisplay Hostile Only : This will filter out all non-hostile radar contacts.\n";
		Help_RLF_txt = EditorGUILayout.TextArea(Help_RLF_txt);
		
	}
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
EditorGUILayout.Space ();
}

function RadarRender(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Radar Rendering Settings", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("* Render As Perspective:");
GUILayout.FlexibleSpace();
target.SetStatus[3] = EditorGUILayout.Toggle ("", target.SetStatus[3], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Render As 2D:");
GUILayout.FlexibleSpace();
target.SetStatus[7] = EditorGUILayout.Toggle ("", target.SetStatus[7], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();


	
	if(target.SetStatus[3]){
		target.Per2D_Y = EditorGUILayout.FloatField ("Perspective Camera Y Pos:", target.Per2D_Y);
	}

EditorGUILayout.Space ();

EditorGUILayout.LabelField("Radar Rendering Position Options", EditorStyles.boldLabel);
target.RadarPos = EditorGUILayout.EnumPopup("Position:", target.RadarPos);

EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_RRS = EditorGUILayout.Foldout(Help_RRS," ?");
	if(Help_RRS){
		EditorStyles.textField.wordWrap = true;
		var Help_RRS_txt : String = 	"Define radar render style, screen position, and render to texture settings. \n\nRender As Perspective : This will render the radar using perspective cameras giving the radar a sense of depth \n\nRender As 2D : This will change the radar to a more traditional 2D style mini map radar.\n\nPosition : Pre-defined settings for where the radar camera window will be located on the screen.\n\n• Custom : Will use the current radar camera Normalized View Port Rect settings. \n\n• Render To Texture : This will render the radar to a texture. \n\n• Anchor To : This will allow the radars screen position to be relative to an assigned objects world positon. This can be used instead of Render To Texture for non Unity Pro users. !!Limitations Apply!! \n\n";
		Help_RRS_txt = EditorGUILayout.TextArea(Help_RRS_txt);
	}

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();

	if(target.RadarPos == 5){
		RenderTex();
	}

	if(target.RadarPos == 6){
		RenderAnchor();
	}	
}

function RenderAnchor(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Radar Anchor", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();
target.RadarAnchorPos = EditorGUILayout.ObjectField ("Anchor To:", target.RadarAnchorPos, Transform, true);
EditorGUILayout.Space ();
target.RadarPosOffset = EditorGUILayout.Vector2Field("Offset (Range -1 to 1):",target.RadarPosOffset);


EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_RAP = EditorGUILayout.Foldout(Help_RAP," ?");
	if(Help_RAP){
		EditorStyles.textField.wordWrap = true;
		var Help_RAP_txt : String = 	"Anchoring can be a good way for non Unity Pro users to simulate the effect of the radar being integrated inside a cockpit without having to render the radar to a texture first. \n\nHowever anchoring the Radar's Rect to a world object has some limitations. \n\n1. The anchor object can not be outside the players FOV this will result in errors with the Radar's camera Rect window. \n\n2. After the Radar's camera Rect window reaches the edges of the screen the radar will begin to shrink until it is fully out side the screen, then it will generate errors. \n\nAnchor To : This is the object that the Radar will refrence for it's Viewport position. \n\nOffset : Defines the Radar's Rect position relative to its Anchor Position.  \n\n";
		Help_RAP_txt = EditorGUILayout.TextArea(Help_RAP_txt);
		
	}

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function RenderTex(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Render Texture Settings (Unity Pro)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

		if(target.SetStatus[3] == false){
			EditorGUILayout.LabelField("Texture Render Target", EditorStyles.boldLabel);
			target.Transforms[4] = EditorGUILayout.ObjectField ("Render Target:", target.Transforms[4], Transform, true);
			
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("Texture Resolution", EditorStyles.boldLabel);
			
			target.TextureSizeX = EditorGUILayout.EnumPopup("Width:",target.TextureSizeX);
			target.TextureSizeY = EditorGUILayout.EnumPopup("Height:",target.TextureSizeY);
			target.DBuffer = EditorGUILayout.EnumPopup("Depth Buffer:",target.DBuffer);
			
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("Texture Quality Settings", EditorStyles.boldLabel);
			target.AALevel = EditorGUILayout.EnumPopup("Anti-Aliasing:",target.AALevel);
			target._FilterMode = EditorGUILayout.EnumPopup("Filter Mode:",target._FilterMode);
			target.RTextureAnsio = EditorGUILayout.IntSlider("Ansio Level:", target.RTextureAnsio, 0, 9);
			
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("Texture Background Fill Color", EditorStyles.boldLabel);
			target.RenderSolidColor = EditorGUILayout.ColorField ("Color:", target.RenderSolidColor);
			
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("Radar Scaling Settings", EditorStyles.boldLabel);
			target.RIDSizeOverride[0] = EditorGUILayout.IntSlider("Radar ID Size (Pixels):", target.RIDSizeOverride[0], 32, 256);
			target.RIDSizeOverride[1] = EditorGUILayout.IntSlider("VDI Multiplier:", target.RIDSizeOverride[1], 1, 128);
			
		}else{
			EditorGUILayout.LabelField("Render to texture is disabled while", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("perspective rendering is active.", EditorStyles.boldLabel);
		}

EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_RRTT = EditorGUILayout.Foldout(Help_RRTT," ?");
	if(Help_RRTT){
		EditorStyles.textField.wordWrap = true;
		var Help_RRTT_txt : String = 	"Render Target : The object that the radar will be rendered to. \n\nRadar ID Size : Defines the size of the radar contacts ID size in pixels. \n\nVDI Multiplier : Increases the width of the radar contacts vertical directional indicator.\n";
		Help_RRTT_txt = EditorGUILayout.TextArea(Help_RRTT_txt);
	}

EditorGUILayout.Space ();
Help_RRTT1 = EditorGUILayout.Foldout(Help_RRTT1," Tip");
	if(Help_RRTT1){
		EditorStyles.textField.wordWrap = true;
		var Help_RRTT1_txt : String = 	"Try these settings for standard PC development. \n\nRender Texture Resolution = 512 / 1024 \nRadar ID Size = 128 \nRadar VDI Multiplier = 4";
		Help_RRTT1_txt = EditorGUILayout.TextArea(Help_RRTT1_txt);
	}

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function HUDPadding(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("HUD Indicators : Screen Edge Padding (Pixels)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.LabelField("HUD Target Selection Box", EditorStyles.boldLabel);

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Horizontal:");
target.EdgePadding[0] = EditorGUILayout.IntField ("", target.EdgePadding[0], GUILayout.MaxWidth(110));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Vertical:");
target.EdgePadding[1] = EditorGUILayout.IntField ("", target.EdgePadding[1], GUILayout.MaxWidth(110));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();


EditorGUILayout.Space ();
EditorGUILayout.LabelField("HUD Target Indicators", EditorStyles.boldLabel);

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Horizontal:");
target.EdgePadding[2] = EditorGUILayout.IntField ("", target.EdgePadding[2], GUILayout.MaxWidth(110));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Vertical:");
target.EdgePadding[3] = EditorGUILayout.IntField ("", target.EdgePadding[3], GUILayout.MaxWidth(110));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();


EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_HPad = EditorGUILayout.Foldout(Help_HPad," ?");
	if(Help_HPad){
		EditorStyles.textField.wordWrap = true;
		var Help_HPad_txt : String = 	"Padding defines how close to the edge of the screen HUD elements such as the Target Selection Box and Target Indicators can be. \n";
		Help_HPad_txt = EditorGUILayout.TextArea(Help_HPad_txt);
	}

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function HUD_Display(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Radar RID / HUD Settings", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("*Radar Indicators Mouse Selectable:");
GUILayout.FlexibleSpace();
target.SetStatus[17] = EditorGUILayout.Toggle ("", target.SetStatus[17], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("*HUD Indicators Mouse Selectable:");
GUILayout.FlexibleSpace();
target.SetStatus[16] = EditorGUILayout.Toggle ("", target.SetStatus[16], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();



EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("*Disable TSB Off Screen:");
GUILayout.FlexibleSpace();
target.SetStatus[12] = EditorGUILayout.Toggle ("", target.SetStatus[12], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("*Disable HUD TSB ID:");
GUILayout.FlexibleSpace();
target.HUDElements[3] = EditorGUILayout.Toggle ("", target.HUDElements[3], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("*Disable VDI:");
GUILayout.FlexibleSpace();
target.HUDElements[4] = EditorGUILayout.Toggle ("", target.HUDElements[4], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();


	if(!target.HUDElements[4]){
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("*Disable RID Base:");
		GUILayout.FlexibleSpace();
		target.HUDElements[5] = EditorGUILayout.Toggle ("", target.HUDElements[5], GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
	}
	
EditorGUILayout.Space ();
target.HUDOpacity[0] = EditorGUILayout.Slider("*HUD Opacity:",target.HUDOpacity[0], 0.0,1.0);

target.HUDOpacity[1] = EditorGUILayout.Slider("*RID Opacity:",target.HUDOpacity[1], 0.0,1.0);
	if(!target.HUDElements[4]){
		target.HUDOpacity[2] = EditorGUILayout.Slider("*VDI Opacity:",target.HUDOpacity[2], 0.0,1.0);
	}
	
EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_RRHS = EditorGUILayout.Foldout(Help_RRHS," ?");
	if(Help_RRHS){
		EditorStyles.textField.wordWrap = true;
		var Help_RRHS_txt : String = 	"Radar Indicators Mouse Selectable : If enabled will allow radar contacts in the radar to be selected with the mouse. \n\nHUD Indicators Mouse Selectable : If enabled will allow HUD Target Indicator contacts to be selected with the mouse. \n\nDisable TSB Off Screen : This will disable the Target Selection Box if the target leaves the players field of view, otherwise it will stick to the edge of the screen. \n\nDisable HUD TSB ID : Will disable the targets ID icon above the Target Selection Box. \n\nDisable VDI : Will disable all radar contacts Vertical Directional Indicator in the radar. \n\nDisable RID Base : Will disable all radar contacts base icon at the bottom of the VDI. \n\nOpactiy : Adjust the opacity amount for various elements of the radar system and HUD. \n\n\n";
		Help_RRHS_txt = EditorGUILayout.TextArea(Help_RRHS_txt);
	}
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function IFFColors(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("IFF Color Identifiers", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();		
target.ColorNeutral = EditorGUILayout.ColorField ("Neutral:", target.ColorNeutral);
target.ColorFriendly = EditorGUILayout.ColorField ("Friendly:", target.ColorFriendly);
target.ColorHostile = EditorGUILayout.ColorField ("Hostile:", target.ColorHostile);
target.ColorAbandoned = EditorGUILayout.ColorField ("Abandoned:", target.ColorAbandoned);
target.ColorOwned = EditorGUILayout.ColorField ("Player Owned:", target.ColorOwned);
target.ColorUnknown = EditorGUILayout.ColorField ("Unknown:", target.ColorUnknown);
target.ColorNAV = EditorGUILayout.ColorField ("NAV:", target.ColorNAV);
target.ColorObjective = EditorGUILayout.ColorField ("Objective:", target.ColorObjective);

EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function TLI(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Target Lead Indicator Settings (HUD)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Enable TLI:");
GUILayout.FlexibleSpace();
target.SetStatus[11] = EditorGUILayout.Toggle("", target.SetStatus[11], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.Space ();
	if(target.SetStatus[11]){
		EditorGUILayout.LabelField("Calculate Velocity VIA Physics (Rigidbody.velocioty)", EditorStyles.boldLabel);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target Physics:");
		GUILayout.FlexibleSpace();
		target.SetStatus[9] = EditorGUILayout.Toggle("", target.SetStatus[9], GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Player Physics:");
		GUILayout.FlexibleSpace();
		target.SetStatus[10] = EditorGUILayout.Toggle("", target.SetStatus[10], GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();		
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();		
		EditorGUILayout.LabelField("! Projectile Velocity", EditorStyles.boldLabel);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Auto Scale:");
		GUILayout.FlexibleSpace();
		target.SetStatus[21] = EditorGUILayout.Toggle("", target.SetStatus[21], GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();	
		
		EditorGUILayout.Space ();
		
		EditorGUILayout.BeginHorizontal();
		GUI.color = Color.red;
		EditorGUILayout.LabelField("	! Projectile Velocity:");
		target.ProjectileVelocity = EditorGUILayout.FloatField ("", target.ProjectileVelocity, GUILayout.MaxWidth(110));
		GUI.color = Color.white;
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	
		EditorGUILayout.Space ();
		
	}else{
		
	}
	
EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_TLI = EditorGUILayout.Foldout(Help_TLI," ?");
	if(Help_TLI){
		EditorStyles.textField.wordWrap = true;
		var Help_TLI_txt : String = 	"Target Lead Indicator is used to help determine where the player will need to shoot in order to hit the target. \n\nTarget Physics : If the target object is using rigidbody physics for movement then enable this to send its velocity to the radar, otherwise the targets velocity will be computed based on distance moved over time. \n\nPlayer Physics : Same as Target Physics. \n\nAuto Scale : This will automatically adjust the projectiles velocity to match with the projects scale. Disabled if you want to keep your values unchanged. \n\nProjectile Velocity : A Projectile Velocity is required in order to compute a intercept point for the Target Lead Indicator. This information must be passed in manually. \n\nFX_3DRadar_Mgr.ProjectileVelocity = Value; \n\n";
		Help_TLI_txt = EditorGUILayout.TextArea(Help_TLI_txt);
		
	}
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function TargetIndicators(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Global Target Indicator Settings (HUD)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Enable Target Indicators:");
GUILayout.FlexibleSpace();
target.SetStatus[13] = EditorGUILayout.Toggle ("", target.SetStatus[13], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

	if(target.SetStatus[13]){
		EditorGUILayout.Space ();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Enable On Screen:");
		GUILayout.FlexibleSpace();
		target.SetStatus[15] = EditorGUILayout.Toggle ("", target.SetStatus[15], GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("*Indicators As ID:");
		GUILayout.FlexibleSpace();
		target.SetStatus[14] = EditorGUILayout.Toggle ("", target.SetStatus[14], GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space ();
		target.HUDOpacity[4] = EditorGUILayout.Slider("*Indicators Opacity:",target.HUDOpacity[4], 0.0,1.0);
	}

EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_TIS = EditorGUILayout.Foldout(Help_TIS," ?");
	if(Help_TIS){
		EditorStyles.textField.wordWrap = true;
		var Help_TIS_txt : String = 	"Target Indicators are displayed as icons on the edge of the screen for any target that is in the players radar range. \n\nEnable On Screen : This will keep the Target Indicator icon active even if the target is inside the players FOV. \n\nIndicators As ID : This will replace the default Indicator icon with the targets Radar ID icon. \n\nIndicators Opacity : Will adjust the Indicators icon opacity. \n";
		Help_TIS_txt = EditorGUILayout.TextArea(Help_TIS_txt);
		
	}
EditorGUILayout.Space ();

EditorGUILayout.EndVertical();
}

function TargetBounds(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Global Target Bounds Settings (HUD)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Enable Bounds:");
GUILayout.FlexibleSpace();
target.HUDElements[2] = EditorGUILayout.Toggle("", target.HUDElements[2], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.Space ();		
	if(target.HUDElements[2]){
		
		EditorGUILayout.Space ();
		target.BoundsShow = EditorGUILayout.EnumPopup("*Bounds Display:",target.BoundsShow);
		target.BoundsSize = EditorGUILayout.EnumPopup("Bounds Size:",target.BoundsSize);
		
		if(target.BoundsSize == 0){
			target.BoundsCalculation = EditorGUILayout.EnumPopup("Bounds Calculation:",target.BoundsCalculation);
		}else{
			target.BoundsCalculation = 0;
		}

		
		EditorGUILayout.Space ();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Limit Screen Size:");
		GUILayout.FlexibleSpace();
		target.SetStatus[4] = EditorGUILayout.Toggle("", target.SetStatus[4], GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		if(target.SetStatus[4]){
			EditorGUILayout.LabelField("	Settings are based on Viewport space");
			target.MaxSize[0] = EditorGUILayout.Slider("	Max Width:",target.MaxSize[0], 0.0,1.0);
			target.MaxSize[1] = EditorGUILayout.Slider("	Max Height:",target.MaxSize[1], 0.0,1.0);
		}
		
		EditorGUILayout.Space ();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Padding:");
		target.BoundsPadding = EditorGUILayout.FloatField("",target.BoundsPadding, GUILayout.MaxWidth(110));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space ();
		target.HUDOpacity[3] = EditorGUILayout.Slider("*Opacity:",target.HUDOpacity[3], 0.0,1.0);
		EditorGUILayout.Space ();
	}
	
EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_TB = EditorGUILayout.Foldout(Help_TB," ?");
	if(Help_TB){
		
		EditorStyles.textField.wordWrap = true;
		var Help_TB_txt : String = 	"Target bounds will display a targets extents with indicators that display on the players HUD. \n\nBounds Display : Determines what rules will be used to display a target bounding indicators. \n\n• Show Only In Radar Range : This will only display the targets bounds indicators if the target is inside the players radar range. \n\n• Show Always After Contact : This will always show an objects bounding indicators as long as the object is considered discovered. \n\n• Custom : Allows user control on how bounds will be handled on a per target basis. \n\n Bounds Calculation : Determines which method will be used to calculate the objects bounds. \n\n• Simple : \n\n• Advanced : \n\nBounds Size : Determines the size of the targets bounding indicator on screen. \n\n• Dynamic Size : The bounds size will grow or shrink depending on the size of the object on the screen. \n\n• Static Size : The targets bounding indicator will be the same no matter how large or small the target appears on the screen. \n\nLimit Screen Size : Sets a Maximum and Minimum size the objects bounding indicator can occupy on the screen. The calculations are performed in Viewport Space. \n\nPadding : Determines the relative size of the bounds in relation to the objects actual bounding size. \n\nOpacity : Determines how opaque the bounds indicators will be. \n\n\n";
		Help_TB_txt = EditorGUILayout.TextArea(Help_TB_txt);
		
	}
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function NAV(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("NAV & POI Settings", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("* Display NAV In Radar:");
GUILayout.FlexibleSpace();
target.HUDElements[1] = EditorGUILayout.Toggle ("", target.HUDElements[1], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();
		
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("	Arrival Distance:");
target.NavDistance[0] = EditorGUILayout.FloatField ("", target.NavDistance[0], GUILayout.MaxWidth(110));
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
		
EditorGUILayout.Space ();

target.NAVArrival = EditorGUILayout.EnumPopup ("NAV Arrival Action:", target.NAVArrival);

/*
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Home POI", EditorStyles.boldLabel);
target.SetStatus[20] = EditorGUILayout.Toggle ("Enable Home POI:", target.SetStatus[20]);
	if(target.SetStatus[20]){
		target.HUDElements[6] = EditorGUILayout.Toggle ("Display Home In Radar:", target.HUDElements[6]);
		target.HUDLoc = EditorGUILayout.ObjectField ("Home:", target.HUDLoc, Transform, true);
	}
*/
EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_NAV = EditorGUILayout.Foldout(Help_NAV," ?");
	if(Help_NAV){
		
		EditorStyles.textField.wordWrap = true;
		var Help_NAV_txt : String = 	"Enable NAV In Radar : This will display the current NAV in the players radar. \n\nArrival Distance : The distance at which the NAV is considered to be arrived at. \n\nNAV Arrival Action : Determines what action will be performed once the player has reached a NAV location. \n\n• Do Nothing : No actions will be taken. \n\n• Go To Next NAV : This will automatically switch to the next NAV in the list. \n\n• Go To Next NAV (Destroy Current) : This will automatically switch to the next NAV in the list and remove the current NAV. \n";
		Help_NAV_txt = EditorGUILayout.TextArea(Help_NAV_txt);
		
	}
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function DIA(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("Directional Indicator Arrow Settings (HUD)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Enable Directional Indicator:");
GUILayout.FlexibleSpace();
target.HUDElements[0] = EditorGUILayout.Toggle ("", target.HUDElements[0], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();

EditorGUILayout.Space ();
	if(target.HUDElements[0]){
	
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Radius:");
				target.DIARadius = EditorGUILayout.FloatField ("", target.DIARadius, GUILayout.MaxWidth(110));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}	

	
EditorGUILayout.Space ();
EditorGUI.indentLevel = 1;
Help_DIA = EditorGUILayout.Foldout(Help_DIA," ?");
	if(Help_DIA){
		
		EditorStyles.textField.wordWrap = true;
		var Help_DIA_txt : String = 	"The Directional Indicator Arrow is an icon that rotates around the center of the players screen at a set distance that points towards the currently selected target. \n\nIf the target is inside the Directional Indicator Arrows arc then the arrow will be disabled. \n\nRadius : Defines how far from the center of the screen the arrow will be located. \n";
		Help_DIA_txt = EditorGUILayout.TextArea(Help_DIA_txt);
		
	}
EditorGUILayout.Space ();
EditorGUILayout.EndVertical();
}

function StatusBars(){
EditorGUI.indentLevel = 0;
EditorGUILayout.Space ();
EditorGUILayout.Space ();
EditorGUILayout.LabelField("*Status Bars (HUD)", EditorStyles.boldLabel);
EditorGUILayout.BeginVertical ("HelpBox");
EditorGUILayout.Space ();
EditorGUILayout.Space ();

EditorGUILayout.Space ();

EditorGUILayout.BeginHorizontal();
EditorGUILayout.LabelField("Enable Status Bars:");
GUILayout.FlexibleSpace();
target.SetStatus[20] = EditorGUILayout.Toggle ("", target.SetStatus[20], GUILayout.MaxWidth(110));
EditorGUILayout.EndHorizontal();


	if(target.SetStatus[20]){
		BarsInt = target.IsTargetBar.length;
		BarsInt = EditorGUILayout.IntField( "Status Bars: ", BarsInt);
		EditorGUILayout.Space ();
		target.BarBGColor = EditorGUILayout.ColorField ("Background Color:", target.BarBGColor);
		}
		
		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 1;
		Help_SB = EditorGUILayout.Foldout(Help_SB," ?");
			if(Help_SB){
				
				EditorStyles.textField.wordWrap = true;
				var Help_SB_txt : String = 	"Status Bars are used to display a visual representation of a given value 0-1. \n\nStatus Bares are stored as a Static variable giving you direct access to their properties from any script without having to create a local reference to them first. The information needed to access any given Status Bar's material properties are displayed under the Name field on each Status Bar editor window. \n\nFX_3DRadar_Mgr.BarMaterial[n]; \n\nStatus Bars : Defines the number of Status Bars to be drawn. \n\nBackground Color : Defines the background color for all default style Status Bars. \n\nBar Type \n• Create New : Creates a new status bar that can be displayed on the players screen as a HUD element.\n• Manage Existing : Manages an existing object that has been pre assigned with a material. This is useful for accessing an objects material that is part of a console or cockpit.\n\nName : A user definable description to help distinguish one bar from another in the inspector and hierarchy views. \n\nCustom Material : Allows the use of a user definable material in the inspector. \n\nInherit TSB Status : This will force the Status Bar to use the Target Selection Box display enable/disable status. \n\nAnchor Position : Defines where on the screen the Status Bar should be located. \n\nOrientation : Defines the Status Bar's screen orientation and fill direction based on the default Status Bar material. \n\nSize : Defines the Status Bar's size on the screen in pixels. If a custom textured Status Bar is defined in a material it is recommended to set the Size to the size of the texture in the material. \n\nOffset : Defines the Status Bars position relative to its Anchor Position. \n\n\n";
				Help_SB_txt = EditorGUILayout.TextArea(Help_SB_txt);
				
			}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();
		
		if (target.SetStatus[20] && BarsInt != target.IsTargetBar.Length){//resize the array
			var arr0 : Array = new Array(target.FoldOut);
			var arr1 : Array = new Array (target.IsTargetBar);
			var arr2 : Array = new Array(target.BarAnchor);
			var arr3 : Array = new Array(target.BarOffset);
			var arr4 : Array = new Array(target.BarSize);
			var arr5 : Array = new Array(target.BarColor);
			var arr6 : Array = new Array(target.BarName);
			var arr7 : Array = new Array(target.BarDirection);
			var arr8 : Array = new Array(target.BarCustom);
			var arr9 : Array = new Array(target.BarMatTemp);
			var arr10 : Array = new Array(target.SBRift);
			var arr11 : Array = new Array(target.StatusBarType);
			var arr12 : Array = new Array(target.StatusBar);
			
			arr0.length = BarsInt;
			arr1.length = BarsInt;
			arr2.length = BarsInt;
			arr3.length = BarsInt;
			arr4.length = BarsInt;
			arr5.length = BarsInt;
			arr6.length = BarsInt;
			arr7.length = BarsInt;
			arr8.length = BarsInt;
			arr9.length = BarsInt;
			arr10.length = BarsInt;
			arr11.length = BarsInt;
			arr12.length = BarsInt;
			
			target.FoldOut = arr0.ToBuiltin(boolean);
			target.IsTargetBar = arr1.ToBuiltin(boolean);
			target.BarAnchor = arr2.ToBuiltin(AnchorPos);
			target.BarOffset = arr3.ToBuiltin(Vector2);
			target.BarSize = arr4.ToBuiltin(Vector2);
			target.BarColor = arr5.ToBuiltin(Color);
			target.BarName = arr6.ToBuiltin(String);
			target.BarDirection = arr7.ToBuiltin(barDirection);
			target.BarCustom = arr8.ToBuiltin(boolean);
			target.BarMatTemp = arr9.ToBuiltin(Material);
			target.SBRift = arr10.ToBuiltin(sbRift);
			target.StatusBarType = arr11.ToBuiltin(statusBarType);
			target.StatusBar = arr12.ToBuiltin(Transform);
		}
	
	
		if(target.SetStatus[20]){
		for (var i : int = 0; i < target.IsTargetBar.Length; i++){//display  all the elements of the array
			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical ("HelpBox");		
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("Status Bar " + i.ToString(), EditorStyles.boldLabel);
			EditorGUILayout.Space ();
			target.BarName[i] = EditorGUILayout.TextField ("Name:", target.BarName[i]);
			
			EditorGUILayout.Space ();
			target.StatusBarType[i] = EditorGUILayout.EnumPopup ("Bar Type:", target.StatusBarType[i]);
			EditorGUILayout.Space ();

			target.FoldOut[i] = EditorGUILayout.Foldout(target.FoldOut[i],"  FX_3DRadar_Mgr.BarMaterial[" + i.ToString() + "]");
			if(target.FoldOut[i]){

					EditorGUILayout.Space ();
					if(target.StatusBarType[i] == 0){
						EditorGUILayout.Space ();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Custom Material:");
						GUILayout.FlexibleSpace();
						target.BarCustom[i] = EditorGUILayout.Toggle ("", target.BarCustom[i], GUILayout.MaxWidth(110));
						EditorGUILayout.EndHorizontal();
						
						if(target.BarCustom[i]){
							target.BarMatTemp[i] = EditorGUILayout.ObjectField ("Material:", target.BarMatTemp[i], Material, true);
						}
						
						EditorGUILayout.Space ();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Inherit TSB Status:");
						GUILayout.FlexibleSpace();
						target.IsTargetBar[i] = EditorGUILayout.Toggle ("", target.IsTargetBar[i], GUILayout.MaxWidth(110));
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.Space ();
							
						target.BarAnchor[i] = EditorGUILayout.EnumPopup ("Anchor Position:", target.BarAnchor[i]);
						target.BarDirection[i] = EditorGUILayout.EnumPopup ("Orientation:", target.BarDirection[i]);
						
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
						EditorGUILayout.LabelField("Oculus Rift Options", EditorStyles.boldLabel);
						EditorGUILayout.BeginVertical ("HelpBox");		
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
						if(target.SetStatus[1]){
							target.SBRift[i] = EditorGUILayout.EnumPopup ("Render To:", target.SBRift[i]);
						}else{
							EditorGUILayout.LabelField("Oculus Rift Is Disabled", EditorStyles.boldLabel);
						}
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
						EditorGUILayout.EndVertical();
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();

						target.BarSize[i] = EditorGUILayout.Vector2Field ("Size (Pixels):", target.BarSize[i]);
						EditorGUILayout.Space ();
						target.BarOffset[i] = EditorGUILayout.Vector2Field ("Offset (Pixels):", target.BarOffset[i]);
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
						if(!target.BarCustom[i]){
							target.BarColor[i] = EditorGUILayout.ColorField ("Color:", target.BarColor[i]);
						}
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
					}else{
						target.StatusBar[i] = EditorGUILayout.ObjectField ("Object:", target.StatusBar[i], Transform, true);
					}
				}
				EditorGUILayout.Space ();
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space ();
		}
	}
}
}