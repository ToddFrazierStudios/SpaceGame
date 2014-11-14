using UnityEngine;
using System.Collections;

public class WeaponsManager : MonoBehaviour {

	public Transform leftGun, rightGun, missileBay;
	public float muzzleVelocity;
	public GameObject bulletPrefab, missilePrefab;
	public float delay;
	public float range;
	private float timeUntilFire;
	private bool alternate = true;

	// Use this for initialization
	void Start () {
		missileBay.LookAt (Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		if (timeUntilFire > 0) {
			timeUntilFire -= Time.deltaTime;
		} else {
			timeUntilFire = 0;
		}
		if ((Input.GetKey (KeyCode.Space) || ParsedInput.controller[0].RightTrigger > 0) && timeUntilFire == 0) {
			shootMachineGuns();
			timeUntilFire = delay;
		}
		if (Input.GetKeyDown (KeyCode.X) || ParsedInput.controller[0].Xdown) {
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
		if (alternate) {
			if (Physics.Raycast (transform.position, transform.forward, out hit, range)) {
				leftGun.LookAt (hit.point); 
			} else {
				leftGun.localRotation = Quaternion.identity;
			}
			GameObject created = Instantiate(bulletPrefab,leftGun.position,leftGun.rotation) as GameObject;
			Bullet b = created.GetComponent<Bullet>();
			b.setVelocity(leftGun.forward*muzzleVelocity);
		} else {
			if (Physics.Raycast (transform.position, transform.forward, out hit, range)) {
				rightGun.LookAt (hit.point); 
			} else {
				rightGun.localRotation = Quaternion.identity;
			}
			GameObject created = Instantiate(bulletPrefab,rightGun.position,rightGun.rotation) as GameObject;
			Bullet b = created.GetComponent<Bullet>();
			b.setVelocity(rightGun.forward*muzzleVelocity);
		}
		alternate = !alternate;
		//		leftGun.Emit(1);
		//		rightGun.Emit(1);
	}

	[RPC]
	public void shootMissile() {
		GameObject missile = Instantiate(missilePrefab,missileBay.position,missileBay.rotation) as GameObject;
		missile.rigidbody.velocity = rigidbody.velocity;

	}
}
