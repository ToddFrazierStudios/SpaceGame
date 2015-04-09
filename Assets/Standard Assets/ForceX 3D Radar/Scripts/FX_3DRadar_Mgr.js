#pragma strict
import System.Collections.Generic;
import System.Linq;

//Inspector
enum rsi{RadarSetup, RadarRenderingSettings, RadarHUDDisplaySettings, StatusBarSettings}
var  RSI : rsi;
var FoldOut : boolean[];
var FoldOut1 : boolean[];

//ENUM
enum statusBarType {CreateNew, ManageExisting}
var StatusBarType : statusBarType[] = new statusBarType[0];

enum customRenderingL {Include, Exclude}
var CustomRenderingL : customRenderingL[] = new customRenderingL[0];
var CRL : int[] = new int[0];

enum radarZoom {Normal, Zoom_In_2X, Zoom_In_X4, Boost_1_5, Boost_2}
var RadarZoom : radarZoom;

enum radarPos {CustomPosition, TopLeft, TopRight, BottomLeft, BottomRight, RenderToTextureUnityPro, AnchorTo}
var RadarPos : radarPos;

enum dBuffer {None, _16_bit, _24_bit}
var DBuffer : dBuffer;

enum textureSizeX {_16,_32,_64,_128,_256,_512,_1024,_2048}
var TextureSizeX : textureSizeX = 5;

enum textureSizeY {_16,_32,_64,_128,_256,_512,_1024,_2048}
var TextureSizeY : textureSizeY = 5;

enum aaLevel {None, Samples_2, Samples_4, Samples_8}
var AALevel : aaLevel;

enum filterMode {Point, Bilinear, Trilinear}
var _FilterMode : filterMode;

enum boundsShow {Custom, DisplayOnlyInRadarRange, DisplayAlwaysAfterContact}
var BoundsShow : boundsShow;

enum boundsSize {DynamicSize, StaticSize}
var BoundsSize : boundsSize;

enum  boundsCalculation {Simple, Advanced}
var BoundsCalculation  : boundsCalculation;

enum AnchorPos {RelativeToTSB, BottomLeft, BottomCenter, BottomRight, MidleLeft, MidleCenter, MidleRight, TopLeft, TopCenter, TopRight}
enum barDirection {HorizontalLeftRight, HorizontalRightLeft, VerticalBottomTop, VerticalTopBottom}
enum sbRift {MainRightCamera, LeftCamera, Both}

enum navArrival {DoNothing, GoToNextNAV, GoToNextNAVDestroyCurrent}
var NAVArrival : navArrival;

//Components
var RadarAtlasMaterial : Material;

var Cameras : Camera[] = new Camera[6]; // 0 = Radar Camera Perspective, 1 = Radar Camera Ortho, 2 = HUD Camera 1, 3 = HUD Camera 2, 4 = Player Camera Main / Right, 5 = Player Camera Left
var Transforms : Transform[] = new Transform[5]; // 0 = Player, 1 = Radar, 2 = Player Camera Main / Right, 3 = Player Camera Left, 4 = Render Target
var NavList : Transform[] = new Transform[0];
var HUDLoc : Transform;
var RadarSounds : AudioClip[] = new AudioClip[3]; // 0 = Select, 1 = Clear, 2 = Warning

var Layers : int[] = new int[9]; // 0 = RadarLayer1, 1 = RadarLyaer2, 2 = HUD Layer 1, 3 = HUD Layer 2, 4 = RadarContactLayer, 5 = Obstruction Mask, 6,7,8 = Obstruction Layer
var DIARadius : int = 150;

var RadarRange : float[] = [100.0,0.0,0.0,0.0]; // 0 = Radar Range, 1 = Radar Range Square, 2 = Radar Game Scale, 3 = Radar Local Scale
var Timers : float[] = [1.0, 5.0]; // 0 = Radar Update Time, 1 = Reaquire Target Time
var VDIScale : float;
var GameScale : float = 1;
var LocalTime : float;
var Per2D_Y : float = 1.25;

var SetStatus : boolean[] = [true, false, false, false, true, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false]; // 0 = Radar Enabled, 1 = Rift Enabled, 2 = Blind Radar Enabled, 3 = Perspective Enabled, 4 = Set Bounds Screen Size Limit, 5 = Play Warning Sound @ Start, 6 = Target Is Front, 7 = Enable 2D Radar, 8 = Is 2D Mode, 9 = Use Physics, 10 = Player Use Physics, 11 = Enable Target Lead, 12 = Enable HUD TSB Offscreen, 13 = Enable HUD Targets Indicators (Every Target), 14 = Indicators As ID, 15 = Indicators On Screen 16 = Indicators Clickable 17 = Radar ID Clickable, 18 = Assign Custom Assets, 19 = Assign Custom Sounds, 20 = Enable Status Bars, 21 = Auto Scale Projectile Velocity, 22 = Not Defined, 23 = Not Defined, 24 = Not Defined, 25 = Not Defined
var HUDElements : boolean[] = new boolean[7]; // 0 = Enable Directional Arrow, 1 = Enable NAV In Radar, 2 = Enable Bounds, 3 = Disable HUD TSB RID, 4 = Disable Radar VDI, 5 = Disable Radar VDI Base, 6 = Enable Locator In Radar
var HUDEnabled : boolean[] = new boolean[4]; // 0 = Main HUD, 1 = SC TSB, 2 = TLI, 3 = Directional Indicator Arrow

var RadarAnchorPos : Transform;
var RadarPosOffset : Vector2;

var StatusBar : Transform[] = new Transform[0];
var StatusBar2 : Transform[] = new Transform[0];
var StaticBar : GameObject[] = new GameObject[0];
var IsTargetBar : boolean[] = new boolean[0];
var BarAnchor : AnchorPos[] = new AnchorPos[0];
var BarDirection : barDirection[] = new barDirection[0];
var BarOffset : Vector2[] = new Vector2[0];
var BarSize : Vector2[] = new Vector2[0];
var BarColor : Color[] = new Color[0];
var BarBGColor : Color;
var BarName : String[] = new String[0];
var BarCustom : boolean[] = new boolean[0];
var BarMatTemp : Material[] = new Material[0];
var SBRift : sbRift[] = new sbRift[0];
static var BarMaterial : Material[] = new Material[0];

var EnableTargetList : boolean;
var EnableNAVList : boolean;
var FilterHostile : boolean;

var FXFM : FX_Faction_Mgr;
private var KeyAssign : FX_Input_Mgr;
private var DIAdisableAngle : int;
private var CurrentZoom : int = -1;
var ScreenSize : Vector2;
var VFBounds : float[] = new float[2];
private var scrollPosition : Vector2;
private var Camera3DPos : Vector3[] = new Vector3[2];
private var RigidBodys : Rigidbody[] = new Rigidbody[2];
static var PlayerPos : Vector3;

var TargetPos : Vector3;
var RelPos : Vector3;

//Render To Texture Settings
var RTextureAnsio : int;
var RenderSolidColor : Color;

//Bounds
var BoundsPadding : float = 1.0;
var MaxSize : float[] = [0.08, 0.2];

//Target Lead Indicator Settings
var ProjectileVelocity : float = 1;
private var previousPos : Vector3[] = new Vector3[2];

//NAV Settings
var NavDistance : float[] = [1.0, 0.0]; // 0 = Arival Distance, 1 = Current Distance
private var CurNav : int;

//Target & Targeting List
var SelectedTarget : Transform[] = new Transform[2]; // 0 = Current Target, 1 = Current SubComponent Target
private var SelectedTargetRID : FX_3DRadar_RID; // Target Radar ID Properties
private var TargetListAll : List.<Transform> = new List.<Transform>();
private var TargetListFriendly : List.<Transform> = new List.<Transform>();
private var TargetListNeutral : List.<Transform> = new List.<Transform>();
private var TargetListHostile : List.<Transform> = new List.<Transform>();
private var TargetListOwned : List.<Transform> = new List.<Transform>();
private var TargetListAband : List.<Transform> = new List.<Transform>();
private var SubComponentList : List.<Transform> = new List.<Transform>();
private var ThisCurrentTarget : Vector2 = Vector2.zero;
private var ThisClass : int[] = new int[3];
var ThisDistance : float;
var HostileCount : int;

// HUD GUI Settings
var ColorAbandoned : Color = Color.magenta;
var ColorNeutral : Color = Color.white;
var ColorFriendly : Color = Color(0,.3,1,1);
var ColorHostile : Color = Color.red;
var ColorUnknown : Color = Color.gray;
var ColorOwned: Color = Color.green;
var ColorNAV : Color = Color(1, .5, 0, 1);
var ColorObjective : Color = Color(1.0, 1.0, 0.1, 1);
var EdgePadding : int[] = [32, 45, 8, 8];
var HUDOpacity : float[] =[1.0, 1.0, 1.0, 0.5, 0.25]; // 0 = HUD, 1 = RID, 2 = VDI, 3 = Bounds, 4 = Selectors
var RIDSizeOverride : int[] = [128,4];

private var HUD_NAV : Transform[] = new Transform[2];
private var HUD_TLI : Transform[] = new Transform[2];
private var HUD_TSB : Transform[] = new Transform[2];
private var HUD_TSB_ID : Transform[] = new Transform[2];
private var HUD_SC_TSB : Transform[] = new Transform[2];
private var HUD_DIA : Transform[] = new Transform[2];
private var HUD_Loc : Transform[] = new Transform[2];
private var HUD_Bar : Transform[] = new Transform[3];
var Radar_TSB : Transform;

