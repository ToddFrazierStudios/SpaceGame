using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float hull;
	public float maxHull;
	public float shield;
	public float maxShield;
	public float shieldRegenRate;
	public float rechargeDelay;
	private float timeUntilRecharge;

	// Use this for initialization
	void Start () {
		hull = maxHull;
		shield = maxShield;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeUntilRecharge > 0) {
			timeUntilRecharge -= Time.deltaTime;
		} else {
			timeUntilRecharge = 0;
		}
		string shieldColor = "blue";
		string hullColor = "blue";
		if (shield < maxShield) {
			if (timeUntilRecharge == 0) {
				shield += shieldRegenRate * Time.deltaTime;
			}
			if (shield <= 0) {
				shieldColor = "red";
			} else if (shield <= maxShield / 3f) {
				shieldColor = "yellow";
			}
		} else {
			shield = maxShield;
		}
		if (hull <= 0) {
			hullColor = "red";
		} else if (shield <= maxShield / 3f) {
			hullColor = "yellow";
		}
		DebugHUD.setValue ("Hull", "<color=" + hullColor + ">" + hull + "</color>");
		DebugHUD.setValue ("Shields", "<color=" + shieldColor + ">" + shield + "</color>");
	}

	public void hurt(float damage, Vector3 direction) {
		if (shield > 0) {
			shield -= damage;
		} else if (hull > 0) {
			hull -= damage;
		}
		timeUntilRecharge = rechargeDelay;
	}
}
