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
	private OurRadar radar;
	private Transform target;
	private float timeUntilFire;
	private Transform nextGunToFire;
	private int layerMask = 1 << 13;
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
		missileBay.LookAt (Vector3.zero);
		nextGunToFire = leftGun;
	}
	
	// Update is called once per frame
	void Update () {
		if (radar) {
			target = radar.getTarget();
		}
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
	
	[RPC]
	public void shootMachineGuns() {
		RaycastHit hit;
		GameObject created = Network.Instantiate(bulletPrefab,nextGunToFire.position,nextGunToFire.rotation, 0) as GameObject;
//		LineRenderer line = created.GetComponent<LineRenderer>();
		// Dynamically resize based on distance? Maybe later.
		if (Physics.SphereCast (transform.position, 7f, transform.forward, out hit, range, layerMask)) {
//			nextGunToFire.LookAt (hit.point); 
//			line.SetPosition (0, transform.position);
//			line.SetPosition (1, hit.point);
		} else {
//			nextGunToFire.localRotation = Quaternion.identity;
//			line.SetPosition (0, transform.position);
//			line.SetPosition (1, transform.TransformPoint (Vector3.forward * 20f));
		}

		Bullet b = created.GetComponent<Bullet>();
		if (gameObject.layer == 13) {
//			b.layerMask = layerMask;
		} else {
//			b.layerMask = 
		}
		b.colliderToIgnore = GetComponent<MeshCollider>();
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

	[RPC]
	public void shootMissile() {
		GameObject missile = Network.Instantiate(missilePrefab,missileBay.position,transform.rotation, 0) as GameObject;
		Missile missileComponent = missile.GetComponent<Missile>();
		missileComponent.colliderToIgnore = this.GetComponentInChildren<MeshCollider>();
		RaycastHit hit;
		if (target != null) {
			missileComponent.target = target;
		} else if (Physics.Raycast(transform.position, transform.forward, out hit, range, layerMask)) {
			missileComponent.target = hit.transform;
		} else {
			missileComponent.target = null;
		}
//		missile.rigidbody.velocity = rigidbody.velocity;
		if (tag != "Player") missile.tag = tag;
	}
}