function Awake(){
Layers[0] = LayerMask.NameToLayer ("Radar_Layer_1");
Layers[1] = LayerMask.NameToLayer ("Radar_Layer_2");
Layers[2] = LayerMask.NameToLayer ("HUD_Layer_1");
Layers[3] = LayerMask.NameToLayer ("HUD_Layer_2");
Layers[4] = LayerMask.NameToLayer ("Radar_Contact");
		
	if(!SetStatus[18]){
		RadarAtlasMaterial = Resources.Load("Materials/Radar_Atlas_Mat") as Material;
		
		Transforms[0] = GameObject.Find("_Player").transform;
		Transforms[1] = GameObject.Find("_Radar2013").transform;
		Cameras[0] = GameObject.Find("_RadarCamera").GetComponent.<Camera>();
		
		if(!SetStatus[1]){
			Cameras[4] = GameObject.Find("_PlayerCamera").GetComponent.<Camera>();
		}else{
			Cameras[4] = GameObject.Find("CameraRight").GetComponent.<Camera>();
			Cameras[5] = GameObject.Find("CameraLeft").GetComponent.<Camera>();
		}
	}
	//if(!SetStatus[19]){
		//RadarSounds[0] = Resources.Load("Sounds/Target_Select") as AudioClip;
		//RadarSounds[1] = Resources.Load("Sounds/Target_Clear") as AudioClip;
		//RadarSounds[2] = Resources.Load("Sounds/Radar_Warning") as AudioClip;
	//}

FXFM = GetComponent(FX_Faction_Mgr);
KeyAssign = GetComponent(FX_Input_Mgr);
transform.position = Vector3.zero;
gameObject.AddComponent(AudioSource);

NavDistance[1] = ((NavDistance[0] / GameScale) * (NavDistance[0] / GameScale));
Layers[5] = 1 << Layers[6] | 1 << Layers[7] | 1 << Layers[8];

var RadarTags : GameObject = new GameObject("RadarTags");
RadarTags.transform.parent = transform;

var BC : Transform = new GameObject("BoundsCorners").transform;
BC.transform.parent = transform;

	if(SetStatus[9]){
		RigidBodys[0] = Transforms[0].GetComponent.<Rigidbody>();
	}

	if(RadarPos != 5){
		RIDSizeOverride[0] = 32;
	}
/***********************************************************************************/
														//Cache Player Camera
/***********************************************************************************/
Transforms[2] = Cameras[4].transform;
	if(SetStatus[1]){
		if(Cameras[5]){
			Transforms[3] = Cameras[5].transform;
			Cameras[5].cullingMask = ~(1 << Layers[0] | 1 << Layers[1] | 1 << Layers[2] | 1 << Layers[3]);
			SetCustomRenderingLayers(Cameras[5]);
		}
		Cameras[4].cullingMask = ~(1 << Layers[0] | 1 << Layers[1] | 1 << Layers[2] | 1 << Layers[3]);
	}else{
		Cameras[4].cullingMask = ~(1 << Layers[0] | 1 << Layers[1] | 1 << Layers[2]);
	}
	SetCustomRenderingLayers(Cameras[4]);

/***********************************************************************************/
														//Cache & Configure Radar Camera
/***********************************************************************************/
Cameras[0].renderingPath = RenderingPath.Forward;
Cameras[0].cullingMask = 1 << Layers[0];
Cameras[0].depth = Cameras[4].depth + 1;
Cameras[0].gameObject.layer = Layers[1];
Cameras[1] = new GameObject("RadarCameraO").AddComponent(Camera).GetComponent.<Camera>();
Cameras[1].rect = Cameras[0].rect;
Cameras[1].orthographic = true;
Cameras[1].orthographicSize = 0.5;
Cameras[1].depth = Cameras[0].depth + 1;
Cameras[1].transform.position = Cameras[0].transform.position;
Cameras[1].transform.eulerAngles = Cameras[0].transform.eulerAngles;
Cameras[1].clearFlags = CameraClearFlags.Depth;
Cameras[1].farClipPlane = 2;
Cameras[1].transform.parent = Cameras[0].transform;

Camera3DPos[0] = Cameras[0].transform.localPosition;
Camera3DPos[1] = Cameras[0].transform.localEulerAngles;

	if(SetStatus[3]){
		Cameras[1].cullingMask = 1 << Layers[1];
		Cameras[0].orthographic = false;
	}else{
		Cameras[0].enabled = false;
		Cameras[1].cullingMask = 1 << Layers[1] | 1 << Layers[0];
	}

/***********************************************************************************/
														//Cache & Configure Radar Object
/***********************************************************************************/
Transforms[1].gameObject.layer = Layers[0];

CreateHUD();
DisableHUD();
RadarSetup();
//ResetScale();
}

function SetCustomRenderingLayers(ThisCamera : Camera){
	for (var i : int = 0; i < CustomRenderingL.Length; i++){
		if(CustomRenderingL[i] == 0){
		 	ThisCamera.cullingMask += (~1<<CRL[i]);
		}else if(CustomRenderingL[i] == 1){
			ThisCamera.cullingMask += (1<<CRL[i]);
		}
	}
}

/***********************************************************************************/
																	//Late Update
/***********************************************************************************/
function LateUpdate(){
LocalTime = Time.time;
PlayerPos = Transforms[0].position;

	if(RadarPos == 6){//Make the radar's screen position relative to a object viewport position.
		var APSP : Vector3 = Cameras[4].WorldToViewportPoint(RadarAnchorPos.position); //Radar Anchor point viewport position
		var CRect : Vector2 = Vector2(Cameras[1].rect.width, Cameras[1].rect.height); //Cache the Radar cameras Rect
		Cameras[1].rect = Rect( 1 - ((-APSP.x + 1) - RadarPosOffset.x + CRect.x - (CRect.x * 0.5)), 1 - ((-APSP.y + 1) - RadarPosOffset.y + CRect.y) + (CRect.y * 0.5), CRect.x, CRect.y);
	}

	if(SetStatus[0]){
		TargetingCommands();
		Radar3D_2D();
		UpdateNAV();

		if(CurrentZoom != RadarZoom){
			SetRadarScaleZoom();
		}
		
		if(SetStatus[5] && TargetListHostile.Count > 0){
			PlayWarningSound();
			SetStatus[5] = false;
		}
		
		if(SelectedTarget[0]){
			TargetPos = SelectedTarget[0].position;
			RelPos = (PlayerPos - TargetPos);
			ThisDistance = RelPos.magnitude;
			RIDS();
		}else if(HUDEnabled[0]){
			DisableHUD();
		}

		if(ScreenSize.y != Screen.height || ScreenSize.x != Screen.width){ // if the screen size changes reset the scale of the HUD & Radar UI elements
			ResetScale();
		}
	}
}

/***********************************************************************************/
																//NAV Late Update
/***********************************************************************************/
function CreateNAV(MyNAV : Transform[]){
NavList = new Transform[MyNAV.Length];
	for(var i : int = 0; i < MyNAV.Length; i++){
		NavList[i] = MyNAV[i];
	}
CurNav = 0;
NavList[CurNav].gameObject.AddComponent(FX_3DRadar_RID).IsNAV = true;
HUD_NAV[0].GetComponent.<Renderer>().enabled = true;
	if(SetStatus[1]){
		HUD_NAV[1].GetComponent.<Renderer>().enabled = true;
	}
}

function UpdateNAV(){
	if(NavList.Length > 0){
		var newScreenPos : Vector3 = IndicatorPositions(NavList[CurNav].position,2,4);
		HUD_NAV[0].position = newScreenPos;
		
		if(SetStatus[1]){
			newScreenPos = IndicatorPositions(NavList[CurNav].position,3,5);
			HUD_NAV[1].position = newScreenPos;
		}

		if(NAVArrival != 0 && (NavList[CurNav].position - PlayerPos).sqrMagnitude < NavDistance[1]){
			
			if(NAVArrival == 2){
				NavList[CurNav].GetComponent(FX_3DRadar_RID).DestroyThis();
			}
			
			if(CurNav == (NavList.Length - 1)){
			 	NavList = new Transform[0];
			 	HUD_NAV[0].GetComponent.<Renderer>().enabled = false;
			 	if(SetStatus[1]){
			 		HUD_NAV[1].GetComponent.<Renderer>().enabled = false;
			 	}
			}else{
				CurNav++;
				NavList[CurNav].gameObject.AddComponent(FX_3DRadar_RID).IsNAV = true;
			}
		}
	}
}

/***********************************************************************************/
																//Radar Late Update
/***********************************************************************************/
function RIDS(){//Targeting
			
if(ThisDistance < RadarRange[2] || SelectedTargetRID.IsPOI){
	if(!HUDEnabled[0]){
		HUD_TSB[0].GetComponent.<Renderer>().enabled = true;
		HUD_TSB_ID[0].GetComponent.<Renderer>().enabled = true;
		Radar_TSB.GetComponent.<Renderer>().enabled = true;
		
		for(var b : int = 0; b < StatusBar.Length; b++){
			if(IsTargetBar[b]){
				StatusBar[b].GetComponent.<Renderer>().enabled = true;
				if(SetStatus[1] && SBRift[b] == 2){
					StatusBar2[b].GetComponent.<Renderer>().enabled = true;
				}
			}
		}
		
		if(SetStatus[1]){
			HUD_TSB[1].GetComponent.<Renderer>().enabled = true;
			HUD_TSB_ID[1].GetComponent.<Renderer>().enabled = true;	
		}
		HUDEnabled[0] = true;
	}

/***********************************************************************************/
									//Set HUD GUI color based on selected target IFF status
/***********************************************************************************/

	if(ThisClass[2] != SelectedTargetRID.IFF){
		GetNewColor();
	}
	
	if(ThisClass[0] != SelectedTargetRID.ThisClass[0] || ThisClass[1] != SelectedTargetRID.ThisClass[1]){
		SetTextureOffset(SelectedTargetRID.ThisClass[0], SelectedTargetRID.ThisClass[1] + 9, HUD_TSB_ID[0].gameObject, 32);
		if(SetStatus[1]){
			SetTextureOffset(SelectedTargetRID.ThisClass[0], SelectedTargetRID.ThisClass[1] + 9, HUD_TSB_ID[1].gameObject, 32);
		}
		ThisClass[0] = SelectedTargetRID.ThisClass[0];
		ThisClass[1] = SelectedTargetRID.ThisClass[1];
	}
/***********************************************************************************/
											//Draw the HUD TSB / Directional Indicator & Target ID
/***********************************************************************************/
var newScreenPos : Vector3 = IndicatorPositions(TargetPos,2,4);	
HUD_TSB[0].position = newScreenPos;
HUD_TSB_ID[0].position = newScreenPos + Vector3(25,25,0);
	
	if(SetStatus[1]){
		newScreenPos= IndicatorPositions(TargetPos,3,5);
		HUD_TSB[1].position = newScreenPos;
		HUD_TSB_ID[1].position = newScreenPos+ Vector3(25,25,0);
	}

	for(b = 0; b < StatusBar.Length; b++){
		
		if(BarAnchor[b] == 0){
			if(SetStatus[1]){
				if(SBRift[b] == 0){
					StatusBar[b].position = HUD_TSB[0].position + BarOffset[b];
				}else if(SBRift[b] == 1){
					StatusBar[b].position = HUD_TSB[1].position + BarOffset[b];
				}else{
					StatusBar[b].position = HUD_TSB[0].position + BarOffset[b];
					StatusBar2[b].position = HUD_TSB[1].position + BarOffset[b];
				}
			}else{
				StatusBar[b].position = HUD_TSB[0].position + BarOffset[b];
			}
		}
	}
//**************************************************************************	
//												Draw Indicator Arrows
//**************************************************************************	

	if(HUDElements[0] && SelectedTarget[0]){
		if(!HUDEnabled[3]){
			HUD_DIA[0].GetComponent.<Renderer>().enabled = true;
			HUDEnabled[3] = true;
		}
		DirectionIndicator(SelectedTarget[0], HUD_DIA[1]);
	}else if(HUDEnabled[3]){
		HUD_DIA[0].GetComponent.<Renderer>().enabled = false;
		HUDEnabled[3] = false;
	}
	/*
	if(HUDElements[0] && Objective){
		Indicator_Objective.gameObject.SetActive(true);
		DirectionIndicator(Objective, Indicator_Objective);
	}else{
		Indicator_Objective.gameObject.SetActive(false);
	}
}
	*/

	
/***********************************************************************************/
												//Draw the HUD sub component TSB
/***********************************************************************************/
	if(Vector3.Dot(Transforms[2].TransformDirection(Vector3.forward), RelPos) < 0){
		if(SelectedTarget[1]){
			 if(!HUDEnabled[1]){
				HUD_SC_TSB[0].GetComponent.<Renderer>().enabled = true;
				if(SetStatus[1]){
					HUD_SC_TSB[1].GetComponent.<Renderer>().enabled = true;
				}
				HUDEnabled[1] = true;
			}
			newScreenPos =  IndicatorPositions(SelectedTarget[1].position,2,4);	
			HUD_SC_TSB[0].position = newScreenPos;
			
			if(SetStatus[1]){
				newScreenPos =  IndicatorPositions(SelectedTarget[1].position,3,5);	
				HUD_SC_TSB[1].position = newScreenPos;
			}
		}

/***********************************************************************************/
												//Draw the HUD TLI (Target Lead Indicator)
/***********************************************************************************/		
		if(SetStatus[11]){
			if(!HUDEnabled[2]){
				HUD_TLI[0].GetComponent.<Renderer>().enabled = true;
				if(SetStatus[1]){
					HUD_TLI[1].GetComponent.<Renderer>().enabled = true;
				}
				HUDEnabled[2] = true;
			}
			
			var interceptPoint : Vector3 = FindIntercept();
			HUD_TLI[0].position = TLIPosition(interceptPoint,2,4);
			
			if(SetStatus[1]){
				newScreenPos = TLIPosition(interceptPoint,3,5);
				HUD_TLI[1].position = newScreenPos;
			}
			previousPos[1] = PlayerPos;
			previousPos[0] = TargetPos;
		}else if(HUDEnabled[2]){
			HUD_TLI[0].GetComponent.<Renderer>().enabled = false;
			HUDEnabled[2] = false;
		}
/***********************************************************************************/
										//Disable HUD GUI if the target is not in front of us
/***********************************************************************************/		
	}else if(HUDEnabled[2]){
		HUD_TLI[0].GetComponent.<Renderer>().enabled = false;
		HUD_SC_TSB[0].GetComponent.<Renderer>().enabled = false;
		if(SetStatus[1]){
			HUD_TLI[1].GetComponent.<Renderer>().enabled = false;
			HUD_SC_TSB[1].GetComponent.<Renderer>().enabled = false;		
		}
		HUDEnabled[1] = false;
		HUDEnabled[2] = false;
	}
/***********************************************************************************/
												//Disable HUD GUI when the target is out of range
/***********************************************************************************/
	}else if(!SelectedTargetRID.IsPOI && HUDEnabled[0]){
		DisableHUD();
	}
}

