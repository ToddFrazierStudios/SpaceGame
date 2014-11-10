#pragma strict

private var FX3DRM : FX_3DRadar_Mgr;

function Start(){
FX3DRM = GameObject.Find("_GameMgr").GetComponent(FX_3DRadar_Mgr);
}

function OnGUI () {
	if(FX3DRM.SelectedTarget[0]){
		var DisplayDistance : float = (FX3DRM.ThisDistance * FX3DRM.GameScale);
		if(DisplayDistance < 1000){
			GUI.Label (Rect(Screen.width * .5 - 100,Screen.height - 20,200,20),FX3DRM.SelectedTarget[0].name.ToString() + " Distance: " + DisplayDistance.ToString("0. :m"));
		}else{
			GUI.Label (Rect(Screen.width * .5 - 100,Screen.height - 20,200,20),FX3DRM.SelectedTarget[0].name.ToString() + " Distance: " + (DisplayDistance *.001).ToString("#.0 :km"));
		}
	}
}