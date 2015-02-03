using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class Health : MonoBehaviour {

	public bool mainHealth;
	public float hull;
	public float maxHull;
	public SpriteRenderer hullSprite;
	public float shield;
	public float maxShield;
	public SpriteRenderer shieldSprite;
	public float shieldRegenRate;
	public float rechargeDelay;
	public GameObject explosionPrefab;
	private float timeUntilRecharge;
	private MultiplayerMgr multiplayer;

	// Use this for initialization
	void Start () {
		if (mainHealth) {
			multiplayer = GameObject.Find ("Menu").GetComponent<MultiplayerMgr>();
		}
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
		if (shieldSprite) {
			shieldSprite.color = new Color(1f, shield/maxShield, shield/maxShield);
			DebugHUD.setValue ("shield color", shieldSprite.color);
		}
//		DebugHUD.setValue ("shield health", shield/maxShield * 255f);
		if (hullSprite) hullSprite.color = new Color(1f, hull/maxHull, hull/maxHull);
		string shieldColor = "blue";
		string hullColor = "blue";
		if (shield < maxShield) {
			if (timeUntilRecharge == 0) {
				shield += shieldRegenRate * Time.deltaTime;
			}
			if (shield <= 0) {
				shield = 0;
				shieldColor = "red";
			} else if (shield <= maxShield / 3f) {
				shieldColor = "yellow";
			}
		} else {
			shield = maxShield;
		}
		if (hull <= 0) {
			if (hullSprite)
				hullSprite.color = new Color(0f, 0f, 0f, 0f);
			hullColor = "red";
			networkView.RPC ("Explode", RPCMode.All);
		} else if (hull <= maxHull / 3f) {
			hullColor = "yellow";
		}
		if (tag == "Player") {
			DebugHUD.setValue ("Hull", "<color=" + hullColor + ">" + hull + "</color>");
			DebugHUD.setValue ("Shields", "<color=" + shieldColor + ">" + shield + "</color>");
		}
	}

	// For it to work with send message, the parameters must be stored as a quaternion,
	// with the last value being the damage and the first three being the direction
	public void hurt(Quaternion parameters) {
		float damage = parameters.w;
		Vector3 direction = new Vector3(parameters.x, parameters.y, parameters.z);
		if (shield > 0) {
			shield -= damage;
		} else if (hull > 0) {
			hull -= damage;
		}
		timeUntilRecharge = rechargeDelay;
	}

	[RPC]
	public void Explode() {
		if (tag == "Player" && mainHealth) {
			multiplayer.Respawn();
		}
		Network.RemoveRPCs (networkView.viewID);
		GameObject explosion = Network.Instantiate (explosionPrefab, transform.position, transform.rotation, 0) as GameObject;
		Destroy (explosion, 2.0f);
		if (mainHealth) {
			Destroy (transform.parent.gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}