function FindIntercept(){// Find the targets intercept point
var PlayerVel : Vector3;
var TargetVel : Vector3;
var TargetRelVel : Vector3;
var SmoothDT : float = Time.smoothDeltaTime;

	if(SetStatus[9]){
		//if(SetStatus[10]){
			//PlayerVel = RigidBodys[0].velocity;
		//}else{
			//PlayerVel = (PlayerPos - previousPos[1]) / SmoothDT;
		//}

		TargetVel = RigidBodys[1].velocity;
		//TargetRelVel =  (TargetVel - (PlayerVel));
		TargetRelVel =  TargetVel;
	}else{
		//PlayerVel = (PlayerPos - previousPos[1]);

		TargetVel = (TargetPos - previousPos[0]);
		//TargetRelVel =  (TargetVel - PlayerVel) / SmoothDT;
		TargetRelVel =  TargetVel / SmoothDT;
	}

var t : float = InterceptTime(TargetVel, TargetRelVel, PlayerVel);
return TargetPos + t * TargetRelVel;
}

function InterceptTime(TargetVel : Vector3, TargetRelVel : Vector3, PlayerVel : Vector3){// Get the time to intercept the target
var TargetRelVelSqr : float = TargetRelVel.sqrMagnitude;
var TArgetRelPosSqr : float = RelPos.sqrMagnitude;

	if(TargetVel.sqrMagnitude < 0.001){
		return 0.0;
	}

var a : float;

	if(SetStatus[21]){
		a = (TargetRelVelSqr - (ProjectileVelocity * ProjectileVelocity) + PlayerVel.sqrMagnitude) / GameScale;
	}else{
		a = (TargetRelVelSqr - (ProjectileVelocity * ProjectileVelocity) + PlayerVel.sqrMagnitude);
	}

var TDot : float = Vector3.Dot(TargetRelVel, RelPos);

	if(Mathf.Abs(a) < 0.001){
		var t : float = -TArgetRelPosSqr / (2.0 * TDot);
		return Mathf.Max(t, 0.0);
	}
	
var b : float = 2.0 * TDot;
var c : float = b*b - 4.0 * a * TArgetRelPosSqr;

	if(c > 0.0){
		var dSqr : float = Mathf.Sqrt(c);
		var t1 = (-b + dSqr) / (2.0 * a);
		var t2 = (-b - dSqr) / (2.0 * a);
		
		if(t1 > 0.0){
			if(t2 > 0.0){
				return Mathf.Min(t1, t2);
			}else{
				return t1;
			}
		}else{
			return Mathf.Max (t2, 0.0);
		}
	}else if(c < 0.0){
		return 0.0;
	}else{
		return Mathf.Max(-b / (2.0 * a), 0.0);
	}
}

function TLIPosition(NewPos : Vector3, x : int, y : int){
var ISP : Vector3;
var TRelPos : Vector3 = Transforms[x].InverseTransformPoint(NewPos);
var ScreenPos : Vector3 = Cameras[y].WorldToViewportPoint(NewPos);
	
ISP = ScreenPos;
ISP = Cameras[x].ViewportToScreenPoint(ISP);

ISP = Vector3(Mathf.Round(ISP.x), Mathf.Round(ISP.y), 1);
ISP = Cameras[x].ScreenToWorldPoint(Vector3(ISP.x + 0.5, ISP.y, .9));
return ISP;
}

function IndicatorPositions(NewPos : Vector3, x : int, y : int){
var ISP : Vector3;
var TRelPos : Vector3 = Transforms[x].InverseTransformPoint(NewPos);
var ScreenPos : Vector3 = Cameras[y].WorldToViewportPoint(NewPos);
var Hit : RaycastHit;
	
	if(ScreenPos.x > 1 || ScreenPos.x < 0 || ScreenPos.y > 1 || ScreenPos.y < 0 || ScreenPos.z <= 0.01){
		if(ScreenPos.x == .5 && ScreenPos.y == .5){
			TRelPos.y = 1;
		}
		Physics.Raycast (Vector3.zero, Vector3(TRelPos.x, TRelPos.y, 0), Hit, 2, 1 << Layers[2]);
		ISP = Vector3(((VFBounds[0] * 0.5) + Hit.point.x) / VFBounds[0], (0.5 + Hit.point.y), 0 );
	}else{
		ISP = ScreenPos;
	}

ISP = Cameras[x].ViewportToScreenPoint(ISP);

var ScreenX : float = (ScreenSize.x - EdgePadding[0]);
var ScreenY : float = (ScreenSize.y - EdgePadding[1]);

	if(SetStatus[1]){
		if(x == 2){
			var a1 : float = (ScreenSize.x * .5) + EdgePadding[0];
			if(ISP.x >= ScreenX){
				ISP.x = ScreenX;
			}else if(ISP.x <= a1){
				ISP.x = a1;
			}		
		}else{
			var a2 : float = (ScreenSize.x * .5) - EdgePadding[0];
			if(ISP.x >= a2){
				ISP.x = a2;
			}else if(ISP.x <= EdgePadding[0]){
				ISP.x = EdgePadding[0];
			}			
		}
	}else{
		if(ISP.x >= ScreenX){
			ISP.x = ScreenX;
		}else if(ISP.x <= EdgePadding[0]){
			ISP.x =EdgePadding[0];
		}
	}
	
	if(ISP.y >= ScreenY){
		ISP.y = ScreenY;
	}else if(ISP.y <= EdgePadding[1]){
		ISP.y = EdgePadding[1];
	}

	if(ISP.x <= EdgePadding[0] || ISP.x >= ScreenX || ISP.y <= EdgePadding[1] || ISP.y >= ScreenY || x == 2 && ISP.x >= ScreenX){
		
		if(SetStatus[6]){
			if(SetStatus[12]){
				HUD_TSB[0].GetComponent.<Renderer>().enabled = false;
				HUD_TSB_ID[0].GetComponent.<Renderer>().enabled = false;
			}else{
				SetTextureOffset(1, 0, HUD_TSB[0].gameObject, 64);
			}
			if(SetStatus[1]){
				if(SetStatus[12]){
					HUD_TSB[1].GetComponent.<Renderer>().enabled = false;
					HUD_TSB_ID[1].GetComponent.<Renderer>().enabled = false;
				}else{
					SetTextureOffset(1, 0, HUD_TSB[1].gameObject, 64);
				}
			}
			SetStatus[6] = false;
		}
	}else{
		if(!SetStatus[6] && x == 2){
			if(SetStatus[12]){
				HUD_TSB[0].GetComponent.<Renderer>().enabled = true;
				HUD_TSB_ID[0].GetComponent.<Renderer>().enabled = true;		
			}
			SetTextureOffset(0, 0, HUD_TSB[0].gameObject, 64);
			if(SetStatus[1]){
				if(SetStatus[12]){
					HUD_TSB[1].GetComponent.<Renderer>().enabled = true;
					HUD_TSB_ID[1].GetComponent.<Renderer>().enabled = true;		
				}
				SetTextureOffset(0, 0, HUD_TSB[1].gameObject, 64);
			}
			SetStatus[6] = true;
		}
	}


ISP = Vector3(Mathf.Round(ISP.x), Mathf.Round(ISP.y), 1);
ISP = Cameras[x].ScreenToWorldPoint(Vector3(ISP.x + 0.5, ISP.y, .9));
return ISP;
}

function DirectionIndicator(ThisObject : Transform, ThisIndicator : Transform){
var RelPos1 : Vector3 = Transforms[2].InverseTransformPoint(ThisObject.position);
var angle = Vector3.Angle(RelPos1, Vector3(0,0,1));	

	if(RelPos1.x == 0 && RelPos1.y == 0){
		RelPos1.y = 1;
	}
	
ThisIndicator.rotation = Quaternion.LookRotation(Vector3(RelPos1.x, RelPos1.y, 0));

	if(ThisIndicator.localEulerAngles.y > 260){
		HUD_DIA[0].localEulerAngles = Vector3(0,90,90);
	}else{
		HUD_DIA[0].localEulerAngles = Vector3(0, 270, 270);
	}
	
	if(ThisIndicator.localEulerAngles == Vector3(270, 0,0)){
		HUD_DIA[0].localEulerAngles = Vector3(90, 0, 0);
	}else if(ThisIndicator.localEulerAngles == Vector3(90, 0,0)){
		HUD_DIA[0].localEulerAngles = Vector3(270, 180, 0);
	}
	
	if(angle >= 0 && angle <= DIAdisableAngle){
		if(HUDEnabled[3]){
			HUD_DIA[0].GetComponent.<Renderer>().enabled = false;
			HUDEnabled[3] = false;
		}
	}else if(!HUDEnabled[3]){
		HUD_DIA[0].GetComponent.<Renderer>().enabled = true;
		HUDEnabled[3] = true;
	}
}
/***********************************************************************************/
																//OnGUI Update
/***********************************************************************************/
function OnGUI(){
	if (EnableTargetList){
		DrawTargetList();
	}
	
	if(EnableNAVList){
		DrawNAVList();
	}
}

