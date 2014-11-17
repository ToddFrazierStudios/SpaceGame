using UnityEngine;
using System.Collections;

/// <summary>
/// Weapons manager.
/// This class is responsible for actually firing weapons and "reloading them" (enforcing firing rate delays)
/// </summary>
public class WeaponsManager : MonoBehaviour {

	public Transform leftGun, rightGun, missileBay;
	public float muzzleVelocity;
	public GameObject bulletPrefab, missilePrefab;
	public float delay;
	public float range;
	private float timeUntilFire;
	private Transform nextGunToFire;

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
		missileBay.LookAt (Vector3.zero);
		nextGunToFire = leftGun;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeUntilFire > 0) {
			timeUntilFire -= Time.deltaTime;
		} else {
			timeUntilFire = 0;
		}
		if (primaryFire && timeUntilFire == 0) {
			shootMachineGuns();
		}
		if (secondaryFire) {
			shootMissile();
		}
//		if (Input.GetKeyDown (KeyCode.C) || ParsedInput.controller[0].Adown) {
//			RaycastHit hit;
//			if (Physics.Raycast (transform.position, transform.forward, out hit)) {
//				GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().SelectedTarget[0] = hit.transform; 
//			}
//		}
	}
	
//	[RPC]
	public void shootMachineGuns() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, range)) {
			nextGunToFire.LookAt (hit.point); 
		} else {
			nextGunToFire.localRotation = Quaternion.identity;
		}
		GameObject created = Instantiate(bulletPrefab,nextGunToFire.position,nextGunToFire.rotation) as GameObject;
		Bullet b = created.GetComponent<Bullet>();
		b.setVelocity(nextGunToFire.forward*muzzleVelocity);

		if(nextGunToFire == leftGun){
			nextGunToFire = rightGun;
		}else{
			nextGunToFire = leftGun;
		}
		timeUntilFire = delay;
		//		leftGun.Emit(1);
		//		rightGun.Emit(1);
	}

//	[RPC]
	public void shootMissile() {
		GameObject missile = Instantiate(missilePrefab,missileBay.position,missileBay.rotation) as GameObject;
		missile.GetComponent<Missile>().colliderToIgnore = this.GetComponentInChildren<Collider>();
		missile.rigidbody.velocity = rigidbody.velocity;

	}
}
