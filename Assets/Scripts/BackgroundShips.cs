using UnityEngine;
using System.Collections;

public class BackgroundShips : MonoBehaviour {
	
	public Transform targetShip;
	public Transform[] missileBay;
	public GameObject missilePrefab;
	public float timeBetweenMissiles = 1f;
	private float timer;

	void Start () {
		timer = 0f;
	}

	// Update is called once per frame
	void Update () {
		timer = timer + Time.deltaTime;
		if (timer > timeBetweenMissiles) {
			timer = 0f;
			int bay = Random.Range (0, missileBay.Length);
			
			GameObject missile = Instantiate(missilePrefab,missileBay[bay].position,missileBay[bay].rotation) as GameObject;
			missile.tag = tag;
			missile.GetComponent<Missile>().target = targetShip;
			missile.GetComponent<Missile>().colliderToIgnore = this.GetComponentInChildren<MeshCollider>();

		}
	}
}