function DrawTargetList(){

var Row : int = 0; 
var Col : int = 0;

// Background Box
GUILayout.BeginArea (Rect(Screen.width - 225,20,250,200),"");
GUI.Box (Rect(0,0,150,200),"");

// Slider Info
scrollPosition = GUI.BeginScrollView (Rect (0,0,170,200), scrollPosition, Rect (0, 0, 0, (TargetListAll.Count * 23)+10)); 

	// Draw Enemy Buttons
	for (var h : int = 0; h < TargetListHostile.Count; h++){
		GUI.color = ColorHostile;
		if (GUI.Button(Rect (0, 5 + (Col++ * 23), 150, 20),"" + TargetListHostile[h].name/*,GUIstyle*/)){ 
			if(SelectedTarget[0]){
				SelectedTargetRID.IsPlayerTarget = false;
			}
			SelectedTarget[0] = TargetListHostile[h];
			SetTarget();
		}
	}
	
	// Draw ColorOwnedButtons
	for (var o : int = 0; o < TargetListOwned.Count; o++){
		GUI.color = ColorOwned;
		if (GUI.Button(Rect (0, 5 + (Col++ * 23), 150, 20),"" + TargetListOwned[o].name/*,GUIstyle*/)){ 
			if(SelectedTarget[0]){
				SelectedTargetRID.IsPlayerTarget = false;
			}
			SelectedTarget[0] = TargetListOwned[o];
			SetTarget();
		}
	}
		
	// Draw Friendly Buttons
	for (var f : int = 0; f < TargetListFriendly.Count; f++){
		GUI.color = ColorFriendly;
		if (GUI.Button(Rect (0, 5 + (Col++ * 23), 150, 20),"" + TargetListFriendly[f].name/*,GUIstyle*/)){ 
			if(SelectedTarget[0]){
				SelectedTargetRID.IsPlayerTarget = false;
			}
			SelectedTarget[0] = TargetListFriendly[f];
			SetTarget();
		}
	}

	// Draw Neutral Buttons
	for (var n : int = 0; n < TargetListNeutral.Count; n++){
		GUI.color = ColorNeutral;
		if (GUI.Button(Rect (0, 5 + (Col++ * 23), 150, 20),"" + TargetListNeutral[n].name/*,GUIstyle*/)){ 
			if(SelectedTarget[0]){
				SelectedTargetRID.IsPlayerTarget = false;
			}
			SelectedTarget[0] = TargetListNeutral[n];
			SetTarget();
		}
	}
	
	// Draw Abandoned Buttons
	for (var a : int = 0; a < TargetListAband.Count; a++){
		GUI.color = ColorAbandoned;
		if (GUI.Button(Rect (0, 5 + (Col++ * 23), 150, 20),"" + TargetListAband[a].name/*,GUIstyle*/)){ 
			if(SelectedTarget[0]){
				SelectedTargetRID.IsPlayerTarget = false;
			}
			SelectedTarget[0] = TargetListAband[a];
			SetTarget();
		}
	}	
	GUI.EndScrollView(); 
	GUILayout.EndArea();
}

function DrawNAVList(){
var Row : int = 0; 
var Col : int = 0;

// Background Box
GUILayout.BeginArea (Rect(Screen.width - 400,20,250,200),"");
GUI.Box (Rect(0,0,150,200),"");

// Slider Info
scrollPosition = GUI.BeginScrollView (Rect (0,0,170,200), scrollPosition, Rect (0, 0, 0, (NavList.Length * 23)+10)); 

	// Draw Enemy Buttons
	for (var n : int = 0; n < NavList.Length; n++){
		GUI.color = ColorNAV;
		if (NavList[n] && GUI.Button(Rect (0, 5 + (Col++ * 23), 150, 20),"" + NavList[n].name/*,GUIstyle*/)){ 
			CurNav = n;
		}
	}

	GUI.EndScrollView(); 
	GUILayout.EndArea();
}
/***********************************************************************************/
																//Called Functions
/***********************************************************************************/
function AddToList(IFF : int, Contact : Transform){
	if(IFF == 0){
		TargetListAband.Add(Contact);
		TargetListAll.Add(Contact);
	}
	
	if(IFF == 1){
		TargetListNeutral.Add(Contact);
		TargetListAll.Add(Contact);
	}

	if(IFF == 2){
		TargetListFriendly.Add(Contact);
		TargetListAll.Add(Contact);
	}
	
	if(IFF == 3){
		TargetListHostile.Add(Contact);
		TargetListAll.Add(Contact);
	}

	if(IFF == 5){
		TargetListOwned.Add(Contact);
		TargetListAll.Add(Contact);
	}
}

function RemoveFromList(Contact : Transform){
	for(var a : int; a < TargetListAll.Count; a++){
		if(Contact == TargetListAll[a]){
			TargetListAll.RemoveAt(a);
		}
	}
	for(var f : int; f < TargetListFriendly.Count; f++){
		if(Contact == TargetListFriendly[f]){
			TargetListFriendly.RemoveAt(f);
		}
	}

	for(var n : int; n < TargetListNeutral.Count; n++){
		if(Contact == TargetListNeutral[n]){
			TargetListNeutral.RemoveAt(n);
		}
	}

	for(var h : int; h < TargetListHostile.Count; h++){
		if(Contact == TargetListHostile[h]){
			TargetListHostile.RemoveAt(h);
		}
	}

	for(var o : int; o < TargetListOwned.Count; o++){
		if(Contact == TargetListOwned[o]){
			TargetListOwned.RemoveAt(o);
		}
	}

	for(var ab : int; ab < TargetListAband.Count; ab++){
		if(Contact == TargetListAband[ab]){
			TargetListAband.RemoveAt(ab);
		}
	}
}

function RadarSetup(){//Setup Radar Settings & Camera Viewport
SetRadarScaleZoom();

	switch(RadarPos){
		case 1: // top left
		Cameras[1].targetTexture = null;
		Cameras[1].rect = Rect( 1 - (Cameras[1].rect.x + Cameras[1].rect.width), 1 - (Cameras[1].rect.y + Cameras[1].rect.height), Cameras[1].rect.width, Cameras[1].rect.height);
		break;
			
		case 2: // top right
		Cameras[1].targetTexture = null;
		Cameras[1].rect = Rect(Cameras[1].rect.x, 1 - (Cameras[1].rect.y + Cameras[1].rect.height), Cameras[1].rect.width, Cameras[1].rect.height);
		break;
			
		case 3: // bottom left
		Cameras[1].targetTexture = null;
		Cameras[1].rect = Rect( 1 - (Cameras[1].rect.x +  Cameras[1].rect.width), Cameras[1].rect.y, Cameras[1].rect.width, Cameras[1].rect.height);
		break;
			
		case 4: // bottom right
		Cameras[1].targetTexture = null;
		Cameras[1].rect = Rect(Cameras[1].rect.x, Cameras[1].rect.y, Cameras[1].rect.width, Cameras[1].rect.height);
		break;
			
		case 5: // Render To Texture
				if(Transforms[4]){
				
				var TextureSize : int[] = new int[2];
				var TextureQuality : int[] = new int[2];
				switch(TextureSizeX){
					case 0:
						TextureSize[0] = 16;
					break;
					case 1:
						TextureSize[0] = 32;
					break;
					case 2:
						TextureSize[0] = 64;
					break;
					case 3:
						TextureSize[0] = 128;
					break;
					case 4:
						TextureSize[0] = 256;
					break;
					case 5:
						TextureSize[0] = 512;
					break;
					case 6:
						TextureSize[0] = 1024;
					break;
					case 7:
						TextureSize[0] = 2048;
					break;
				}
				switch(TextureSizeY){
					case 0:
						TextureSize[1] = 16;
					break;
					case 1:
						TextureSize[1] = 32;
					break;
					case 2:
						TextureSize[1] = 64;
					break;
					case 3:
						TextureSize[1] = 128;
					break;
					case 4:
						TextureSize[1] = 256;
					break;
					case 5:
						TextureSize[1] = 512;
					break;
					case 6:
						TextureSize[1] = 1024;
					break;
					case 7:
						TextureSize[1] = 2048;
					break;
				}
				
				switch(DBuffer){
					case 0:
						TextureQuality[0] = 0;
					break;
					case 1:
						TextureQuality[0] = 16;
					break;
					case 2:
						TextureQuality[0] = 24;
					break;
				}			
				switch(AALevel){
					case 0:
						TextureQuality[1] = 1;
					break;
					case 1:
						TextureQuality[1] = 2;
					break;
					case 2:
						TextureQuality[1] = 4;
					break;
					case 3:
						TextureQuality[1] = 8;
					break;
				}
				
				var TT = new RenderTexture(TextureSize[0], TextureSize[1], RTextureAnsio);
				
				switch(_FilterMode){
					case 0:
						TT.filterMode = FilterMode.Point;
					break;
					case 1:
						TT.filterMode = FilterMode.Bilinear;
					break;
					case 2:
						TT.filterMode = FilterMode.Trilinear;
					break;
				}
					
					TT.anisoLevel = TextureQuality[0];
					TT.antiAliasing = TextureQuality[1];
					Cameras[1].targetTexture = TT;
					Cameras[1].rect = Rect(0, 0, 1, 1);
					Transforms[4].GetComponent.<Renderer>().material = Resources.Load("Materials/Radar_Render_T") as Material;
					Transforms[4].GetComponent.<Renderer>().material.SetTexture("_MainTex", TT);
					Cameras[1].clearFlags = CameraClearFlags.SolidColor;
					Cameras[1].backgroundColor = RenderSolidColor;
				}else{
					Debug.Log("Please assign an object to recieve the render texture");
				}
		break;
	}
	Cameras[0].rect = Cameras[1].rect;
}

function SetRadarScaleZoom(){
var RadarZoomAmount : float;
	
	switch (RadarZoom){
		case 0:
		RadarZoomAmount = GameScale;
		break;
		
		case 1:
		RadarZoomAmount = (GameScale * 2.0);
		break;
		
		case 2:
		RadarZoomAmount = (GameScale * 4.0);
		break;
		
		case 3:
		RadarZoomAmount = (GameScale * 0.5);
		break;
		
		case 4:
		RadarZoomAmount = (GameScale * 0.25);
		break;
	}
	
RadarRange[2] = (RadarRange[0] / RadarZoomAmount);
RadarRange[1] = (RadarRange[2] * RadarRange[2]);
RadarRange[3] = (1 / (RadarRange[2] * 2));
CurrentZoom = RadarZoom;
}

function DisableHUD(){
Radar_TSB.GetComponent.<Renderer>().enabled = false;
HUD_TSB[0].GetComponent.<Renderer>().enabled = false;
HUD_TSB_ID[0].GetComponent.<Renderer>().enabled = false;
HUD_TLI[0].GetComponent.<Renderer>().enabled = false;
HUD_SC_TSB[0].GetComponent.<Renderer>().enabled = false;
	
	for(var b : int = 0; b < StatusBar.Length; b++){
		if(IsTargetBar[b]){
			StatusBar[b].GetComponent.<Renderer>().enabled = false;
			if(SetStatus[1] && SBRift[b] == 2){
				StatusBar2[b].GetComponent.<Renderer>().enabled = false;
			}
		}
	}

	if(SetStatus[1]){
		HUD_TSB[1].GetComponent.<Renderer>().enabled = false;
		HUD_TSB_ID[1].GetComponent.<Renderer>().enabled = false;
		HUD_TLI[1].GetComponent.<Renderer>().enabled = false;
		HUD_SC_TSB[1].GetComponent.<Renderer>().enabled = false;
	}
	
HUD_DIA[0].GetComponent.<Renderer>().enabled = false;
ClearSubC();
ClearTarget();

HUDEnabled[0] = false;
HUDEnabled[1] = false;
HUDEnabled[2] = false;
HUDEnabled[3] = false;
}

