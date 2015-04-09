#pragma strict

enum _class {Misc, Civilian_Transport, Cargo_Transport, Drone, Fighter, Bomber, Escort, Frigate, Cruiser, Battleship, Dreadnaught, Capital, SpaceObject, Celestial, XO}
var Class : _class;

enum misc{Pilot, Misc_2, Misc_3, Misc_4, Misc_5, Misc_6, Misc_7}
var Misc : misc;

enum civT{CivT_1, CivT_2, CivT_3, CivT_4, CivT_5, CivT_6, CivT_7}
var CIVT : civT;

enum coT{CoT_1, CoT_2, CoT_3, CoT_4, CoT_5, CoT_6, CoT_7}
var COT : coT;

enum drone{Drone_1, Drone_2, Drone_3, Drone_4, Drone_5, Drone_6, Drone_7}
var Drone : drone;

enum fighter{Fighter_1, Fighter_2, Fighter_3, Fighter_4, Fighter_5, Fighter_6, Fighter_7}
var Fighter : fighter;

enum bomber{Bomber_1, Bomber_2, Bomber_3, Bomber_4, Bomber_5, Bomber_6, Bomber_7}
var Bomber : bomber;

enum escort{Escort_1, Escort_2, Escort_3,Escort_4, Escort_5, Escort_6, Escort_7}
var Escort : escort;

enum frigate{Frigate_1, Frigate_2, Frigate_3,Frigate_4, Frigate_5, Frigate_6, Frigate_7}
var Frigate : frigate;

enum cruiser{Cruiser_1, Cruiser_2, Cruiser_3, Cruiser_4, Cruiser_5, Cruiser_6, Cruiser_7}
var Cruiser : cruiser;

enum battleship{Battleship_1, Battleship_2, Battleship_3, Battleship_4, Battleship_5, Battleship_6, Battleship_7}
var BattleShip : battleship;

enum dreadnought{Dreadnought_1, Dreadnought_2, Dreadnought_3, Dreadnought_4, Dreadnought_5, Dreadnought_6, Dreadnought_7}
var Dreadnought : dreadnought;

enum capital{Capital_1, Capital_2, Capital_3, Capital_4, Capital_5, Capital_6, Capital_7}
var Capital : capital;

enum spaceObject{Station, satellite, Gate, Cargo_Container, Wreckage, SO_6, SO_7}
var SpaceObject : spaceObject;

enum celestial{Star, Planet, Moon, Asteroid, Celestial_5, Celestial_6, Celestial_7}
var Celestial : celestial;

enum xo{Mine, Missile, Torpedo, XO_4, XO_5, XO_6, XO_7}
var XO : xo;

enum iff{Abandoned, Neutral, Friendly, Hostile, Unknown, Owned, NAV, Objective}
var IFF : iff;

var EnableRadar : boolean;
var IndicatorEnabled : boolean;
var BoundsEnabled : boolean;
var IsDiscovered : boolean;
var IsDetectable : boolean = true;
var DetectionReset : boolean;
var PermDiscovery : boolean;
var BlindRadarOverride : boolean;
var IsPlayerTarget : boolean;
var IsAbandoned : boolean;
var IsPlayerOwned : boolean;
var IsPlayer : boolean;
var IsNAV : boolean;
var IsPOI : boolean;
var IsObjective : boolean;

var ThisClass : int[] = new int[5]; // 0 = Current Class, 1 = Current Sub Class, 2 = Previous Class, 3 = Previous Sub Class, 4 = Current IFF
var ThisFaction : int[] = new int[3]; //0 = Previous Faction, 1 = Current Faction, 2 = Current Faction ID

private var FX3DRM : FX_3DRadar_Mgr;
private var ThisID : Transform[] = new Transform[5]; // 0 = This Transform, 1 = Radar ID Tag, 2 = Radar ID Base, 3 = Radar ID VDI, 4 = Screen Indicator
private var IndicatorCorners : Transform[] = new Transform[4];
private var CurStatus : boolean[] = new boolean[13]; // 0 = RID Enabled, 1 = Is Player, 2 = Is Player Owned, 3 = Is Abandoned, 4 = Detectable, 5 = LOS, 6 = Is Obstructed, 7 = NAV Distance, 8 = Previous Bounds State, 9 = 2D Radar, 10 = Indicator Enabled 11 = Objective 12 = PIO Active
private var Timers : float[] = new float[4]; // 0 = Check Status Timer, 1 = LOS Timer, 2 = Reacquire Timer, 3 = Update Hostile List TImer
private var VDIvertices : Vector3[] = new Vector3[4];
private var VDIMesh : Mesh;
private var ThisPos : Vector3;
private var BoundsCorners : GameObject; // Need to dynamically generate this and the TL TR BL BR transforms

//Hostile List
var HostileList : List.<Transform> = new List.<Transform>();
var RadarRange : int = 100;
var UpdateHL : float = 10.0;

function Start(){
FX3DRM = GameObject.Find("/_GameMgr").GetComponent(FX_3DRadar_Mgr);

if(FX3DRM.SetStatus[1]){
ThisID = new Transform[6];
}

ThisID[0] = transform;
ThisFaction[0] = ThisFaction[1];
CurStatus[4] = !IsDetectable;

	if(ThisFaction[1] > FX3DRM.FXFM.Factions.Length - 1){
		ThisFaction[1] = 0;
		ThisFaction[2] = FX3DRM.FXFM.FactionID[ThisFaction[1]];
	}else{
		ThisFaction[2] = FX3DRM.FXFM.FactionID[ThisFaction[1]];
	}

	if(FX3DRM.BoundsShow == 1){
		PermDiscovery = false;
	}else if(FX3DRM.BoundsShow == 2){
		PermDiscovery = true;
	}
	
	if(!IsNAV){
		if(IsPlayer){
			SetAsPlayer();
		}else{
			SetAsAI();
		}
	}else{
		BoundsEnabled = false;
		SetAsNAV();
	}

SetIFFStatus();
CreateBounds();
CreateRadarID();
}

