using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	public float maxShield;
	[System.NonSerialized]
	public float shield;
	public float regenRate;
	public float regenDelay;
	private float regenTime;
	public SpriteRenderer shieldSprite;

	// Use this for initialization
	void Start () {
		shield = maxShield;
	}
	
	// Update is called once per frame
	void Update () {
		DebugHUD.setValue ("shield", shield);
		if (regenTime > 0) {
			regenTime -= Time.deltaTime;
		} else {
			regenTime = 0;
		}

		if (shieldSprite) {
			shieldSprite.color = new Color(1f, shield/maxShield, shield/maxShield, shield > 0 ? 1f : 0f);
		}

		if (shield < maxShield) {
			if (regenTime == 0) {
				shield += regenRate * Time.deltaTime;
			}
			if (shield <= 0) {
				shield = 0;
			}
		} else {
			shield = maxShield;
		}
	}

	public void decrementShield(float damage) {
		Debug.Log (damage);
		shield -= damage;
		regenTime = regenDelay;
	}
}