function TargetingCommands(){//Targeting inputs
/***********************************************************************************/
													//Keyboard Input Execution
/***********************************************************************************/
	// Find the closest target to the player
	if(KeyAssign.TargetClosestKM == 0 && Input.GetKeyDown(KeyAssign.TargetClosest)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		ClosestTarget();
	}else if(KeyAssign.TargetClosestKM == 1 && Input.GetKeyDown(KeyAssign.TargetClosest) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		ClosestTarget();	
	}else if(KeyAssign.TargetClosestKM == 2 && Input.GetKeyDown(KeyAssign.TargetClosest) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		ClosestTarget();	
	}else if(KeyAssign.TargetClosestKM == 3 && Input.GetKeyDown(KeyAssign.TargetClosest) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		ClosestTarget();
	}

	// Find the next target in the array
	if(KeyAssign.TargetNextKM == 0 && Input.GetKeyDown(KeyAssign.TargetNext)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		NextTarget();
	}else if(KeyAssign.TargetNextKM == 1 && Input.GetKeyDown(KeyAssign.TargetNext) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		NextTarget();
	}else if(KeyAssign.TargetNextKM == 2 && Input.GetKeyDown(KeyAssign.TargetNext) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		NextTarget();
	}else if(KeyAssign.TargetNextKM == 3 && Input.GetKeyDown(KeyAssign.TargetNext) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		NextTarget();
	}

	// Find the previous target in the array
	if(KeyAssign.TargetPrevKM == 0 && Input.GetKeyDown(KeyAssign.TargetPrev)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		PreviousTarget();
	}else if(KeyAssign.TargetPrevKM == 1 && Input.GetKeyDown(KeyAssign.TargetPrev) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		PreviousTarget();
	}else if(KeyAssign.TargetPrevKM == 2 && Input.GetKeyDown(KeyAssign.TargetPrev) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		PreviousTarget();
	}else if(KeyAssign.TargetPrevKM == 3 && Input.GetKeyDown(KeyAssign.TargetPrev) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}
		PreviousTarget();
	}
	
	// Find the next Sub-component on the selected target	
	if(KeyAssign.TargetNextSKM == 0 && Input.GetKeyDown(KeyAssign.TargetNextS)){
		NextSubComp();
		PlaySelectSCSound();
	}else if(KeyAssign.TargetNextSKM == 1 && Input.GetKeyDown(KeyAssign.TargetNextS) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		NextSubComp();
		PlaySelectSCSound();
	}else if(KeyAssign.TargetNextSKM == 2 && Input.GetKeyDown(KeyAssign.TargetNextS) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		NextSubComp();
		PlaySelectSCSound();
	}else if(KeyAssign.TargetNextSKM == 3 && Input.GetKeyDown(KeyAssign.TargetNextS) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		NextSubComp();
		PlaySelectSCSound();
	}
	
	// Find the previous Sub-component on the selected target
	if(KeyAssign.TargetPrevSKM == 0 && Input.GetKeyDown(KeyAssign.TargetPrevS)){
		PreviousSubComp();
		PlaySelectSCSound();
	}else if(KeyAssign.TargetPrevSKM == 1 && Input.GetKeyDown(KeyAssign.TargetPrevS) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		PreviousSubComp();
		PlaySelectSCSound();
	}else if(KeyAssign.TargetPrevSKM == 2 && Input.GetKeyDown(KeyAssign.TargetPrevS) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		PreviousSubComp();
		PlaySelectSCSound();
	}else if(KeyAssign.TargetPrevSKM == 3 && Input.GetKeyDown(KeyAssign.TargetPrevS) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		PreviousSubComp();
		PlaySelectSCSound();
	}
	
	// Clear selected Sub-component
	if(KeyAssign.ClearSubCKM == 0 && Input.GetKeyDown(KeyAssign.ClearSubC)){
		PlayClearSCSound();
		ClearSubC();
	}else if(KeyAssign.ClearSubCKM == 1 && Input.GetKeyDown(KeyAssign.ClearSubC) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		PlayClearSCSound();
		ClearSubC();
	}else if(KeyAssign.ClearSubCKM == 2 && Input.GetKeyDown(KeyAssign.ClearSubC) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		PlayClearSCSound();
		ClearSubC();
	}else if(KeyAssign.ClearSubCKM == 3 && Input.GetKeyDown(KeyAssign.ClearSubC) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		PlayClearSCSound();
		ClearSubC();
	}
	
	// Clear selected Sub-component
	if(KeyAssign.ClearTargetKM == 0 && Input.GetKeyDown(KeyAssign.ClearTarget)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		ClearTarget();
	}else if(KeyAssign.ClearTargetKM == 1 && Input.GetKeyDown(KeyAssign.ClearTarget) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		ClearTarget();
	}else if(KeyAssign.ClearTargetKM == 2 && Input.GetKeyDown(KeyAssign.ClearTarget) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		ClearTarget();
	}else if(KeyAssign.ClearTargetKM == 3 && Input.GetKeyDown(KeyAssign.ClearTarget) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		ClearTarget();
	}

	// Find the closest Hostile to the player
	if(KeyAssign.TargetClosestHKM == 0 && Input.GetKeyDown(KeyAssign.TargetClosestH)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		ClosestHostile();
	}else if(KeyAssign.TargetClosestHKM == 1 && Input.GetKeyDown(KeyAssign.TargetClosestH) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		ClosestHostile();
	}else if(KeyAssign.TargetClosestHKM == 2 && Input.GetKeyDown(KeyAssign.TargetClosestH) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		ClosestHostile();
	}else if(KeyAssign.TargetClosestHKM == 3 && Input.GetKeyDown(KeyAssign.TargetClosestH) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		ClosestHostile();
	}
	
	// Find the next Hostile in the array
	if(KeyAssign.TargetNextHKM == 0 && Input.GetKeyDown(KeyAssign.TargetNextH)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		NextHostile();
	}else if(KeyAssign.TargetNextHKM == 1 && Input.GetKeyDown(KeyAssign.TargetNextH) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		NextHostile();
	}else if(KeyAssign.TargetNextHKM == 2 && Input.GetKeyDown(KeyAssign.TargetNextH) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		NextHostile();
	}else if(KeyAssign.TargetNextHKM == 3 && Input.GetKeyDown(KeyAssign.TargetNextH) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		NextHostile();
	}
	
	// Find the previous Hostile in the array
	if(KeyAssign.TargetPrevHKM == 0 && Input.GetKeyDown(KeyAssign.TargetPrevH)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		PreviousHostile();
	}else if(KeyAssign.TargetPrevHKM == 1 && Input.GetKeyDown(KeyAssign.TargetPrevH) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		PreviousHostile();
	}else if(KeyAssign.TargetPrevHKM == 2 && Input.GetKeyDown(KeyAssign.TargetPrevH) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		PreviousHostile();
	}else if(KeyAssign.TargetPrevHKM == 3 && Input.GetKeyDown(KeyAssign.TargetPrevH) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		if(SelectedTarget[0]){
			SelectedTargetRID.IsPlayerTarget = false;
		}	
		PreviousHostile();
	}
	
	// Display / Hide NAV List
	if(KeyAssign.NAVListKM == 0 && Input.GetKeyDown(KeyAssign.NAVList)){
		EnableNAVList = !EnableNAVList;
	}else if(KeyAssign.NAVListKM == 1 && Input.GetKeyDown(KeyAssign.NAVList) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		EnableNAVList = !EnableNAVList;
	}else if(KeyAssign.NAVListKM == 2 && Input.GetKeyDown(KeyAssign.NAVList) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		EnableNAVList = !EnableNAVList;
	}else if(KeyAssign.NAVListKM == 3 && Input.GetKeyDown(KeyAssign.NAVList) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		EnableNAVList = !EnableNAVList;
	}

	// Display / Hide Target List
	if(KeyAssign.TargetListKM == 0 && Input.GetKeyDown(KeyAssign.TargetList)){
		EnableTargetList = !EnableTargetList;
	}else if(KeyAssign.TargetListKM == 1 && Input.GetKeyDown(KeyAssign.TargetList) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		EnableTargetList = !EnableTargetList;
	}else if(KeyAssign.TargetListKM == 2 && Input.GetKeyDown(KeyAssign.TargetList) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		EnableTargetList = !EnableTargetList;
	}else if(KeyAssign.TargetListKM == 3 && Input.GetKeyDown(KeyAssign.TargetList) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		EnableTargetList = !EnableTargetList;
	}
		
	// Filter Hostile
	if(KeyAssign.FilterHostileKM == 0 && Input.GetKeyDown(KeyAssign.FilterHostile)){
		FilterHostile = !FilterHostile;
	}else if(KeyAssign.FilterHostileKM == 1 && Input.GetKeyDown(KeyAssign.FilterHostile) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		FilterHostile = !FilterHostile;
	}else if(KeyAssign.FilterHostileKM == 2 && Input.GetKeyDown(KeyAssign.FilterHostile) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		FilterHostile = !FilterHostile;
	}else if(KeyAssign.FilterHostileKM == 3 && Input.GetKeyDown(KeyAssign.FilterHostile) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		FilterHostile = !FilterHostile;
	}

	// Switch 3D 2D Radar
	if(KeyAssign.Switch3D2DKM == 0 && Input.GetKeyDown(KeyAssign.Switch3D2D)){
		SetStatus[7] = !SetStatus[7];
	}else if(KeyAssign.Switch3D2DKM == 1 && Input.GetKeyDown(KeyAssign.Switch3D2D) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		SetStatus[7] = !SetStatus[7];
	}else if(KeyAssign.Switch3D2DKM == 2 && Input.GetKeyDown(KeyAssign.Switch3D2D) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		SetStatus[7] = !SetStatus[7];
	}else if(KeyAssign.Switch3D2DKM == 3 && Input.GetKeyDown(KeyAssign.Switch3D2D) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		SetStatus[7] = !SetStatus[7];
	}
	
	// Toggle Indicators
	if(KeyAssign.ToggleIndicatorsKM == 0 && Input.GetKeyDown(KeyAssign.ToggleIndicators)){
		SetStatus[13] = !SetStatus[13];
	}else if(KeyAssign.ToggleIndicatorsKM == 1 && Input.GetKeyDown(KeyAssign.ToggleIndicators) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
		SetStatus[13] = !SetStatus[13];
	}else if(KeyAssign.ToggleIndicatorsKM == 2 && Input.GetKeyDown(KeyAssign.ToggleIndicators) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
		SetStatus[13] = !SetStatus[13];
	}else if(KeyAssign.ToggleIndicatorsKM == 3 && Input.GetKeyDown(KeyAssign.ToggleIndicators) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
		SetStatus[13] = !SetStatus[13];
	}
	
/***********************************************************************************/
										//Use the mouse button 0 to select a target
/***********************************************************************************/
	if(Input.GetMouseButtonUp(0)){
		var MouseHit : RaycastHit;
		var ray1 : Ray = Cameras[4].ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast (ray1, MouseHit ,Mathf.Infinity, 1 << Layers[4])){
			var newDist : float = (MouseHit.collider.transform.position - PlayerPos).magnitude;
			if(MouseHit.collider.GetComponent(FX_3DRadar_RID).IsPOI || newDist < RadarRange[2]){
				if(SelectedTarget[0]){
					SelectedTargetRID.IsPlayerTarget = false;
				}
	
				SelectedTarget[0] = MouseHit.transform;
				SetTarget();
			}
		}
		
		if(SetStatus[16]){
			var ray2 : Ray = Cameras[2].ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast (ray2, MouseHit ,Mathf.Infinity, 1 << Layers[2])){
				if(SelectedTarget[0]){
					SelectedTargetRID.IsPlayerTarget = false;
				}
	
				SelectedTarget[0] = MouseHit.collider.GetComponent(FX_3DRadar_Info).ThisParent;
				SetTarget();
			}
		}
		
		if(SetStatus[17]){
			var ray3 : Ray = Cameras[1].ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast (ray3, MouseHit ,Mathf.Infinity, 1 << Layers[1])){
				if(SelectedTarget[0]){
					SelectedTargetRID.IsPlayerTarget = false;
				}
	
				SelectedTarget[0] = MouseHit.collider.GetComponent(FX_3DRadar_Info).ThisParent;
				SetTarget();
			}
		}
	}
}

