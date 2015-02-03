using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class Health : MonoBehaviour {

	public bool mainHealth;
	public float hull;
	public float maxHull;
	public SpriteRenderer hullSprite;
	public Shield shield;
	public GameObject explosionPrefab;
	private MultiplayerMgr multiplayer;

	// Use this for initialization
	void Start () {
		if (mainHealth) {
			multiplayer = GameObject.Find ("Menu").GetComponent<MultiplayerMgr>();
		}
		hull = maxHull;
	}
	
	// Update is called once per frame
	void Update () {
//		DebugHUD.setValue ("shield health", shield/maxShield * 255f);
		if (hullSprite) {
			hullSprite.color = new Color(1f, hull/maxHull, hull/maxHull);
		}
		if (hull <= 0) {
			if (hullSprite) {
				hullSprite.color = new Color(0f, 0f, 0f, 0f);
			}
			networkView.RPC ("Explode", RPCMode.All);
		}
	}

	// For it to work with send message, the parameters must be stored as a quaternion,
	// with the last value being the damage and the first three being the direction
	public void hurt(Quaternion parameters) {
		float damage = parameters.w;
		Vector3 direction = new Vector3(parameters.x, parameters.y, parameters.z);
		if (shield && shield.shield > 0) {
			shield.decrementShield (damage);
		} else if (hull > 0) {
			hull -= damage;
		}
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