function LateUpdate(){
ThisPos = ThisID[0].position;
	
	if(IsNAV){
		UpdateNAV();
	}else{
		
		if(FX3DRM.LocalTime > Timers[0]){
			CheckStatus();
			Timers[0] = FX3DRM.LocalTime + 1;
		}
		
		if(IsPlayer || FX3DRM.FilterHostile && IFF != 3){
			if(IsPOI || CurStatus[0]){
				DisableIndicator();
				DisableBounds();
				DisableRID();
			}
			return;
		}else if(EnableRadar && !IsAbandoned && FX3DRM.LocalTime > Timers[3]){
			BuildHostileList();
		}
		UpdateRID();
		UpdateIndicator();
		UpdateBounds();
	}
}

function UpdateNAV(){
var Distance : float = ((FX_3DRadar_Mgr.PlayerPos - ThisPos).sqrMagnitude);

	if(Distance < FX3DRM.RadarRange[1] && FX3DRM.HUDElements[1]){
		if(CurStatus[7]){
			ThisID[2].GetComponent.<Renderer>().enabled = true;
			ThisID[3].GetComponent.<Renderer>().enabled = true;
			CurStatus[7] = false;
		}
		RID();
	}else{
		if(!CurStatus[7]){
			ThisID[2].GetComponent.<Renderer>().enabled = false;
			ThisID[3].GetComponent.<Renderer>().enabled = false;
			CurStatus[7] = true;
		}
		RenderPOI();
	}
}

function UpdateBounds(){

	if(IsDetectable && !BoundsEnabled && FX3DRM.HUDElements[2]){

		if(IsPlayerTarget || FX3DRM.FilterHostile && IFF != 3){
			DisableBounds();
			return;
		}
		
		if(PermDiscovery && IsDiscovered || CurStatus[0] || IsPOI){
			DrawBounds(4);
			if(FX3DRM.SetStatus[1]){
				DrawBounds(5);
			}
		}else{
			DisableBounds();
		}				
		
	}else{
		DisableBounds();
	}
}

function UpdateIndicator(){
	if(!IsPlayerTarget && FX3DRM.SetStatus[13] && !IndicatorEnabled && CurStatus[0] || IsPOI && !IndicatorEnabled){
 	
	 	if(FX3DRM.SetStatus[15] && !CurStatus[10]){
			EnableIndicator();
		}else{
			ThisID[4].position = IndicatorPositions(ThisID[0].position, 2,4);
			if(FX3DRM.SetStatus[1]){
				ThisID[5].position = IndicatorPositions(ThisID[0].position, 3,4);
			}		
		}
		
	}else if(CurStatus[10]){
		DisableIndicator();
	}
}

function IndicatorPositions(NewPos : Vector3, x : int, y : int){
var ISP : Vector3;
var TRelPos : Vector3 = FX3DRM.Transforms[x].InverseTransformPoint(NewPos);
var ScreenPos : Vector3 = FX3DRM.Cameras[y].WorldToViewportPoint(NewPos);
var Hit : RaycastHit;
	
	if(ScreenPos.x > 1 || ScreenPos.x < 0 || ScreenPos.y > 1 || ScreenPos.y < 0 || ScreenPos.z <= 0.01){
		if(!CurStatus[10]){
			EnableIndicator();
		}		
		
		if(ScreenPos.x == .5 && ScreenPos.y == .5){
			TRelPos.y = 1;
		}

		Physics.Raycast (Vector3.zero, Vector3(TRelPos.x, TRelPos.y, 0), Hit, 2, 1 << FX3DRM.Layers[2]);
		ISP = Vector3(((FX3DRM.VFBounds[0] * 0.5) + Hit.point.x) / FX3DRM.VFBounds[0], (0.5 + Hit.point.y), 0 );
	}else{	
		 if(!FX3DRM.SetStatus[15]){
			 if(CurStatus[10]){
				DisableIndicator();
			}
			return;
		}else{
			if(!CurStatus[10]){
				EnableIndicator();
			}
		ISP = ScreenPos;
		}
	}

ISP = FX3DRM.Cameras[x].ViewportToScreenPoint(ISP);
	
	if(FX3DRM.SetStatus[1]){
		if(x == 2){
			var a1 : float = (FX3DRM.ScreenSize.x * .5) + FX3DRM.EdgePadding[2];
			if(ISP.x >= FX3DRM.ScreenSize.x - FX3DRM.EdgePadding[2]){
				ISP.x = FX3DRM.ScreenSize.x - FX3DRM.EdgePadding[2];
			}else if(ISP.x <= a1){
				ISP.x = a1;
				
			}		
		}else{
			var a2 : float = (FX3DRM.ScreenSize.x * .5) - FX3DRM.EdgePadding[2];
			if(ISP.x >= a2){
				ISP.x = a2;
			}else if(ISP.x <= FX3DRM.EdgePadding[2]){
				ISP.x = FX3DRM.EdgePadding[2];
			}			
		}
	}else{
		var a3 : float = FX3DRM.ScreenSize.x - FX3DRM.EdgePadding[2];
		if(ISP.x >= a3){
			ISP.x = a3;
		}else if(ISP.x <= FX3DRM.EdgePadding[2]){
			ISP.x =FX3DRM.EdgePadding[2];
		}
	}
	
	var a4 : float = FX3DRM.ScreenSize.y - FX3DRM.EdgePadding[3];
	if(ISP.y >= a4){
		ISP.y = a4;
	}else if(ISP.y <= FX3DRM.EdgePadding[3]){
		ISP.y = FX3DRM.EdgePadding[3];
	}

ISP = Vector3(Mathf.Round(ISP.x), Mathf.Round(ISP.y), 1);
ISP = FX3DRM.Cameras[x].ScreenToWorldPoint(Vector3(ISP.x + .5, ISP.y, .9));

return ISP;
}