function SetTarget(){//Targeting Function
/***********************************************************************************/
					//Gather the current target components & set target state
/***********************************************************************************/
	if(SelectedTarget[0]){
		PlaySelectSound();
		ClearSubC();
		
		SelectedTargetRID = SelectedTarget[0].GetComponent(FX_3DRadar_RID);
		SelectedTargetRID.IsPlayerTarget = true;

		SetTextureOffset(SelectedTargetRID.ThisClass[0], SelectedTargetRID.ThisClass[1] + 9, HUD_TSB_ID[0].gameObject, 32);
		
		if(SetStatus[9]){
			RigidBodys[1] = SelectedTarget[0].GetComponent.<Rigidbody>();
		}
			
		if(SetStatus[1]){
			SetTextureOffset(SelectedTargetRID.ThisClass[0], SelectedTargetRID.ThisClass[1] + 9, HUD_TSB_ID[1].gameObject, 32);
		}
			
		ThisClass[0] = SelectedTargetRID.ThisClass[0];
		ThisClass[1] = SelectedTargetRID.ThisClass[1];
		GetNewColor();
		
		SelectedTarget[0].SendMessage("ApplyDamage", 0, SendMessageOptions.DontRequireReceiver);
	}
}

function ClosestTarget(){//Targeting Input Command
var closestDistance : float = Mathf.Infinity;
ClearTarget();
	for (var t : Transform in TargetListAll){
		var curDistance = (t.position - PlayerPos).sqrMagnitude;	
		if(curDistance < closestDistance){
			SelectedTarget[0] = t;
			closestDistance = curDistance;
		}
	}
SetTarget();
}

function NextTarget(){//Targeting Input Command
	if(TargetListAll.Count > 0){
		ThisCurrentTarget.x = (ThisCurrentTarget.x + 1) % TargetListAll.Count;
		SelectedTarget[0] = TargetListAll[ThisCurrentTarget.x];
	}
SetTarget();
}

function PreviousTarget(){//Targeting Input Command
	if(TargetListAll.Count > 0){
		if (ThisCurrentTarget.x == 0){
			ThisCurrentTarget.x = TargetListAll.Count;
		}
		if(ThisCurrentTarget.x > 0){
			ThisCurrentTarget.x = ThisCurrentTarget.x -1;
		}
	}
SelectedTarget[0] = TargetListAll[ThisCurrentTarget.x];
SetTarget();
}

function ClearTarget(){//Targeting Input Command
/***********************************************************************************/
														//Clear the current target
/***********************************************************************************/
	if(SelectedTarget[0]){
		PlayClearSound();
		SelectedTarget[0] = null;
	}
}

function FindSubComp(){//Targeting Function
/***********************************************************************************/
								//Create an array of all subcomponents on the selected target
/***********************************************************************************/
if(SelectedTarget[0]){
	SubComponentList.Clear();
	var SubComponent : Transform[] = SelectedTarget[0].Cast.<Transform>().Select(function(t) { return t; }).ToArray();
		for(var s : Transform in SubComponent){
			if(s.tag == "Sub_Component"){
				SubComponentList.Add(s);
			}
		}
	}
}

function NextSubComp(){//Targeting Input Command
FindSubComp();
	if(SelectedTarget[0] && SubComponentList.Count > 0){
		ThisCurrentTarget.y = (ThisCurrentTarget.y + 1) % SubComponentList.Count;
		SelectedTarget[1] = SubComponentList[ThisCurrentTarget.y];
	}
}

function PreviousSubComp(){//Targeting Input Command
FindSubComp();
	if(SelectedTarget[0] && SubComponentList.Count > 0){
		if (ThisCurrentTarget.y == 0){
			ThisCurrentTarget.y = SubComponentList.Count;
		}
		if(ThisCurrentTarget.y > 0){
			ThisCurrentTarget.y = ThisCurrentTarget.y -1;
		}
		SelectedTarget[1] = SubComponentList[ThisCurrentTarget.y];
	}
}

function ClearSubC(){//Targeting Input Command
/***********************************************************************************/
											//Clear the current sub-component target
/***********************************************************************************/
	if(SelectedTarget[1]){
		if(HUDEnabled[1]){
			HUD_SC_TSB[0].GetComponent.<Renderer>().enabled = false;
			if(SetStatus[1]){
				HUD_SC_TSB[1].GetComponent.<Renderer>().enabled = false;
			}
			HUDEnabled[1] = false;
		}
		SelectedTarget[1] = null;
	}
}

function ClosestHostile(){//Targeting Input Command
	var closestDistance : float = Mathf.Infinity;
	//ClearTarget();
	for (var t : Transform in TargetListHostile){
		var curDistance = (t.position - PlayerPos).sqrMagnitude;	
		if(curDistance < closestDistance){
			SelectedTarget[0] = t;
			closestDistance = curDistance;
		}
	}
SetTarget();
}

function NextHostile(){//Targeting Input Command
	if(TargetListHostile.Count > 0){
		ThisCurrentTarget.x = (ThisCurrentTarget.x + 1) % TargetListHostile.Count;
		SelectedTarget[0] = TargetListHostile[ThisCurrentTarget.x];
	}
SetTarget();
}

function PreviousHostile(){//Targeting Input Command
	if(TargetListHostile.Count > 0){
		if (ThisCurrentTarget.x == 0){
			ThisCurrentTarget.x = TargetListHostile.Count;
		}
		if(ThisCurrentTarget.x > 0){
			ThisCurrentTarget.x = ThisCurrentTarget.x -1;
		}
	}
SelectedTarget[0] = TargetListHostile[ThisCurrentTarget.x];
SetTarget();
}

function PlaySelectSound(){
	if(RadarSounds[0] && SelectedTarget[0]){
		GetComponent.<AudioSource>().PlayOneShot(RadarSounds[0]);
	}
}

function PlayClearSound(){
	if(RadarSounds[1] && SelectedTarget[0]){
		GetComponent.<AudioSource>().PlayOneShot(RadarSounds[1]);
	}
}

function PlaySelectSCSound(){
	if(RadarSounds[0] && SelectedTarget[1]){
		GetComponent.<AudioSource>().PlayOneShot(RadarSounds[0]);
	}
}

function PlayClearSCSound(){
	if(RadarSounds[1] && SelectedTarget[1]){
		GetComponent.<AudioSource>().PlayOneShot(RadarSounds[1]);
	}
}

function PlayWarningSound(){
	if(RadarSounds[2]){
		GetComponent.<AudioSource>().PlayOneShot(RadarSounds[2]);
	}
}

function Radar3D_2D(){// Switch between 2D / 3D Camera views
	if(!SetStatus[3]){
		if(SetStatus[7] && !SetStatus[8]){
			Cameras[0].transform.localPosition = Vector3(0,Camera3DPos[0].y,0);
			Cameras[0].transform.localEulerAngles = Vector3(90,0,0);
			ResetScale();
			SetStatus[8] = true;
		}else if(!SetStatus[7] && SetStatus[8]){
			Cameras[0].transform.localPosition = Camera3DPos[0];
			Cameras[0].transform.localEulerAngles = Camera3DPos[1];
			ResetScale();
			SetStatus[8] = false;
		}
	}else{
		if(SetStatus[7] && !SetStatus[8]){
			Cameras[0].transform.localPosition = Vector3(0,Per2D_Y,0);
			Cameras[0].transform.localEulerAngles = Vector3(90,0,0);
			ResetScale();
			SetStatus[8] = true;
		}else if(!SetStatus[7] && SetStatus[8]){
			Cameras[0].transform.localPosition = Camera3DPos[0];
			Cameras[0].transform.localEulerAngles = Camera3DPos[1];
			ResetScale();
			SetStatus[8] = false;
		}	
	}
}
/***********************************************************************************/
																//Colors & Textures
/***********************************************************************************/
function GetIFFColor(uIFF : int){
var UseColor : Color;

	switch(uIFF){
		case  0:
			UseColor = ColorAbandoned;
		break;	
		case  1:
			UseColor = ColorNeutral;
		break;
		case  2:
			UseColor = ColorFriendly;
		break;
		case  3:
			UseColor = ColorHostile;
		break;
		case  4:
			UseColor = ColorUnknown;
		break;
		case  5:
			UseColor = ColorOwned;
		break;
		case 6:
			UseColor = ColorNAV;
		break;
		case 7:
			UseColor = ColorObjective;
		break;
	}
return UseColor;
}

function GetNewColor(){
var UseColor : Color = GetIFFColor(SelectedTargetRID.IFF);

SetNewColor(HUD_TSB[0].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));
SetNewColor(HUD_TLI[0].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));
SetNewColor(HUD_SC_TSB[0].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));
SetNewColor(Radar_TSB.gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[1]));
SetNewColor(HUD_DIA[0].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));

	if(!HUDElements[3]){
		SetNewColor(HUD_TSB_ID[0].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));
	}else{
		SetNewColor(HUD_TSB_ID[0].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, 0.0));
	}

	if(SetStatus[1]){
		SetNewColor(HUD_TSB[1].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));
		SetNewColor(HUD_TLI[1].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));
		SetNewColor(HUD_SC_TSB[1].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));
		if(!HUDElements[3]){
			SetNewColor(HUD_TSB_ID[1].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, HUDOpacity[0]));
		}else{
			SetNewColor(HUD_TSB_ID[1].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, 0.0));
		}
	}
	
ThisClass[2] = SelectedTargetRID.IFF;
}

function SetNewColor(ThisMesh : GameObject, ThisColor : Color){
var mesh : Mesh = ThisMesh.GetComponent(MeshFilter).mesh;
var vertices : Vector3[] = mesh.vertices;
var colors : Color[] = new Color[vertices.Length];

	for (var i = 0; i < vertices.Length;i++){
		colors[i] = ThisColor;
	}

mesh.colors = colors;
}

function SetTextureOffset(uType : int, uRank : int, ID_Transform : GameObject, TextureSize : int){
var AtlasTexture : Vector2 = Vector2(RadarAtlasMaterial.GetTexture("_MainTex").width, RadarAtlasMaterial.GetTexture("_MainTex").height);
var UV_Tiling : Vector2 = Vector2(TextureSize / AtlasTexture.x, TextureSize / AtlasTexture.y);
var theMesh : Mesh = ID_Transform.GetComponent(MeshFilter).mesh as Mesh;
var theUVs : Vector2[] = new Vector2[4];
var UV_Offset : Vector2;

theUVs = theMesh.uv;

	if(uType > 0){
		UV_Offset.x = (uType * TextureSize) / AtlasTexture.x;
	}
	
	if(uRank > 0){
		UV_Offset.y =  (uRank * TextureSize) / AtlasTexture.y;
	}

	theUVs[0] = Vector2(UV_Offset.x, 0 - (UV_Offset.y + UV_Tiling.y));
	theUVs[1] = Vector2(UV_Offset.x, -UV_Offset.y);
	theUVs[2] = Vector2(UV_Offset.x + UV_Tiling.x, -(UV_Offset.y + UV_Tiling.y));
	theUVs[3] = Vector2(UV_Offset.x + UV_Tiling.x, -UV_Offset.y);
	
	theMesh.uv = theUVs;
}

