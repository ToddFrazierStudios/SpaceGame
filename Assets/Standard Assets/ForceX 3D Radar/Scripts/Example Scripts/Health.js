#pragma strict
private var FXRID : FX_3DRadar_RID;

private var MaxShields : float = 1000;
var CurShields : float = 1000;

private var MaxHull : float = 1000;
var CurHull : float = 1000;

private var MaxStruct : float = 1000;
var CurStruct : float = 1000;

function Start(){
FXRID = gameObject.GetComponent(FX_3DRadar_RID);
}

function Update(){
UpdateStatusBars();
}

function ApplyDamage(damage : float){

	if(CurShields > 0){
		if(CurShields - damage >= 0){
			CurShields -= damage;
		}else{
			CurHull -= (damage - CurShields);
			CurShields = 0;
		}
		
	}else if(CurHull > 0){
		if(CurHull - damage >= 0){
			CurHull -= damage;
		}else{
			CurStruct -= (damage - CurHull);
			CurHull = 0;
		}
		
	}else if(CurStruct > 0){
		CurStruct -= damage;
	}else{
		FXRID.DestroyThis();
	}
	
UpdateStatusBars();
}

function UpdateStatusBars(){
var ShieldsOut : float;
var HullOut : float;
var StructOut : float;
	
	if(FXRID.IsPlayer){
		ShieldsOut = (CurShields / MaxShields);
		HullOut = (CurHull / MaxHull);
		StructOut = (CurStruct / MaxStruct);
		
		FX_3DRadar_Mgr.BarMaterial[0].SetFloat("_ProgressH", StructOut);
		FX_3DRadar_Mgr.BarMaterial[1].SetFloat("_ProgressH", HullOut);
		FX_3DRadar_Mgr.BarMaterial[2].SetFloat("_ProgressH", ShieldsOut);
	
	}else if(FXRID.IsPlayerTarget){
		ShieldsOut = (CurShields / MaxShields);
		HullOut = (CurHull / MaxHull);
		StructOut = (CurStruct / MaxStruct);
		
		FX_3DRadar_Mgr.BarMaterial[3].SetFloat("_ProgressH", StructOut);
		FX_3DRadar_Mgr.BarMaterial[4].SetFloat("_ProgressH", HullOut);
		//FX_3DRadar_Mgr.BarMaterial[5].SetFloat("_ProgressH", ShieldsOut);
	}
}