function UpdateRID(){
	if(IsDetectable){
		if(IsPOI && !CurStatus[12]){
			SetPOIRender();
		}
		var Distance : float = ((FX_3DRadar_Mgr.PlayerPos - ThisPos).sqrMagnitude);
		if(Distance < FX3DRM.RadarRange[1]){
			if(FX3DRM.SetStatus[2] && !BlindRadarOverride){
				var hit : RaycastHit;
				if(FX3DRM.LocalTime > Timers[1]){
					if(!Physics.Linecast(ThisPos, FX_3DRadar_Mgr.PlayerPos, hit, FX3DRM.Layers[5]) || hit.transform == FX3DRM.Transforms[0]){
						if(!CurStatus[0]){
							RID();
						}
						if(CurStatus[6] && FX3DRM.LocalTime <= Timers[2]){
							FX3DRM.SelectedTarget[0] = ThisID[0];
							FX3DRM.SetTarget();
							CurStatus[6] = false;
						}else if(CurStatus[6]){
							CurStatus[6] = false;
						}
					}else if(CurStatus[0]){
						Timers[2] = FX3DRM.LocalTime + FX3DRM.Timers[1];
						
						if(IsPlayerTarget){
							CurStatus[6] = true;
						}
						DisableRID();
					}
					Timers[1] = FX3DRM.LocalTime + FX3DRM.Timers[0];
				}
				if(CurStatus[0]){
					RID();
				}
			}else{
				RID();
			}
		}else{
			if(!PermDiscovery && !IsPOI){
				IsDiscovered = false;
			}
			if(!FX3DRM.FilterHostile && IsPOI && !CurStatus[12]){
				EnableRID();
			}
			if(CurStatus[0] || !IsPOI && CurStatus[12]){
				DisableRID();
			}
	
			if(IsPOI){
				if(!CurStatus[12]){
					SetPOIRender();
				}
				RenderPOI();
			}
		}
	}else{
		if(DetectionReset){
			IsDiscovered = false;
		}
		
		if(CurStatus[0]){
			DisableRID();
		}
		if(IsPOI && CurStatus[12]){
			DisableRID();
		}
	}
}

function RID(){
	
	if(!CurStatus[0]){
		if(IFF == 3){
			FX3DRM.PlayWarningSound();
		}
		EnableRID();
	}
			
	if(FX3DRM.SetStatus[3]){
		RenderPerspective();
	}else{
		RenderOrthographic();
	}
	
Set2DMode();
}

function RenderPerspective(){
var RelPos : Vector3 = FX3DRM.Transforms[0].InverseTransformPoint(ThisPos) * FX3DRM.RadarRange[3];
var newPosA : Vector3 = FX3DRM.Cameras[0].WorldToScreenPoint(RelPos);
newPosA = Vector3(Mathf.Round(newPosA.x), Mathf.Round(newPosA.y), newPosA.z);

	if(!FX3DRM.SetStatus[7]){
		var newRidPos : Vector3 = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosA.x + .5, newPosA.y + .5, newPosA.z));
		ThisID[1].position = newRidPos;
		
		if(IsPlayerTarget){
			FX3DRM.Radar_TSB.position = newRidPos;
		}
		
		if(!FX3DRM.HUDElements[4] && !FX3DRM.SetStatus[7]){
			if(!FX3DRM.HUDElements[5]){
				var newPosB : Vector3 = FX3DRM.Cameras[0].WorldToScreenPoint(Vector3(RelPos.x, 0, RelPos.z));
				newPosB = Vector3(Mathf.Round(newPosB.x), Mathf.Round(newPosB.y), newPosB.z);
				
				ThisID[2].position = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosB.x + .5, newPosB.y + .5, newPosB.z));
			}
			
			var ThisVDIPos1 : Vector3 = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosA.x, newPosA.y, newPosA.z));
			var ThisVDIPos2 : Vector3 = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosB.x, newPosB.y, newPosB.z));
				
				if(RelPos.y > 0){		
					VDIvertices[0] = Vector3(ThisVDIPos1.x + FX3DRM.VDIScale, ThisVDIPos1.y, ThisVDIPos1.z); //bottom left
					VDIvertices[1] =  Vector3(ThisVDIPos2.x + FX3DRM.VDIScale, ThisVDIPos2.y, ThisVDIPos2.z); // Top Left
					VDIvertices[2] =  Vector3(ThisVDIPos1.x - FX3DRM.VDIScale, ThisVDIPos1.y, ThisVDIPos1.z); // bottom right
					VDIvertices[3] =  Vector3(ThisVDIPos2.x - FX3DRM.VDIScale, ThisVDIPos2.y, ThisVDIPos2.z); // Top right
				}else{
					VDIvertices[1] = Vector3(ThisVDIPos1.x + FX3DRM.VDIScale, ThisVDIPos1.y, ThisVDIPos1.z); //bottom left
					VDIvertices[0] =  Vector3(ThisVDIPos2.x + FX3DRM.VDIScale, ThisVDIPos2.y, ThisVDIPos2.z); // Top Left
					VDIvertices[3] =  Vector3(ThisVDIPos1.x - FX3DRM.VDIScale, ThisVDIPos1.y, ThisVDIPos1.z); // bottom right
					VDIvertices[2] =  Vector3(ThisVDIPos2.x - FX3DRM.VDIScale, ThisVDIPos2.y, ThisVDIPos2.z); // Top right	
				}
			
			VDIMesh.vertices = VDIvertices;
		}
	}else{
		var newPosD : Vector3 = FX3DRM.Cameras[0].WorldToScreenPoint(Vector3(RelPos.x, 0, RelPos.z));
		newPosD = Vector3(Mathf.Round(newPosD.x), Mathf.Round(newPosD.y), newPosD.z);
		newPosD = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosD.x + .5, newPosD.y + .5, newPosD.z));
		ThisID[1].position = newPosD;
		
		if(IsPlayerTarget){
			FX3DRM.Radar_TSB.position = newPosD;
		}		
	}
}

