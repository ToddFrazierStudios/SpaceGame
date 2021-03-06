﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Weapons manager.
/// This class is responsible for actually firing weapons and "reloading them" (enforcing firing rate delays)
/// </summary>
public class WeaponsManager : MonoBehaviour {
	
	public Transform leftGun, rightGun, missileBay;
	public float muzzleVelocity;
	public bool recoil;
	public Vector3 recoilForce = new Vector3(0, 0, -1000);
	public GameObject bulletPrefab, missilePrefab;
	public float delay;
	public float missileDelay;
	public float range;
	public float nearRange = 1000;
	public Targeting targeting;
	private OurRadar radar;
	private Transform target;
	private float timeUntilFire;
	private float timeUntilFireMissile;
	private Transform nextGunToFire;
	private int layerMask = 1 << 13;
	private Rigidbody rgb;
	//I plan on using this later
	public float TimeUntilNextPrimaryShot{
		get{
			return timeUntilFire;
		}
	}

	//*** INPUT VARIABLES; see ShipController.cs ***//
	[System.NonSerialized]
	public bool primaryFire = false;//right trigger
	[System.NonSerialized]
	public bool secondaryFire = false;//x button

	//Note for later:
	//maybe it would be better to do it Halo: Reach style, with Y to switch selected weapons and the same fire button for each?
	//for now, I think I'll just use bools for fire primary weapon and one for secondary and let ShipController do the thinking on which the player asked to fire.
	//This class will be responsible for firing weapons (and determining which weapons can be fired right now)

	// Use this for initialization
	void Start () {
		layerMask = ~layerMask;
		radar = GetComponent<OurRadar>();
		targeting = GetComponent<Targeting>();
		missileBay.LookAt (Vector3.zero);
		nextGunToFire = leftGun; // woo for arbitrary decisions
		rgb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (radar) {
			target = radar.getTarget();
		}
		if (timeUntilFireMissile > 0) {
			timeUntilFireMissile -= Time.deltaTime;
		} else {
			timeUntilFireMissile = 0;
		}
		if (timeUntilFire > 0) {
			timeUntilFire -= Time.deltaTime;
		} else {
			timeUntilFire = 0;
		}
		if (primaryFire && timeUntilFire == 0) {
			shootMachineGuns();
		}
		if (secondaryFire && timeUntilFireMissile == 0) {
			shootMissile();
		}
//		if (Input.GetKeyDown (KeyCode.C) || ParsedInput.controller[0].Adown) {
//			RaycastHit hit;
//			if (Physics.Raycast (transform.position, transform.forward, out hit)) {
//				GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().SelectedTarget[0] = hit.transform; 
//			}
//		}
	}
	
	[RPC]
	public void shootMachineGuns() {
//		LineRenderer line = created.GetComponent<LineRenderer>();
		// Dynamically resize based on distance? Maybe later.
		if (targeting && targeting.getTarget () != null && (targeting.getTarget().position - transform.position).sqrMagnitude > nearRange) {
			nextGunToFire.LookAt(targeting.hitPosition);
//			line.SetPosition (0, transform.position);
//			line.SetPosition (1, hit.point);
		} else {
			nextGunToFire.localRotation = Quaternion.identity;
//			line.SetPosition (0, transform.position);
//			line.SetPosition (1, transform.TransformPoint (Vector3.forward * 20f));
		}
		
		GameObject created = Instantiate(bulletPrefab,nextGunToFire.position,nextGunToFire.rotation) as GameObject;
		Bullet b = created.GetComponent<Bullet>();
		b.playerBullet = (gameObject.layer == 13);
		b.gameObject.layer = gameObject.layer;
		b.colliderToIgnore = GetComponent<MeshCollider>();
		b.setVelocity(nextGunToFire.forward*muzzleVelocity);

		if (recoil) rgb.AddForceAtPosition(recoilForce, nextGunToFire.position);

		if(nextGunToFire == leftGun){
			nextGunToFire = rightGun;
		}else{
			nextGunToFire = leftGun;
		}
		timeUntilFire = delay;
		//		leftGun.Emit(1);
		//		rightGun.Emit(1);
	}

	[RPC]
	public void shootMissile() {
		GameObject missile = Instantiate(missilePrefab,missileBay.position,transform.rotation) as GameObject;
		Missile missileComponent = missile.GetComponent<Missile>();
		missileComponent.colliderToIgnore = this.GetComponentInChildren<MeshCollider>();
		if (target != null) {
			missileComponent.target = target;
		} else if (targeting.getTarget () != null) {
			Debug.Log ("set missile target");
			missileComponent.target = targeting.getTarget ();
		} else {
			missileComponent.target = null;
		}
		float forwardVelocity = transform.InverseTransformVector (GetComponent<Rigidbody>().velocity).z;
		missile.GetComponent<Rigidbody>().velocity = missile.transform.forward * forwardVelocity;
        missileComponent.targeting = targeting;
		timeUntilFireMissile = missileDelay;
		if (tag != "Player") missile.tag = tag;
		missile.layer = gameObject.layer;
	}
}