/***********************************************************************************/
																//Mesh Creators & Mods
/***********************************************************************************/
function MakeTri(Name : String, ThisScale : float, ThisParent : Transform, ThisLayer : int){
var ThisMesh : Transform  = new GameObject(Name).transform;
var mesh : Mesh = ThisMesh.gameObject.AddComponent(MeshFilter).mesh;
ThisScale *= .5;

var vertices : Vector3[]  = new Vector3[4];
vertices[0] = Vector3( -ThisScale, -ThisScale, 0);
vertices[1] =  Vector3( 0, ThisScale, 0);
vertices[2] =  Vector3(ThisScale, -ThisScale, 0);

var uv : Vector2[] = new Vector2[4];
uv[0] =  Vector2(0, 0);
uv[1] =  Vector2(0, 1);
uv[2] =  Vector2(1, 0);
uv[3] =  Vector2(1, 1);

var triangles : int[]  = new int[6];
triangles[0] =  0;
triangles[1] =  1; 
triangles[2] =  2;

mesh.vertices = vertices;
mesh.uv = uv;
mesh.triangles = triangles;

ThisMesh.gameObject.AddComponent(MeshRenderer);
ThisMesh.GetComponent.<Renderer>().receiveShadows = false;
ThisMesh.GetComponent.<Renderer>().castShadows = false;
ThisMesh.GetComponent.<Renderer>().enabled = false;
ThisMesh.parent = ThisParent;
ThisMesh.gameObject.layer = ThisLayer;
ThisMesh.GetComponent.<Renderer>().material = RadarAtlasMaterial;
return ThisMesh;
}

function BarSetup(Bar : int, BA : AnchorPos, Name : String, SizeX : int, SizeY : int, Parent : Transform){
var Rotation : float;

	switch(BarDirection[Bar]){
		case 0:
			Rotation = 0;
		break;

		case 1:
			Rotation = 180;
		break;
			
		case 2:
			Rotation = 90;
		break;
		
		case 3:
			Rotation = 270;
		break;
	}

	if(SetStatus[1]){
		if(SBRift[Bar] == 0){
			StatusBar[Bar] = MakeQuad(Name, SizeX, SizeY, false, Vector3(0,0,Rotation), null, Layers[2]);
		}else if(SBRift[Bar] == 1){
			StatusBar[Bar] = MakeQuad(Name, SizeX, SizeY, false, Vector3(0,0,Rotation), null, Layers[3]);
		}else{
			StatusBar[Bar] = MakeQuad(Name, SizeX, SizeY, false, Vector3(0,0,Rotation), null, Layers[2]);
			StatusBar2[Bar] = MakeQuad(Name, SizeX, SizeY, false, Vector3(0,0,Rotation), null, Layers[3]);
			StatusBar2[Bar].parent = Parent;
		}
	}else{
		StatusBar[Bar] = MakeQuad(Name, SizeX, SizeY, false, Vector3(0,0,Rotation), null, Layers[2]);
	}
	StatusBar[Bar].parent = Parent;
	
	var BarRelPos : Vector2;
	
	switch(BA){
		case 0:
			BarRelPos = Vector2(0, 0);
		break;

		case 1:
			BarRelPos = Vector2(0, 0);
		break;
			
		case 2:
			BarRelPos = Vector2(Screen.width * 0.5, 0);
		break;
		
		case 3:
			BarRelPos = Vector2(Screen.width, 0);
		break;
		
		case 4:
			BarRelPos = Vector2(0, Screen.height * 0.5);
		break;
		
		case 5:
			BarRelPos = Vector2(Screen.width * 0.5, Screen.height * 0.5);
		break;
		
		case 6:
			BarRelPos = Vector2(Screen.width, Screen.height * .5);
		break;
		
		case 7:
			BarRelPos = Vector2(0, Screen.height);
		break;
		
		case 8:
			BarRelPos = Vector2(Screen.width * 0.5, Screen.height * .5);
		break;
		
		case 9:
			BarRelPos = Vector2(Screen.width, Screen.height);
		break;
	}

	if(SetStatus[1]){
		if(SBRift[Bar] == 0){
			StatusBar[Bar].position = Cameras[2].ScreenToWorldPoint(Vector3((BarRelPos.x + BarOffset[Bar].x) + (Screen.width * Cameras[2].rect.width) + .5, BarRelPos.y + BarOffset[Bar].y, .9));
		}else if(SBRift[Bar] == 1){
			StatusBar[Bar].position = Cameras[3].ScreenToWorldPoint(Vector3(BarRelPos.x + BarOffset[Bar].x + .5, BarRelPos.y + BarOffset[Bar].y, .9));
		}else{
			StatusBar[Bar].position = Cameras[2].ScreenToWorldPoint(Vector3((BarRelPos.x + BarOffset[Bar].x) + (Screen.width * Cameras[2].rect.width) + .5, BarRelPos.y + BarOffset[Bar].y, .9));
			StatusBar2[Bar].position = Cameras[3].ScreenToWorldPoint(Vector3(BarRelPos.x + BarOffset[Bar].x + .5, BarRelPos.y + BarOffset[Bar].y, .9));
		}
	}else{
		StatusBar[Bar].position = Cameras[2].ScreenToWorldPoint(Vector3(BarRelPos.x + BarOffset[Bar].x + .5, BarRelPos.y + BarOffset[Bar].y, .9));
	}
		
	if(!BarCustom[Bar]){
		StatusBar[Bar].GetComponent.<Renderer>().material = Resources.Load("Materials/Status_Bar_Lte") as Material;
		BarMaterial[Bar] = StatusBar[Bar].GetComponent.<Renderer>().material;
		BarMaterial[Bar].SetColor("_BarColor", BarColor[Bar]);
		BarMaterial[Bar].SetColor("_BarBGColor", BarBGColor);
	}else{
		StatusBar[Bar].GetComponent.<Renderer>().material = BarMatTemp[Bar];
		BarMaterial[Bar] = StatusBar[Bar].GetComponent.<Renderer>().material;
	}
	
	if(SetStatus[1] && SBRift[Bar] == 2){
		StatusBar2[Bar].GetComponent.<Renderer>().sharedMaterial = BarMaterial[Bar];
	}
	
	if(!IsTargetBar[Bar]){
		StatusBar[Bar].GetComponent.<Renderer>().enabled = true;
		if(SetStatus[1] && SBRift[Bar] == 2){
			StatusBar2[Bar].GetComponent.<Renderer>().enabled = true;
		}
	}
}

function MakeQuad(Name : String, ThisScaleX : int, ThisScaleY : int, CustomScale : boolean, ThisRotation : Vector3, ThisParent : Transform, ThisLayer : int){
var ThisMesh : Transform = new GameObject(Name).transform;
var scaleX : float;
var scaleY : float;
var mesh : Mesh = ThisMesh.gameObject.AddComponent(MeshFilter).mesh;

	if(CustomScale){
		scaleX = (1 / ((Screen.height * Cameras[1].rect.height) / ThisScaleX)) * .5;
		scaleY = (1 / ((Screen.height * Cameras[1].rect.height) / ThisScaleY)) * .5;
	}else{
		scaleX = (ThisScaleX * .5);
		scaleY = (ThisScaleY * .5);
	}

var vertices : Vector3[]  = new Vector3[4];
vertices[0] = Vector3( -scaleX, -scaleY, 0);
vertices[1] =  Vector3( -scaleX, scaleY, 0);
vertices[2] =  Vector3(scaleX, -scaleY, 0);
vertices[3] =  Vector3(scaleX, scaleY, 0);

var uv : Vector2[] = new Vector2[4];
uv[0] =  Vector2(0, 0); // BL
uv[1] =  Vector2(0, 1); // TL
uv[2] =  Vector2(1, 0); // BR
uv[3] =  Vector2(1, 1); // TR

var triangles : int[]  = new int[6];
triangles[0] =  0;
triangles[1] =  1; 
triangles[2] =  2;
triangles[3] =  2; 
triangles[4] =  1; 
triangles[5] =  3;

mesh.vertices = vertices;
mesh.uv = uv;
mesh.triangles = triangles;

ThisMesh.gameObject.AddComponent(MeshRenderer);
ThisMesh.GetComponent.<Renderer>().receiveShadows = false;
ThisMesh.GetComponent.<Renderer>().castShadows = false;

	if(!SelectedTarget[0]){
		ThisMesh.GetComponent.<Renderer>().enabled = false;
	}
	
ThisMesh.eulerAngles = ThisRotation;
ThisMesh.parent = ThisParent;
ThisMesh.gameObject.layer = ThisLayer;
ThisMesh.GetComponent.<Renderer>().sharedMaterial = RadarAtlasMaterial;

return ThisMesh;
}

function MakeVFB(Name : String, ThisParent : Transform, ThisLayer : int){
var ThisMesh : Transform = new GameObject(Name).transform;
var mesh : Mesh = ThisMesh.gameObject.AddComponent(MeshFilter).mesh;
var vertices : Vector3[]  = new Vector3[24];

//Face Down - TOP
vertices[0] = Vector3( -.5, .5, .5); //bottom left
vertices[1] =  Vector3( -.5, .5, -.5); // Top Left
vertices[2] =  Vector3(.5, .5, .5); // bottom right
vertices[3] =  Vector3(.5, .5, -.5); // Top right

//Face Up - Down
vertices[4] = Vector3( -.5, -.5, -.5); //bottom left
vertices[5] =  Vector3( -.5, -.5, .5); // Top Left
vertices[6] =  Vector3(.5, -.5, -.5); // bottom right
vertices[7] =  Vector3(.5, -.5, .5); // Top right

//Face Right - Left
vertices[8] = Vector3( -.5, -.5, .5); //bottom left
vertices[9] =  Vector3( -.5, -.5, -.5); // Top Left
vertices[10] =  Vector3(-.5, .5, .5); // bottom right
vertices[11] =  Vector3(-.5, .5, -.5); // Top right

//Face Left - Right
vertices[12] = Vector3( .5, -.5, -.5); //bottom left
vertices[13] =  Vector3( .5, -.5, .5); // Top Left
vertices[14] =  Vector3(.5, .5, -.5); // bottom right
vertices[15] =  Vector3(.5, .5, .5); // Top right

//Face Back
vertices[16] = Vector3( -.5, .5, -.5); //bottom left
vertices[17] =  Vector3( -.5, -.5, -.5); // Top Left
vertices[18] =  Vector3(.5, .5, -.5); // bottom right
vertices[19] =  Vector3(.5, -.5, -.5); // Top right

//Face Front
vertices[20] = Vector3( -.5, -.5, .5); //bottom left
vertices[21] =  Vector3( -.5, .5, .5); // Top Left
vertices[22] =  Vector3(.5, -.5, .5); // bottom right
vertices[23] =  Vector3(.5, .5, .5); // Top right

var uv : Vector2[] = new Vector2[16];
uv[0] =  Vector2(0, 0);
uv[1] =  Vector2(0, 1);
uv[2] =  Vector2(1, 0);
uv[3] =  Vector2(1, 1);

uv[4] =  Vector2(0, 0);
uv[5] =  Vector2(0, 1);
uv[6] =  Vector2(1, 0);
uv[7] =  Vector2(1, 1);

uv[8] =  Vector2(0, 0);
uv[9] =  Vector2(0, 1);
uv[10] =  Vector2(1, 0);
uv[11] =  Vector2(1, 1);

uv[12] =  Vector2(0, 0);
uv[13] =  Vector2(0, 1);
uv[14] =  Vector2(1, 0);
uv[15] =  Vector2(1, 1);

var triangles : int[]  = new int[24];

//Top
triangles[0] = 0;
triangles[1] = 1; 
triangles[2] = 2;
triangles[3] = 2; 
triangles[4] = 1; 
triangles[5] = 3;

//Bottom
triangles[6] = 4;
triangles[7] = 5; 
triangles[8] = 6;
triangles[9] = 6; 
triangles[10] = 5; 
triangles[11] = 7;

//Left
triangles[12] = 8;
triangles[13] = 9; 
triangles[14] = 10;
triangles[15] = 10; 
triangles[16] = 9; 
triangles[17] = 11;

//Right
triangles[18] = 12;
triangles[19] = 13; 
triangles[20] = 14;
triangles[21] = 14; 
triangles[22] = 13; 
triangles[23] = 15;

mesh.vertices = vertices;
mesh.triangles = triangles;

ThisMesh.parent = ThisParent;
ThisMesh.localPosition = Vector3.zero;
ThisMesh.localScale.x *= ((Screen.width  * 1.0) / Screen.height);
ThisMesh.gameObject.AddComponent(MeshCollider);
ThisMesh.gameObject.layer = ThisLayer;
VFBounds[0] = ThisMesh.localScale.x;
VFBounds[1] = ThisMesh.localScale.y;
}