function RenderOrthographic(){
var RelPos : Vector3 = FX3DRM.Transforms[0].InverseTransformPoint(ThisPos) * FX3DRM.RadarRange[3];
var newPosA : Vector3 = FX3DRM.Cameras[1].WorldToScreenPoint(RelPos);
newPosA = Vector3(Mathf.Round(newPosA.x), Mathf.Round(newPosA.y), newPosA.z);
	
	if(!FX3DRM.SetStatus[7]){
		var newRidPos : Vector3 = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosA.x + .5, newPosA.y + .5, 1));
		ThisID[1].position = newRidPos;
		
		if(IsPlayerTarget){
			FX3DRM.Radar_TSB.position = newRidPos;
		}
	
		if(!FX3DRM.HUDElements[4] && !FX3DRM.SetStatus[7]){
			if(!FX3DRM.HUDElements[5]){
				var newPosB : Vector3 = FX3DRM.Cameras[1].WorldToScreenPoint(Vector3(RelPos.x, 0, RelPos.z));
				newPosB = Vector3(Mathf.Round(newPosB.x), Mathf.Round(newPosB.y), newPosB.z);
				
				ThisID[2].position = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosA.x + .5, newPosB.y + .5, 1));
			}
			ThisID[3].position.x = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3((newPosA.x),newPosA.y, 1)).x;
			
				if(RelPos.y > 0){
					VDIvertices[0] = Vector3(FX3DRM.VDIScale, RelPos.y, RelPos.z); //bottom left
					VDIvertices[1] =  Vector3(FX3DRM.VDIScale, 0, RelPos.z); // Top Left
					VDIvertices[2] =  Vector3(-FX3DRM.VDIScale, RelPos.y, RelPos.z); // bottom right
					VDIvertices[3] =  Vector3(-FX3DRM.VDIScale, 0, RelPos.z); // Top right
				}else{
					VDIvertices[0] = Vector3(FX3DRM.VDIScale,0, RelPos.z); //bottom left
					VDIvertices[1] =  Vector3(FX3DRM.VDIScale, RelPos.y, RelPos.z); // Top Left
					VDIvertices[2] =  Vector3(-FX3DRM.VDIScale, 0, RelPos.z); // bottom right
					VDIvertices[3] =  Vector3(-FX3DRM.VDIScale, RelPos.y, RelPos.z); // Top right	
				}
				
			VDIMesh.vertices = VDIvertices;
		}
	}else{
		var newPosC : Vector3 = FX3DRM.Cameras[1].WorldToScreenPoint(Vector3(RelPos.x, 0, RelPos.z));
		newPosC = Vector3(Mathf.Round(newPosC.x), Mathf.Round(newPosC.y), newPosC.z);
		newPosC = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosA.x + .5, newPosC.y + .5, 1));
		ThisID[1].position = newPosC;
		
		if(IsPlayerTarget){
			FX3DRM.Radar_TSB.position = newPosC;
		}
	}
}

function RenderPOI(){
var RelPos : Vector3 = FX3DRM.Transforms[0].InverseTransformPoint(ThisPos) * FX3DRM.RadarRange[3];
var newPosA : Vector3;

	if(FX3DRM.SetStatus[3]){
		newPosA = FX3DRM.Cameras[0].WorldToScreenPoint(RelPos.normalized * .5);
		newPosA = Vector3(Mathf.Round(newPosA.x), Mathf.Round(newPosA.y), newPosA.z);
		newPosA = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosA.x + .5, newPosA.y + .5, newPosA.z));
		ThisID[1].position = newPosA;
		if(IsPlayerTarget){
			FX3DRM.Radar_TSB.position = newPosA;
		}
	}else{
		newPosA = FX3DRM.Cameras[1].WorldToScreenPoint(RelPos.normalized * .5);
		newPosA = Vector3(Mathf.Round(newPosA.x), Mathf.Round(newPosA.y), newPosA.z);
		newPosA = FX3DRM.Cameras[1].ScreenToWorldPoint(Vector3(newPosA.x + .5, newPosA.y + .5, 1));
		ThisID[1].position = newPosA;
		if(IsPlayerTarget){
			FX3DRM.Radar_TSB.position = newPosA;
		}
	}
}

