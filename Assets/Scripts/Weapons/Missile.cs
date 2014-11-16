using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

	public float missileThrust;
	public float brakeThrust;
	public float damage;
	private Collider target;
	public float clearTime;
	private float timer;
	Vector3 brakeVector;

	void Start () {
		Destroy (gameObject, 20.0f);
		RaycastHit hit;
		GameObject _GameMgr = GameObject.Find("_GameMgr");
		if (_GameMgr && _GameMgr.GetComponent<FX_3DRadar_Mgr>().SelectedTarget[0] != null) {
			target = GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().SelectedTarget[0].collider;
			Debug.DrawRay (transform.position, rigidbody.velocity, Color.green);
		} else if (Physics.Raycast (transform.position, transform.forward, out hit)) {
			target = hit.collider; 
		} else {
			target = null;
		}
		timer = 0;
		collider.enabled = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.deltaTime;
		if (timer > clearTime) {
			collider.enabled = true;
		}
		if (target != null) {
			transform.LookAt (target.transform);
		} else {
//			transform.LookAt (transform.forward);
		}
		brakeVector = Vector3.Cross (rigidbody.velocity, transform.forward);
		brakeVector = Vector3.Cross (brakeVector, transform.forward);
		rigidbody.AddForce (brakeVector * brakeThrust);
		rigidbody.AddForce (transform.forward * missileThrust);
		Debug.DrawRay(transform.position, brakeVector, Color.red);
		Debug.DrawRay (transform.position, transform.forward);
	}

	void OnCollisionEnter (Collision collision) {
			Quaternion parameters = new Quaternion(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z, damage);
			Debug.Log ("hit you");
			collision.collider.SendMessage ("hurt", parameters, SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);
	}
}
