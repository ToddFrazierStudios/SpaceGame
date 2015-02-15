using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private Vector3 oldPos;
	private float lifeTime = 0.0f;
	public float maxLife = 1.5f;
	public float damage;
	public Collider colliderToIgnore;
	public int layerMask;
	private Vector3 velocity;//251 m/s
	public bool playerBullet;
	
//	public AudioClip[] hitEnemySounds,hitWallSounds;
//	
//	private AudioSource source;
	
	public void setVelocity(Vector3 newVel){
		velocity = newVel;
	}
	// Use this for initialization
	void Start () {
//		source = GetComponent<AudioSource>();
		oldPos = transform.position;
		Destroy (gameObject,maxLife);
	}
	
//	private void playOneSound(AudioClip[] list){
//		int chosen = Random.Range(0,list.Length-1);
////		source.PlayOneShot(list[chosen]);
//	}
	
	// Update is called once per frame
	void Update () {
		lifeTime+=Time.deltaTime;
		//raycast; did we hit anything?
		RaycastHit hit;
		transform.position+=velocity*Time.deltaTime;
		if(Physics.Linecast(oldPos,transform.position,out hit)){
			if (!(hit.collider.gameObject.layer == 13 && playerBullet)) {
				Vector3 direction = transform.position - oldPos;
				Quaternion parameters = new Quaternion(direction.x, direction.y, direction.z, damage);
				hit.collider.gameObject.SendMessage("hurt", parameters, SendMessageOptions.DontRequireReceiver);
				if(hit.collider.gameObject.tag == "Enemy"){
	//				playOneSound(hitEnemySounds);
				}else{
	//				playOneSound(hitWallSounds);
				}
				Network.Destroy(gameObject);
			}
		}
		//next...
		//		//adjust the trail
		//		for(int i = 0; i<segment;i++){
		//			Color start = new Color(1,1,1,(1.0 - lifeTime * 5.0)*0.05);
		//			Color end = new Color(1,1,1,5.0*0.05);
		//		}
		
	}
}