function DrawBounds(x : int){
	
	var center : Vector3 = ThisPos;
	var Max : Vector3;
	var Min : Vector3;
			
	if(Vector3.Dot(FX3DRM.Transforms[2].TransformDirection(Vector3.forward), (center - FX3DRM.Transforms[2].position)) > 0){ // Check if the object is in front of the camera. If not then disable the corners & do not execute the code.
		EnableBounds();
								
		var ThisBounds : Vector3;
				
		if(FX3DRM.BoundsSize == 0){
			ThisBounds = GetComponent.<Renderer>().bounds.extents * FX3DRM.BoundsPadding;
		}else{
			ThisBounds = Vector3(1,1,1) * FX3DRM.BoundsPadding;
		}

		if(FX3DRM.BoundsCalculation == 0){ // Calculation Method 1 : Do a basic calculation for finding 4 points around the center of the object & convert there world positions to viewport space
			var corner1 : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint((center + Vector2(ThisBounds.x, ThisBounds.y)));
			var corner2 : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint((center + Vector2(-ThisBounds.x, ThisBounds.y)));
			var corner3 : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint((center + Vector2(ThisBounds.x, -ThisBounds.y)));
			var corner4 : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint((center + Vector2(-ThisBounds.x, -ThisBounds.y)));
					
			//Find the Left, Right, Top, & Bottom most points
			Min.x = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
			Max.x = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);		
			Max.y = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);
			Min.y = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
		}else{ // Calculation Method 2 : Do a more advanced calculation for finding all 8 points of the objects bounds & convert there world positions to viewport space
			var TopFrontLeft : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint(Vector3(center.x + ThisBounds.x, center.y + ThisBounds.y, center.z - ThisBounds.z));
			var TopFrontRight : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint(Vector3(center.x - ThisBounds.x, center.y + ThisBounds.y, center.z - ThisBounds.z));
			var TopBackLeft : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint((center + ThisBounds));
			var TopBackRight : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint(Vector3(center.x - ThisBounds.x, center.y + ThisBounds.y, center.z + ThisBounds.z));
			var BottomFrontLeft : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint(Vector3(center.x + ThisBounds.x, center.y - ThisBounds.y, center.z - ThisBounds.z));
			var BottomFrontRight : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint((center - ThisBounds));
			var BottomBackLeft : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint(Vector3(center.x + ThisBounds.x, center.y - ThisBounds.y, center.z + ThisBounds.z));
			var BottomBackRight : Vector3 = FX3DRM.Cameras[x].WorldToViewportPoint(Vector3(center.x - ThisBounds.x, center.y - ThisBounds.y, center.z + ThisBounds.z));
					
			//Find the Left, Right, Top, & Bottom most points
			Max.x = Mathf.Max(TopFrontLeft.x, TopBackLeft.x, TopFrontRight.x, TopBackRight.x, BottomFrontLeft.x, BottomBackLeft.x, BottomFrontRight.x, BottomBackRight.x);
			Min.x = Mathf.Min(TopFrontLeft.x, TopBackLeft.x, TopFrontRight.x, TopBackRight.x, BottomFrontLeft.x, BottomBackLeft.x, BottomFrontRight.x, BottomBackRight.x);
			Max.y = Mathf.Max(TopFrontLeft.y, TopBackLeft.y, TopFrontRight.y, TopBackRight.y, BottomFrontLeft.y, BottomBackLeft.y, BottomFrontRight.y, BottomBackRight.y);
			Min.y = Mathf.Min(TopFrontLeft.y, TopBackLeft.y, TopFrontRight.y, TopBackRight.y, BottomFrontLeft.y, BottomBackLeft.y, BottomFrontRight.y, BottomBackRight.y);
		}
				
		if(FX3DRM.SetStatus[4]){
			var centerPos : Vector2 = FX3DRM.Cameras[x].WorldToViewportPoint(center);
			var PreComputeMinMax : Vector2 = Vector2(FX3DRM.MaxSize[0] * .5, FX3DRM.HUDOpacity[3] * .5);
			
			if(Max.x - Min.x >= FX3DRM.MaxSize[0]){
			  Max.x = centerPos.x + PreComputeMinMax.x;
			  Min.x = centerPos.x - PreComputeMinMax.x;
			}
		
			if(Max.y - Min.y >= FX3DRM.MaxSize[1]){
			 Max.y = centerPos.y + PreComputeMinMax.y;
			  Min.y = centerPos.y - PreComputeMinMax.y;
			}
		}
		
		Max = FX3DRM.Cameras[x].ViewportToScreenPoint(Vector3(Max.x, Max.y, 1));
		Min = FX3DRM.Cameras[x].ViewportToScreenPoint(Vector3(Min.x, Min.y, 1));
		
		Max = Vector3(Mathf.Round(Max.x) +.5, Mathf.Round(Max.y) + .5, 1);
		Min = Vector3(Mathf.Round(Min.x) + .5, Mathf.Round(Min.y) +.5, 1);
		
		//Position our corners from viewport space back into world space
		if(x == 4){
			IndicatorCorners[0].position = FX3DRM.Cameras[2].ScreenToWorldPoint(Vector3(Min.x, Max.y, 1));
			IndicatorCorners[1].position = FX3DRM.Cameras[2].ScreenToWorldPoint(Vector3(Max.x, Max.y, 1));
			IndicatorCorners[2].position = FX3DRM.Cameras[2].ScreenToWorldPoint(Vector3(Min.x, Min.y, 1));
			IndicatorCorners[3].position = FX3DRM.Cameras[2].ScreenToWorldPoint(Vector3(Max.x, Min.y, 1));
		}else	 if(x == 5 ){
			IndicatorCorners[4].position = FX3DRM.Cameras[3].ScreenToWorldPoint(Vector3(Min.x, Max.y, 1));
			IndicatorCorners[5].position = FX3DRM.Cameras[3].ScreenToWorldPoint(Vector3(Max.x, Max.y, 1));
			IndicatorCorners[6].position = FX3DRM.Cameras[3].ScreenToWorldPoint(Vector3(Min.x, Min.y, 1));
			IndicatorCorners[7].position = FX3DRM.Cameras[3].ScreenToWorldPoint(Vector3(Max.x, Min.y, 1));		
		}		
		
	}else{
		DisableBounds();
	}
}

/***********************************************************************************/
															//Called Functions
/***********************************************************************************/
function DestroyThis(){
	if(!IsNAV){
		Destroy(BoundsCorners);
		FX3DRM.RemoveFromList(ThisID[0]);
	}
		
	Destroy(ThisID[1].gameObject);
	Destroy(ThisID[2].gameObject);
	Destroy(ThisID[3].gameObject);
	Destroy(ThisID[4].gameObject);
	if(FX3DRM.SetStatus[1]){
		Destroy(ThisID[5].gameObject);
	}
	
	if(IsPlayerTarget){
		FX3DRM.ClearTarget();
	}
	
Destroy(gameObject);
}

function EnableRID(){
	if(!FX3DRM.SetStatus[7]){
		ThisID[2].GetComponent.<Renderer>().enabled = true;
		ThisID[3].GetComponent.<Renderer>().enabled = true;
	}

ThisID[1].gameObject.SetActive(true);
CurStatus[0] = true;
IsDiscovered = true;
UpdatePlayerContacts();
SetLayer();
}

function DisableRID(){
CurStatus[0] = false;
CurStatus[12] = false;

	if(!FX3DRM.FilterHostile){
		ThisID[1].gameObject.SetActive(false);
		
		if(!IsPOI || !IsDetectable){
			if(IsPlayerTarget){
				FX3DRM.ClearTarget();
				IsPlayerTarget = false;
			}
			
			UpdatePlayerContacts();
		}
		
		ThisID[2].GetComponent.<Renderer>().enabled = false;
		ThisID[3].GetComponent.<Renderer>().enabled = false;
	}else{
		if(IsPlayerTarget && IFF != 3){
			FX3DRM.ClearTarget();
			IsPlayerTarget = false;	
		}
		
		UpdatePlayerContacts();
		
		ThisID[1].gameObject.SetActive(false);
		ThisID[2].GetComponent.<Renderer>().enabled = false;
		ThisID[3].GetComponent.<Renderer>().enabled = false;
	}
	SetLayer();
}

