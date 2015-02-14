using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform leftGun, rightGun;
	public float detectionRadius;
	public float muzzleVelocity;
	private float delay;
	public Transform targetShip;
	private Transform target;
	private float timeUntilFire;
	private Transform nextGunToFire;

	// Use this for initialization
	void Start () {
		delay = Random.Range (0.1f, 0.3f);
		nextGunToFire = rightGun;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeUntilFire > 0) {
			timeUntilFire -= Time.deltaTime;
		} else {
			timeUntilFire = 0;
		}
		if (target == null) {
			getTarget();
		}
		if (timeUntilFire == 0) {
			shootMachineGuns();
		}
	}

	private void getTarget() {
		foreach(Collider col in Physics.OverlapSphere(transform.position, detectionRadius)){
			if(col!=collider && col.tag != tag && !col.isTrigger){
				target = col.transform;
				if (col.tag == "Player") {
					return;
				}
			}
		}
	}

	public void shootMachineGuns() {
		// Dynamically resize based on distance? Maybe later.
		if (target != null) {
			nextGunToFire.LookAt (target); 
		} else {
			nextGunToFire.LookAt(targetShip);
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
	}
}
