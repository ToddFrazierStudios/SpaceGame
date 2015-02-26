using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public bool mainHealth;
	public float hull;
	public float maxHull;
	public SpriteRenderer hullSprite;
	public Shield shield;
	public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
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
			Explode();
		}
	}

	// For it to work with send message, the parameters must be stored as a quaternion,
	// with the last value being the damage and the first three being the direction
	public void hurt(Quaternion parameters) {
		float damage = parameters.w;
//		Vector3 direction = new Vector3(parameters.x, parameters.y, parameters.z);
		if (shield && shield.shield > 0) {
			shield.decrementShield (damage);
		} else if (hull > 0) {
			hull -= damage;
		}
	}

	public void Explode() {
		GameObject explosion = Instantiate (explosionPrefab, transform.position, transform.rotation) as GameObject;
		Destroy (explosion, 2.0f);
		if (mainHealth) {
			Destroy (transform.parent.gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}