function SetPOIRender(){
CurStatus[0] = false;
ThisID[1].gameObject.SetActive(true);
ThisID[2].GetComponent.<Renderer>().enabled = false;
ThisID[3].GetComponent.<Renderer>().enabled = false;
CurStatus[12] = true;
SetLayer();
}

function EnableBounds(){
	if(!CurStatus[8]){
		BoundsCorners.SetActive(true);
		CurStatus[8] = true;
	}
}

function DisableBounds(){
	if(CurStatus[8]){
		BoundsCorners.SetActive(false);
		CurStatus[8] = false;
	}
}

function EnableIndicator(){
ThisID[4].gameObject.SetActive(true);
	if(FX3DRM.SetStatus[1]){
		ThisID[5].gameObject.SetActive(true);
	}
CurStatus[10] = true;
}

function DisableIndicator(){
ThisID[4].gameObject.SetActive(false);
	if(FX3DRM.SetStatus[1]){
		ThisID[5].gameObject.SetActive(false);
	}
CurStatus[10] = false;
}

function CreateRadarID(){
var Parent : Transform = GameObject.Find("RadarTags").transform;
/***********************************************************************************/
															//Create This Radar ID
/***********************************************************************************/
ThisID[1] = FX3DRM.MakeQuad("ID_Tag", FX3DRM.RIDSizeOverride[0], FX3DRM.RIDSizeOverride[0], true, Vector3(FX3DRM.Cameras[1].transform.eulerAngles.x,0,0), Parent, FX3DRM.Layers[1]);
ThisID[2] = FX3DRM.MakeQuad("ID_Base", FX3DRM.RIDSizeOverride[0], FX3DRM.RIDSizeOverride[0], true, Vector3(FX3DRM.Cameras[1].transform.eulerAngles.x,0,0), Parent, FX3DRM.Layers[1]);
ThisID[3] = FX3DRM.MakeQuad("VDI", 1, 1, true, Vector3.zero, Parent, FX3DRM.Layers[1]);

ThisID[1].GetComponent.<Renderer>().enabled = true;
ThisID[1].gameObject.SetActive(false);

	if(FX3DRM.SetStatus[17]){
		ThisID[1].gameObject.AddComponent(FX_3DRadar_Info).ThisParent = ThisID[0];
		ThisID[1].gameObject.AddComponent(BoxCollider);
		ThisID[1].gameObject.AddComponent(Rigidbody);
		ThisID[1].GetComponent.<Rigidbody>().isKinematic = true;
	}

	if(!FX3DRM.SetStatus[14]){
		ThisID[4] = FX3DRM.MakeQuad("HUD Indicator", 64, 64, false, Vector3.zero, Parent, FX3DRM.Layers[2]);
		if(FX3DRM.SetStatus[1]){
			ThisID[5] = FX3DRM.MakeQuad("HUD Indicator", 64, 64, false, Vector3.zero, Parent, FX3DRM.Layers[3]);
		}
	}else{
		ThisID[4] = FX3DRM.MakeQuad("HUD Indicator", 32, 32, false, Vector3.zero, Parent, FX3DRM.Layers[2]);
		if(FX3DRM.SetStatus[1]){
			ThisID[5] = FX3DRM.MakeQuad("HUD Indicator", 32, 32, false, Vector3.zero, Parent, FX3DRM.Layers[3]);
		}
	}
	if(FX3DRM.SetStatus[13]){
		ThisID[4].GetComponent.<Renderer>().enabled = true;
		if(FX3DRM.SetStatus[1]){
			ThisID[5].GetComponent.<Renderer>().enabled = true;
		}
	}else{
		ThisID[4].GetComponent.<Renderer>().enabled = false;
		if(FX3DRM.SetStatus[1]){
			ThisID[5].GetComponent.<Renderer>().enabled = true;
		}
	}

	if(FX3DRM.SetStatus[16]){	
		ThisID[4].gameObject.AddComponent(BoxCollider);
		ThisID[4].gameObject.AddComponent(Rigidbody);
		ThisID[4].GetComponent.<Rigidbody>().isKinematic = true;
		ThisID[4].gameObject.AddComponent(FX_3DRadar_Info).ThisParent = ThisID[0];
		if(FX3DRM.SetStatus[1]){
		ThisID[5].gameObject.AddComponent(BoxCollider);
		ThisID[5].gameObject.AddComponent(Rigidbody);
		ThisID[5].GetComponent.<Rigidbody>().isKinematic = true;
		ThisID[5].gameObject.AddComponent(FX_3DRadar_Info).ThisParent = ThisID[0];		
		}
	}
	
VDIMesh = ThisID[3].GetComponent(MeshFilter).mesh;

GetNewTexture();
GetNewColor();

	var Distance : float = ((FX_3DRadar_Mgr.PlayerPos - ThisPos).sqrMagnitude);
	if(Distance < FX3DRM.RadarRange[1]){
		EnableRID();
	}else{
		DisableRID();
	}
}

