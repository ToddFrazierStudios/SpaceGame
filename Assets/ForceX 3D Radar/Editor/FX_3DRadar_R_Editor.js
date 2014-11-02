@CustomEditor (FX_3DRadar_RID)

class FX_3DRadar_R_Editor extends Editor {
var setup : boolean = false;
var Factions : String[];
var Help_FS : boolean;
var Help_OC : boolean;
var Help_CND : boolean;
var Help_RS : boolean;
var Help_IBS : boolean;
	function OnInspectorGUI () {
		if(!setup){
			Factions = GameObject.Find("_GameMgr").GetComponent(FX_Faction_Mgr).Factions;
			if(target.ThisFaction[0] > Factions.Length - 1){
				target.ThisFaction[0] = 0;
			}
			setup = true;
		}
		
		EditorGUI.indentLevel = 0;
		EditorGUILayout.LabelField("ForceX 3D Radar EX: Version 1.2.1b", EditorStyles.boldLabel);
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("* = Will only be applied during Start() or Awake()", EditorStyles.boldLabel);

		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Faction Selection", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();

		target.ThisFaction[1] = EditorGUILayout.IntSlider(Factions[target.ThisFaction[1]],target.ThisFaction[1], 0, Factions.Length - 1);

		EditorGUILayout.LabelField("Faction ID :	" + target.ThisFaction[2].ToString());
		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 1;
		Help_FS = EditorGUILayout.Foldout(Help_FS," ?");
		var Help_FS_txt : String = 	"Factions must first be setup in the Faction Mgr for these settings to work. \n\nAdjusting the slider will assign a faction to the object. \n\nFaction ID : This is the currently selected faction's Unique ID, This is used for getting relationship values and is set automatically by the Faction_Mgr script. \n\n";
			if(Help_FS){
				EditorStyles.textField.wordWrap = true;
				Help_FS_txt = EditorGUILayout.TextArea(Help_FS_txt);
			}
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.Space ();
				
		EditorGUILayout.LabelField("Class Selection", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();	
		target.Class = EditorGUILayout.EnumPopup("Object Class:",target.Class);
		target.ThisClass[0] = target.Class;
		ShipSellection();

		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 1;
		Help_OC = EditorGUILayout.Foldout(Help_OC," ?");
		var Help_OC_txt : String = 	"Object Class : Defines the primary classification and will determine what Sub Class options are available. \n\nSub Class : Determines what texture will be assigned as the target Radar ID.\n";
		if(Help_OC){
			EditorStyles.textField.wordWrap = true;
			Help_OC_txt = EditorGUILayout.TextArea(Help_OC_txt);
		}
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();	
		EditorGUILayout.EndVertical ();		
		EditorGUILayout.Space ();	

		EditorGUILayout.LabelField("Condition Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("*Is NAV");
		GUILayout.FlexibleSpace();
		target.IsNAV = EditorGUILayout.Toggle ("", target.IsNAV, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("*Is POI");
		GUILayout.FlexibleSpace();
		target.IsPOI = EditorGUILayout.Toggle ("", target.IsPOI, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("*Is Player");
		GUILayout.FlexibleSpace();
		target.IsPlayer = EditorGUILayout.Toggle ("", target.IsPlayer, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space ();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Is Objective");
		GUILayout.FlexibleSpace();
		target.IsObjective = EditorGUILayout.Toggle ("", target.IsObjective, GUILayout.MaxWidth(110));	
		EditorGUILayout.EndHorizontal();
		

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Is Abandoned");
		GUILayout.FlexibleSpace();
		target.IsAbandoned = EditorGUILayout.Toggle ("", target.IsAbandoned, GUILayout.MaxWidth(110));	
		EditorGUILayout.EndHorizontal();
				
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Is Player Owned");
		GUILayout.FlexibleSpace();
		target.IsPlayerOwned = EditorGUILayout.Toggle ("", target.IsPlayerOwned, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
				
			
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Is Detectable");
		GUILayout.FlexibleSpace();
		target.IsDetectable = EditorGUILayout.Toggle ("", target.IsDetectable, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space ();
	
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Perm Discovery");
		GUILayout.FlexibleSpace();
		target.PermDiscovery = EditorGUILayout.Toggle ("", target.PermDiscovery, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		if(target.PermDiscovery){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("UnDetectable Discovery Reset", GUILayout.MaxWidth(200));
			GUILayout.FlexibleSpace();
			target.DetectionReset = EditorGUILayout.Toggle ("", target.DetectionReset, GUILayout.MaxWidth(110));
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Blind Radar Override");
		GUILayout.FlexibleSpace();
		target.BlindRadarOverride = EditorGUILayout.Toggle ("", target.BlindRadarOverride, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
				

		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 1;
		Help_CND = EditorGUILayout.Foldout(Help_CND," ?");
		var Help_CND_txt : String = 	"Is NAV : Determines if the object will be treated as a NAV waypoint and not a typical radar contact \n\nIs POI : Determines if the object will be treated as a POI. POI's will stick to the edge of the radar when out of radar range. \n\nIs Player : Determines if the current object is controled by the Player.\n\nIs Objective : Determines if the object will be treated as an objective. \n\nIs Abandoned : Sets the objects IFF status to Abandoned. \n\nIs Player Owned : Sets the objects IFF status to Owned & marks the object as beign owned by the player. \n\nIs Detectable : Determines if a object can be detected by the players radar. \n\nPerm Discover : Determines if this object will remain discovered by the player even after the object has left the players radar range. \n\nUnDetectable Discovery Reset : This will reset this object to an un-detected state if the object is outside the players radar range and thie object enters an un-detectable state. The player will need to re-discover the object. \n\nBlind Radar Override : When using Blind Radar this makes the target unable to be obstructed by any objects in the game world. Meaning that it’s will always be known to the player as long as the target is in the radars range. \n\n";
		if(Help_CND){
			EditorStyles.textField.wordWrap = true;
			Help_CND_txt = EditorGUILayout.TextArea(Help_CND_txt);
		}
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();	
		EditorGUILayout.EndVertical ();		
		EditorGUILayout.Space ();	

		EditorGUILayout.LabelField("Radar Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Enable Radar");
		GUILayout.FlexibleSpace();
		target.EnableRadar = EditorGUILayout.Toggle ("", target.EnableRadar, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		
		if(target.EnableRadar){
			EditorGUILayout.Space ();
			target.RadarRange = EditorGUILayout.IntField ("Radar Range", target.RadarRange);
			target.UpdateHL = EditorGUILayout.FloatField ("Refresh Rate (sec)", target.UpdateHL);
			
		}
		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 1;
		Help_RS = EditorGUILayout.Foldout(Help_RS," ?");
		var Help_RS_txt : String = 	"Enable Radar : Determines if this object will build a list of Hostiles based on its own faction relations. \n\nRadar Range : How far this object can detect Hostile contacts. \n\nRefresh Rate : How often the Hostile list is updated. \n\n";
			if(Help_RS){
				EditorStyles.textField.wordWrap = true;
				Help_RS_txt = EditorGUILayout.TextArea(Help_RS_txt);
			}
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();		
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField("Local Indicator & Bounds Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Disable Indicator");
		GUILayout.FlexibleSpace();
		target.IndicatorEnabled = EditorGUILayout.Toggle ("", target.IndicatorEnabled, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Disable Bounds");
		GUILayout.FlexibleSpace();
		target.BoundsEnabled = EditorGUILayout.Toggle ("", target.BoundsEnabled, GUILayout.MaxWidth(110));
		EditorGUILayout.EndHorizontal();		
		
		
		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 1;
		Help_IBS = EditorGUILayout.Foldout(Help_IBS," ?");
		var Help_IBS_txt : String = 	"Disable Indicator : This will prevent the object from displaying it’s screen indicator. \n\nDisable Bounds: This will prevent the object from displaying it’s bounding indicators. \n\n";
			if(Help_IBS){
				EditorStyles.textField.wordWrap = true;
				Help_IBS_txt = EditorGUILayout.TextArea(Help_IBS_txt);
			}
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();		
		EditorGUILayout.Space ();			
		
		EditorGUILayout.LabelField("Current IFF State : Runtime Monitor Only", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();		
		EditorGUILayout.LabelField("IFF Status : ",target.IFF.ToString());
		EditorGUILayout.Space ();
		
		if(target.IsPlayerTarget){
			EditorGUILayout.LabelField("Is Player Target :			True");
		}else{
			EditorGUILayout.LabelField("Is Player Target :			False");
		}
		
		if(target.IsDiscovered){
			EditorGUILayout.LabelField("Is Discovered :				True");
		}else{
			EditorGUILayout.LabelField("Is Discovered :				False");
		}

		EditorGUILayout.Space ();	
		EditorGUILayout.EndVertical ();		
		EditorGUILayout.Space ();						
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
	}
	
	function ShipSellection(){
		switch(target.Class){
		
			case 0:
				target.Misc = EditorGUILayout.EnumPopup("Sub Class:",target.Misc);
				target.ThisClass[1] = target.Misc;
			break;
			
			case 1:
				target.CIVT = EditorGUILayout.EnumPopup("Sub Class:",target.CIVT);
				target.ThisClass[1] = target.CIVT;
			break;

			case 2:
				target.COT = EditorGUILayout.EnumPopup("Sub Class:",target.COT);
				target.ThisClass[1] = target.COT;
			break;
			
			case 3:
				target.Drone = EditorGUILayout.EnumPopup("Sub Class:",target.Drone);
				target.ThisClass[1] = target.Drone;
			break;
			
			case 4:
				target.Fighter = EditorGUILayout.EnumPopup("Sub Class:",target.Fighter);
				target.ThisClass[1] = target.Fighter;
			break;
			
			case 5:
				target.Bomber = EditorGUILayout.EnumPopup("Sub Class:",target.Bomber);
				target.ThisClass[1] = target.Bomber;
			break;
			
			case 6:
				target.Escort = EditorGUILayout.EnumPopup("Sub Class:",target.Escort);
				target.ThisClass[1] = target.Escort;
			break;
			
			case 7:
				target.Frigate = EditorGUILayout.EnumPopup("Sub Class:",target.Frigate);
				target.ThisClass[1] = target.Frigate;
			break;
			
			case 8:
				target.Cruiser = EditorGUILayout.EnumPopup("Sub Class:",target.Cruiser);
				target.ThisClass[1] = target.Cruiser;
			break;
			
			case 9:
				target.BattleShip = EditorGUILayout.EnumPopup("Sub Class:",target.BattleShip);
				target.ThisClass[1] = target.BattleShip;
			break;
			
			case 10:
				target.Dreadnought = EditorGUILayout.EnumPopup("Sub Class:",target.Dreadnought);
				target.ThisClass[1] = target.Dreadnought;
			break;
			
			case 11:
				target.Capital = EditorGUILayout.EnumPopup("Sub Class:",target.Capital);
				target.ThisClass[1] = target.Capital;
			break;
			
			case 12:
				target.SpaceObject = EditorGUILayout.EnumPopup("Sub Class:",target.SpaceObject);
				target.ThisClass[1] = target.SpaceObject;
			break;
			
			case 13:
				target.Celestial = EditorGUILayout.EnumPopup("Sub Class:",target.Celestial);
				target.ThisClass[1] = target.Celestial;
			break;
			
			case 14:
				target.XO = EditorGUILayout.EnumPopup("Sub Class:",target.XO);
				target.ThisClass[1] = target.XO;
			break;
			
			case 15:
				target.ShipType = EditorGUILayout.EnumPopup("Sub Class:",target.ShipType);
				target.ThisClass[1] = target.CIVT;
			break;
		}
	}
}
