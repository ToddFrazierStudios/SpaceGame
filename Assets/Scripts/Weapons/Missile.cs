using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Missile : MonoBehaviour {
	//controlls the stabilizers
	public float brakeThrust;
	//the maximum amount of damage this missile can do to a target
	public float maxDamage;
	//the homing target, if any
	public Transform target;
	//how long the missile takes to arm
	public float clearTime;

	public OurRadar radar;
	public Targeting targeting;
	private float timer;
	Vector3 brakeVector;

	public ParticleSystem explosion;

	//the maximum number of degrees that the missile can turn every FixedUpdate
	public float maxDeltaAnglePerFrame;
	private float maxDeltaAngleRadians{
		get{
			return maxDeltaAnglePerFrame*Mathf.Rad2Deg;
		}
	}
	public LayerMask layerMask;

	//how big the explosion will be
	public float explosionRadius;
	//this curve determines what percentage of damage a unit will take
	//depending on how close it is to the epicenter of the explosion
	public AnimationCurve damageCurve;

	//how close the missile has to come to a collider before exploding
	public float detonationRadius;

	//how many seconds the missile lasts before destroying itself
	public float lifeTime = 20.0f;

//	[System.NonSerialized]
	public Collider colliderToIgnore;

	public float explosionForce;



	void Start () {
//		Destroy (gameObject, 20.0f);
//		RaycastHit hit;
//		GameObject _GameMgr = GameObject.Find("_GameMgr");
//		if (_GameMgr && _GameMgr.GetComponent<FX_3DRadar_Mgr>().SelectedTarget[0] != null) {
//			target = GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().SelectedTarget[0].transform;
//			Debug.DrawRay (transform.position, rigidbody.velocity, Color.green);
//		} else 
//		if (target == null) {
//			if (radar != null && radar.getTarget () != null) {
//				target = radar.getTarget();
//			} else if (Physics.Raycast (transform.position, transform.forward, out hit, 10f, layerMask)) {
//				Debug.Log (hit.transform);
//				target = hit.transform;
//			} else {
//				target = null;
//			}
//		}
//		rigidbody.AddRelativeForce(Vector3.down * 500000);
		timer = 0;
		collider.enabled = false;
		gameObject.AddComponent<NetworkView>();
		EngineThruster et = GetComponent<EngineThruster>();
		if(et){
			et.throttle = 1f;
			et.useStabilizers = false;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (target != null) {
//			transform.LookAt (target.transform);
			Vector3 targetDirection = target.transform.position - transform.position;
			transform.forward = Vector3.RotateTowards(transform.forward,targetDirection,maxDeltaAngleRadians,0.0f);
			brakeVector = Vector3.Cross (rigidbody.velocity, transform.forward);
			brakeVector = Vector3.Cross (brakeVector, transform.forward);
			rigidbody.AddForce (brakeVector * brakeThrust);
			Debug.DrawRay(transform.position, brakeVector, Color.red);
			Debug.DrawRay (transform.position, transform.forward);
		}

		timer += Time.fixedDeltaTime;
		if (timer > clearTime) {
			collider.enabled = true;
		}else{
			return;
		}


		if(timer>lifeTime){
			networkView.RPC("Explode", RPCMode.All);
		}

		//now we determine if we hit anyone!
		foreach(Collider col in Physics.OverlapSphere(transform.position, detonationRadius)){
			if(col!=collider && col!=colliderToIgnore && !col.isTrigger){
				networkView.RPC("Explode", RPCMode.All);
				return;
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		if(collision.collider!=colliderToIgnore && !collision.collider.isTrigger){
			networkView.RPC("Explode", RPCMode.All);
		}
	}

	//this function may need some work later if it becomes too laggy
	[RPC]
	public void Explode(){
		Destroy(gameObject);
		Network.RemoveRPCs (networkView.viewID);
		Collider[] collidersHit = Physics.OverlapSphere(transform.position,explosionRadius);
		foreach (Collider col in collidersHit){
			if(col!=this.collider && !col.isTrigger){
				//might wanna check if they have a Health component first...
				Vector3 closestPoint = col.ClosestPointOnBounds(transform.position);
				Vector3 impactVector = col.transform.position-closestPoint;
				//may want to redo this to use col.transform.position instead of closestPoint
				float distance = Vector3.Distance(closestPoint,transform.position);
				float damage = damageCurve.Evaluate(distance/explosionRadius)*maxDamage;
				col.SendMessage("hurt", new Quaternion(impactVector.x, impactVector.y, impactVector.z, damage), SendMessageOptions.DontRequireReceiver);
				if(col.rigidbody){
					col.rigidbody.AddExplosionForce(explosionForce,transform.position,explosionRadius);
				}
				if(col.transform.parent){
					if(col.transform.parent.rigidbody){
						col.transform.parent.rigidbody.AddExplosionForce(explosionForce,transform.position,explosionRadius);
					}
				}
			}
		}
		if(explosion){
			explosion.gameObject.transform.parent = null;//detatch the explosion
			explosion.gameObject.AddComponent<NetworkView>();
			Destroy (explosion.gameObject, 1f);//destroy it after some time
			//this is where i'd like to set the system's radius to match the explosionRadius, but oh well
			explosion.Play();
			Network.RemoveRPCs (explosion.networkView.viewID);
		}
	}

	void OnDrawGizmosSelected(){
		Gizmos.DrawWireSphere(transform.position, detonationRadius);
	}

}