function CreateBounds(){
	if(!IsNAV){
		//Create our corners
		BoundsCorners = new GameObject("BoundsContainer");
		BoundsCorners.transform.parent = GameObject.Find("BoundsCorners").transform;
		BoundsCorners.transform.localPosition = Vector3.zero;
		/***********************************************************************************/
													//Create Bounds Corners
		/***********************************************************************************/
			if(FX3DRM.SetStatus[1]){
				IndicatorCorners = new Transform[8];
				IndicatorCorners[4] = FX3DRM.MakeQuad("TL", 32, 32, false,  Vector3(0, 0, 270), BoundsCorners.transform, FX3DRM.Layers[3]);
				IndicatorCorners[5] = FX3DRM.MakeQuad("TR", 32, 32, false, Vector3(0, 0, 180), BoundsCorners.transform, FX3DRM.Layers[3]);
				IndicatorCorners[6] = FX3DRM.MakeQuad("BL", 32, 32, false, Vector3(0, 0, 0), BoundsCorners.transform, FX3DRM.Layers[3]);
				IndicatorCorners[7] = FX3DRM.MakeQuad("BR", 32, 32, false, Vector3(0, 0, 90), BoundsCorners.transform, FX3DRM.Layers[3]);
		
				IndicatorCorners[4].GetComponent.<Renderer>().enabled = true;
				IndicatorCorners[5].GetComponent.<Renderer>().enabled = true;
				IndicatorCorners[6].GetComponent.<Renderer>().enabled = true;
				IndicatorCorners[7].GetComponent.<Renderer>().enabled = true;
			}
				
		IndicatorCorners[0] = FX3DRM.MakeQuad("TL", 32, 32, false,  Vector3(0, 0, 270), BoundsCorners.transform, FX3DRM.Layers[2]);
		IndicatorCorners[1] = FX3DRM.MakeQuad("TR", 32, 32, false, Vector3(0, 0, 180), BoundsCorners.transform, FX3DRM.Layers[2]);
		IndicatorCorners[2] = FX3DRM.MakeQuad("BL", 32, 32, false, Vector3(0, 0, 0), BoundsCorners.transform, FX3DRM.Layers[2]);
		IndicatorCorners[3] = FX3DRM.MakeQuad("BR", 32, 32, false, Vector3(0, 0, 90), BoundsCorners.transform, FX3DRM.Layers[2]);
		
		IndicatorCorners[0].GetComponent.<Renderer>().enabled = true;
		IndicatorCorners[1].GetComponent.<Renderer>().enabled = true;
		IndicatorCorners[2].GetComponent.<Renderer>().enabled = true;
		IndicatorCorners[3].GetComponent.<Renderer>().enabled = true;
	}
}

function GetNewColor(){
var UseColor : Color = FX3DRM.GetIFFColor(IFF);

FX3DRM.SetNewColor(ThisID[1].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[1]));

	if(!FX3DRM.HUDElements[4]){
		if(!FX3DRM.HUDElements[5]){
			FX3DRM.SetNewColor(ThisID[2].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[2]));					
		}else{
			FX3DRM.SetNewColor(ThisID[2].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, 0.0));
		}
		FX3DRM.SetNewColor(ThisID[3].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[2]));
	}else{
		FX3DRM.SetNewColor(ThisID[2].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, 0.0));
		FX3DRM.SetNewColor(ThisID[3].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, 0.0));
	}

	FX3DRM.SetNewColor(ThisID[4].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[4]));
	if(FX3DRM.SetStatus[1]){
		FX3DRM.SetNewColor(ThisID[5].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[4]));
	}
		
	if(!IsNAV){
		FX3DRM.SetNewColor(IndicatorCorners[0].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[3]));
		FX3DRM.SetNewColor(IndicatorCorners[1].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[3]));
		FX3DRM.SetNewColor(IndicatorCorners[2].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[3]));
		FX3DRM.SetNewColor(IndicatorCorners[3].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[3]));
		
		if(FX3DRM.SetStatus[1]){
			FX3DRM.SetNewColor(IndicatorCorners[4].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[3]));
			FX3DRM.SetNewColor(IndicatorCorners[5].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[3]));
			FX3DRM.SetNewColor(IndicatorCorners[6].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[3]));
			FX3DRM.SetNewColor(IndicatorCorners[7].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, FX3DRM.HUDOpacity[3]));		
		}
	}else if(!FX3DRM.HUDElements[1]){
		FX3DRM.SetNewColor(ThisID[1].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, 0.0));
		FX3DRM.SetNewColor(ThisID[2].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, 0.0));
		FX3DRM.SetNewColor(ThisID[3].gameObject, Color(UseColor.r, UseColor.g, UseColor.b, 0.0));
	}
	
	if(ThisClass[2] != ThisClass[0] || ThisClass[2] != ThisClass[1]){
		GetNewTexture();
		ThisClass[2] = ThisClass[0];
		ThisClass[3] = ThisClass[1];
	}
ThisClass[4] = IFF;
}

function GetNewTexture(){
	if(IFF == 6){ // Nav
		FX3DRM.SetTextureOffset(1, 8, ThisID[1].gameObject, 32);
	}else{
		FX3DRM.SetTextureOffset(ThisClass[0], ((ThisClass[1] + 9)), ThisID[1].gameObject, 32);
	}

FX3DRM.SetTextureOffset(15, 15, ThisID[2].gameObject, 32);
FX3DRM.SetTextureOffset(14, 15, ThisID[3].gameObject, 32);
	
	
	if(!FX3DRM.SetStatus[14]){
		FX3DRM.SetTextureOffset(3, 1, ThisID[4].gameObject, 64);
		if(FX3DRM.SetStatus[1]){
			FX3DRM.SetTextureOffset(3, 1, ThisID[5].gameObject, 64);
		}
	}else{
		FX3DRM.SetTextureOffset(ThisClass[0], ((ThisClass[1] + 9)), ThisID[4].gameObject, 32);
		if(FX3DRM.SetStatus[1]){
			FX3DRM.SetTextureOffset(ThisClass[0], ((ThisClass[1] + 9)), ThisID[5].gameObject, 32);
		}
	}
	
	if(!IsNAV){
		FX3DRM.SetTextureOffset(2, 8, IndicatorCorners[0].gameObject, 32);
		FX3DRM.SetTextureOffset(2, 8, IndicatorCorners[1].gameObject, 32);
		FX3DRM.SetTextureOffset(2, 8, IndicatorCorners[2].gameObject, 32);
		FX3DRM.SetTextureOffset(2, 8, IndicatorCorners[3].gameObject, 32);

		if(FX3DRM.SetStatus[1]){
			FX3DRM.SetTextureOffset(2, 8, IndicatorCorners[4].gameObject, 32);
			FX3DRM.SetTextureOffset(2, 8, IndicatorCorners[5].gameObject, 32);
			FX3DRM.SetTextureOffset(2, 8, IndicatorCorners[6].gameObject, 32);
			FX3DRM.SetTextureOffset(2, 8, IndicatorCorners[7].gameObject, 32);		
		}
	}
}