/***********************************************************************************/
																//HUD Elements & Scale
/***********************************************************************************/
function CreateHUD(){
	
var HUDGUI : Transform = new GameObject("GUI_HUD_Icons").transform;
HUDGUI.parent = transform;
/***********************************************************************************/
											//Create, Cache & Configure HUD Camera
/***********************************************************************************/	
//Rift Camera Right	HUD
Cameras[2] = new GameObject("HUD / GUI Camera").AddComponent(Camera);
Cameras[2].gameObject.AddComponent(GUILayer);
Cameras[2].transform.parent = HUDGUI;
Cameras[2].transform.localPosition = Vector3.zero;
Cameras[2].transform.localEulerAngles = Vector3.zero;
Cameras[2].clearFlags = CameraClearFlags.Depth;
Cameras[2].depth = Cameras[1].depth + 1;
Cameras[2].fieldOfView = Cameras[4].fieldOfView;
Cameras[2].renderingPath = RenderingPath.Forward;
Cameras[2].cullingMask = 1 << Layers[2];
Cameras[2].orthographic = true;
Cameras[2].orthographicSize = Screen.height * .5;

	if(SetStatus[1]){
		//Rift Camera Left HUD
		Cameras[3] = new GameObject("HUD / GUI Camera 2").AddComponent(Camera);
		Cameras[3].gameObject.AddComponent(GUILayer);
		Cameras[3].transform.parent = HUDGUI;
		Cameras[3].transform.localPosition = Vector3.zero;
		Cameras[3].transform.localEulerAngles = Vector3.zero;
		Cameras[3].clearFlags = CameraClearFlags.Depth;
		Cameras[3].depth = Cameras[2].depth;
		Cameras[3].fieldOfView = Cameras[4].fieldOfView;
		Cameras[3].renderingPath = RenderingPath.Forward;
		Cameras[3].cullingMask = 1 << Layers[3];
		Cameras[3].orthographic = true;
		Cameras[3].orthographicSize = Screen.height * .5;
		
		Cameras[3].rect.width = .5;
		Cameras[2].rect.width = .5;
		
		Cameras[3].rect.x = 0;
		Cameras[2].rect.x = .5;
	}
	
/***********************************************************************************/
											//HUD Target Direction Indicator Arrow
/***********************************************************************************/
HUD_DIA[1] = new GameObject("HUD_DIA[1]").transform;
HUD_DIA[1].position = Vector3(1,0,1);
HUD_DIA[1].eulerAngles = Vector3(0,-90,0);
HUD_DIA[1].parent = HUDGUI;
HUD_DIA[0] = MakeTri("HUD_DIA[0]", 12, HUD_DIA[1], Layers[2]);
HUD_DIA[0].localPosition = Vector3(0, 0, DIARadius);
SetTextureOffset(14, 15, HUD_DIA[0].gameObject, 32);

var cp : Vector3 = Cameras[4].transform.position;
var cr : Vector3 = Cameras[4].transform.eulerAngles;
Cameras[4].transform.position = Vector3.zero;
Cameras[4].transform.eulerAngles = Vector3.zero;

var RelPos2 : Vector3 = (Cameras[2].WorldToScreenPoint(HUD_DIA[0].position));
DIAdisableAngle = Vector3.Angle(Cameras[4].ScreenToWorldPoint(RelPos2), Vector3(0,0,1));	

Cameras[4].transform.position = cp;
Cameras[4].transform.eulerAngles = cr;
/***********************************************************************************/
											//HUD Target Selection Box
/***********************************************************************************/
HUD_TSB[0] = MakeQuad("HUD_TSB", 64, 64, false, Vector3.zero, HUDGUI, Layers[2]);
SetTextureOffset(0, 0, HUD_TSB[0].gameObject, 64);

	if(SetStatus[1]){
		HUD_TSB[1] = MakeQuad("HUD_TSB", 64, 64, false, Vector3.zero, HUDGUI, Layers[3]);
		SetTextureOffset(0, 0, HUD_TSB[1].gameObject, 64);
	}
/***********************************************************************************/
											//HUD Target Selection Box Target ID
/***********************************************************************************/
HUD_TSB_ID[0] = MakeQuad("HUD_TSB_ID", 32, 32, false, Vector3.zero, HUDGUI, Layers[2]);

	if(SetStatus[1]){
		HUD_TSB_ID[1] = MakeQuad("HUD_TSB_ID", 32, 32, false, Vector3.zero, HUDGUI, Layers[3]);
	}
/***********************************************************************************/
											//HUD Target SubComponent Selection Box
/***********************************************************************************/
HUD_SC_TSB[0] = MakeQuad("HUD_SC_TSB", 64, 64, false, Vector3.zero, HUDGUI, Layers[2]);
SetTextureOffset(0, 1, HUD_SC_TSB[0].gameObject, 64);

	if(SetStatus[1]){
		HUD_SC_TSB[1] = MakeQuad("HUD_SC_TSB", 64, 64, false, Vector3.zero, HUDGUI, Layers[3]);
		SetTextureOffset(0, 1, HUD_SC_TSB[1].gameObject, 64);	
	}
/***********************************************************************************/
											//HUD NAV Indicator
/***********************************************************************************/
HUD_NAV[0] = MakeQuad("HUD_NAV", 64, 64, false, Vector3.zero, HUDGUI, Layers[2]);
SetTextureOffset(2, 1, HUD_NAV[0].gameObject, 64);
SetNewColor(HUD_NAV[0].gameObject, ColorNAV);

	if(SetStatus[1]){
		HUD_NAV[1] = MakeQuad("HUD_NAV", 64, 64, false, Vector3.zero, HUDGUI, Layers[3]);
		SetTextureOffset(2, 1, HUD_NAV[1].gameObject, 64);
		SetNewColor(HUD_NAV[1].gameObject, ColorNAV);
	}
/***********************************************************************************/
											//HUD Locator Indicator
/***********************************************************************************/
/*
HUD_Loc[0] = MakeQuad("HUD_Loc", 64, 64, false, Vector3.zero, HUDGUI, Layers[2]);
SetTextureOffset(2, 1, HUD_Loc[0].gameObject, 64);
SetNewColor(HUD_Loc[0].gameObject, ColorNAV);

	if(SetStatus[1]){
		HUD_Loc[1] = MakeQuad("HUD_Loc", 64, 64, false, Vector3.zero, HUDGUI, Layers[3]);
		SetTextureOffset(2, 1, HUD_Loc[1].gameObject, 64);
		SetNewColor(HUD_Loc[1].gameObject, ColorNAV);
	}
*/
/***********************************************************************************/
											//HUD Target Lead Indicator
/***********************************************************************************/
HUD_TLI[0] = MakeQuad("HUD_TLI", 64, 64, false, Vector3.zero, HUDGUI, Layers[2]);
SetTextureOffset(1, 1, HUD_TLI[0].gameObject, 64);

	if(SetStatus[1]){
		HUD_TLI[1] = MakeQuad("HUD_TLI", 64, 64, false, Vector3.zero, HUDGUI, Layers[3]);
		SetTextureOffset(1, 1, HUD_TLI[1].gameObject, 64);	
	}
/***********************************************************************************/
											//Radar Target Selection Box
/***********************************************************************************/
Radar_TSB = MakeQuad("Radar_TSB", RIDSizeOverride[0], RIDSizeOverride[0], true, Vector3(Cameras[1].transform.eulerAngles.x,0,0), HUDGUI, Layers[1]);
SetTextureOffset(0, 8, Radar_TSB.gameObject, 32);

/***********************************************************************************/
														//Create Status Bars
/***********************************************************************************/	
	if(SetStatus[20]){
		BarMaterial = new Material[StatusBar.Length];
		
		if(SetStatus[1]){
			StatusBar2 = new Transform[StatusBar.Length];
		}

			for(var b : int = 0; b < StatusBar.Length; b++){
				if(StatusBarType[b] == 0){
					BarSetup(b, BarAnchor[b],BarName[b], BarSize[b].x, BarSize[b].y, HUDGUI);
				}else{
					BarMaterial[b] = StatusBar[b].gameObject.GetComponent.<Renderer>().material;
				}
			}

	}
/***********************************************************************************/
														//Create & Configure VFB
/***********************************************************************************/		
MakeVFB("VFB", HUDGUI,  Layers[2]);
}

function ResetScale(){
ScreenSize = Vector2(Screen.width, Screen.height);

Destroy(GameObject.Find("GUI_HUD_Icons"));

	if(RadarPos == 5){
		VDIScale = (1 / ((Screen.height * Cameras[1].rect.height) / 1) * .5) * RIDSizeOverride[1];
	}else{
		VDIScale = (1 / ((Screen.height * Cameras[1].rect.height) / 1) * .5);
	}
	
CreateHUD();

	if(SelectedTarget[0]){
		SelectedTargetRID = SelectedTarget[0].GetComponent(FX_3DRadar_RID);
		SetTextureOffset(SelectedTargetRID.ThisClass[0], SelectedTargetRID.ThisClass[1] + 9, HUD_TSB_ID[0].gameObject, 32);
		if(SetStatus[1]){
			SetTextureOffset(SelectedTargetRID.ThisClass[0], SelectedTargetRID.ThisClass[1] + 9, HUD_TSB_ID[1].gameObject, 32);
		}
		GetNewColor();
	}

	var Objects : GameObject[] = FindObjectsOfType(GameObject);
	
	for(var o : GameObject in Objects){
		if (o.GetComponent(FX_3DRadar_RID)){
			o.GetComponent(FX_3DRadar_RID).ResetScale();
		}
	}
	
	if(NavList.Length > 0){
		HUD_NAV[0].GetComponent.<Renderer>().enabled = true;
		if(SetStatus[1]){
			HUD_NAV[1].GetComponent.<Renderer>().enabled = true;
		}
	}
}

//End