function ResetScale(){
	if(ThisID[1]){
		Destroy(ThisID[1].gameObject);
		Destroy(ThisID[2].gameObject);
		Destroy(ThisID[3].gameObject);
		Destroy(ThisID[4].gameObject);
		if(FX3DRM.SetStatus[1]){
			Destroy(ThisID[5].gameObject);
		}
		CreateRadarID();
	}
}

function SetAsAI(){
IsNAV = false;
IsPlayer = false;
}

function SetAsPlayer(){
ThisFaction[1] = FX3DRM.FXFM.PlayerFaction;
IsPlayer = true;
IsNAV = false;
}

function SetAsNAV(){
IFF = 6;
IsNAV = true;
IsPlayer = false;
BlindRadarOverride = true;
BoundsEnabled = true;
}

function CheckStatus(){
	if(ThisClass[2] != ThisClass[0] || ThisClass[3] != ThisClass[1] || ThisClass[4] != IFF){
		GetNewColor();
	}
	
	if(CurStatus[1] != IsPlayer){
		if(IsPlayer){
			SetAsPlayer();
		}else{
			SetAsAI();
		}
		CurStatus[1] = IsPlayer;
		SetIFFStatus();
		GetNewColor();
		UpdatePlayerContacts();
	}
			
	if(CurStatus[2] != IsPlayerOwned){
		CurStatus[2] = IsPlayerOwned;
		SetIFFStatus();
		GetNewColor();
		UpdatePlayerContacts();
	}

	if(CurStatus[3] != IsAbandoned){
		CurStatus[3] = IsAbandoned;
		ThisFaction[0] = ThisFaction[1];
		ThisFaction[2] = FX3DRM.FXFM.FactionID[ThisFaction[1]];
		SetIFFStatus();
		GetNewColor();
		UpdatePlayerContacts();
	}

	if(CurStatus[11] != IsObjective){
		CurStatus[11] = IsObjective;
		ThisFaction[0] = ThisFaction[1];
		SetIFFStatus();
		GetNewColor();
		UpdatePlayerContacts();
	}

	if(ThisFaction[0] != ThisFaction[1]){
		ThisFaction[0] = ThisFaction[1];
		ThisFaction[2] = FX3DRM.FXFM.FactionID[ThisFaction[1]];
		SetIFFStatus();
		GetNewColor();
		UpdatePlayerContacts();
	}
}

function SetIFFStatus(){
	if(!IsNAV){
		if(!IsPlayerOwned && IsAbandoned && !IsObjective){
			ThisFaction[2] = -1;
			IFF = 0;
		}else if(IsPlayerOwned && !IsObjective){
			IFF = 5;
			ThisFaction[1] = FX3DRM.FXFM.PlayerFaction;
			ThisFaction[2] = FX3DRM.FXFM.PlayerFactionID;
			IsAbandoned = false;
		}else if(ThisFaction[2] == FX3DRM.FXFM.PlayerFactionID && !IsObjective){
			IFF = 2;
		}else if(IsObjective){
			IFF = 7;
		}else{
		
			var ThisRelation : float = FX3DRM.FXFM.FactionRelations[(FX3DRM.FXFM.PlayerFactionID + ThisFaction[2])];
			
			if(ThisRelation <= FX3DRM.FXFM.HFS[0]){
				IFF = 3;
			}else if(ThisRelation > FX3DRM.FXFM.HFS[0] && ThisRelation < FX3DRM.FXFM.HFS[1]){
				IFF = 1;
			}else if(ThisRelation >= FX3DRM.FXFM.HFS[1]){
				IFF = 2;
			}
		}
	}
}

function BuildHostileList() {
var TempTargetList : Collider[] = Physics.OverlapSphere(transform.position, RadarRange / FX3DRM.GameScale, 1 << FX3DRM.Layers[4]);

	for(var i : int = 0; i < TempTargetList.Length; i++){
		var GetFactionID : int = TempTargetList[i].GetComponent(FX_3DRadar_RID).ThisFaction[2];
		if(GetFactionID > 0 && GetFactionID != ThisFaction[2]){
			var ThisRelation : int = FX3DRM.FXFM.FactionRelations[(GetFactionID + ThisFaction[2])];
		
			if(ThisRelation <= FX3DRM.FXFM.HFS[0]){
				HostileList.Add(TempTargetList[i].transform);
			}
		}
	}
Timers[3] += FX3DRM.LocalTime + UpdateHL;
}

function UpdatePlayerContacts(){

	if(!IsNAV && !IsPlayer){
		FX3DRM.RemoveFromList(ThisID[0]);
		if(CurStatus[0]){
			FX3DRM.AddToList(IFF, ThisID[0]);
		}
	}
}

function SetLayer(){

	if(IsDetectable){
		gameObject.layer = FX3DRM.Layers[4]; // Set to Radar Contact layer
	}else{
		gameObject.layer = 2; // Set to Ignore Raycast layer
	}
}

function Set2DMode(){
	if(FX3DRM.SetStatus[7] && !CurStatus[9]){
		ThisID[2].GetComponent.<Renderer>().enabled = false;
		ThisID[3].GetComponent.<Renderer>().enabled = false;
		CurStatus[9] = true;	
	}else if(!FX3DRM.SetStatus[7] && CurStatus[9]){
		ThisID[2].GetComponent.<Renderer>().enabled = true;
		ThisID[3].GetComponent.<Renderer>().enabled = true;
		CurStatus[9] = false;		
	}
}
//End
		//Set this objects RID status.
		//if(!IsDetectable || Distance > FX3DRM.RadarRange[1] || FX3DRM.SetStatus[2] && !BlindRadarOverride && !IsPOI && !CurStatus[5]){
			//if(CurStatus[0]){
				//DisableRID();
			//}
		//}else{
			//RID();
		//}