using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// it's 3:30 am and im going to make some radar
// how hard can it be
public class OurRadar : MonoBehaviour {

	public const int HOSTILE=3, FRIENDLY=2, NEUTRAL = 1, SCENERY = 0;

	[Header("Targeting:")]
	[Range(0f, 180f)]
	public float targetConeAngle;
	public float range;
	public int maxBlips;
//	public GameObject targetObject;
	public float targetTimer;
	public Transform cameraTransform;
	public Camera radarCamera;
	public Camera miniMapCamera;
	[Space(10)]
	[Header("Radar:")]
	public Transform radarTransform;
	public Material hostileBlip;
	public Material hostileBlipDot;
	public Material friendlyBlip;
	public Material friendlyBlipDot;
	public Material neutralBlip;
	public Material neutralBlipDot;
	public Material sceneryBlip;
	public Material sceneryBlipDot;

	private List<RadarBlip> contacts;
	private Transform target;
	private PriorityQueue<RadarBlip> hostiles;
	private bool minimap;


	// Use this for initialization
	void Start () {
		hostiles = new PriorityQueue<RadarBlip>(maxBlips);
		contacts = new List<RadarBlip>(maxBlips);
		target = null;
//		if (networkView.isMine) {
//			radarCamera.enabled = true;
//			minimap = miniMapCamera;
//			if (minimap) {
//				miniMapCamera.enabled = false;
//			}
//			minimap = miniMapCamera;
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine) {
			if (minimap && ParsedInput.controller[0].D_UpDown) {
				radarCamera.enabled = !radarCamera.enabled;
				miniMapCamera.enabled = !miniMapCamera.enabled;
			}
			Vector3 projectionPosition;
			Vector3 markerPosition;
			RadarBlip toRemove = null;
			foreach(RadarBlip blip in contacts) {
				if (blip != null && blip.realObject != null && blip.marker != null && blip.projection != null) {
					if (blip.realObject.tag != "Player") {
						Vector3 vector = (blip.realObject.transform.position - transform.position) / range * .5f;
						float x = Vector3.Dot (vector, transform.right);
						float y = Vector3.Dot (vector, transform.up);
						float z = Vector3.Dot (vector, transform.forward);
						projectionPosition = new Vector3(x, 0f, z);
						markerPosition = new Vector3(x, y, z);
						projectionPosition = Vector3.ClampMagnitude (projectionPosition, 10f);
						markerPosition  = Vector3.ClampMagnitude (markerPosition, 10f);
						blip.marker.transform.localPosition = Vector3.ProjectOnPlane (projectionPosition, new Vector3(0f, 0.914f, -0.407f));
						markerPosition = new Vector3(blip.marker.transform.localPosition.x, blip.marker.transform.localPosition.y + markerPosition.y, blip.marker.transform.localPosition.z);
						blip.projection.transform.localPosition = markerPosition;
						blip.distance = Mathf.Abs(Vector3.Distance (transform.position, blip.realObject.transform.position));
//						blip.marker.transform.localRotation = Quaternion.identity;
//						blip.projection.transform.localRotation = Quaternion.identity;
//						blip.marker.transform.LookAt (cameraTransform);
//						blip.projection.transform.LookAt (cameraTransform);
					}
				} else {
					toRemove = blip;
				}
			}
	//		if (target != null && Vector3.Angle (transform.forward, targetObject.transform.position - transform.position) <= targetConeAngle) {
	//			targetObject.transform.position = target.position;
	//			targetObject.transform.LookAt (cameraTransform);
	//			targetObject.transform.localRotation = transform.rotation;
	//			targetObject.transform.localScale = new Vector3(12f, 10f, 10f) * Vector3.Distance (targetObject.transform.position, transform.position) / 100f;
	//		} else {
	//			targetObject.renderer.enabled = false;
	//			newTarget ();
	//		}
			if (toRemove != null) {
	//			hostiles.remove (toRemove);
				contacts.Remove (toRemove);
				Destroy (toRemove.marker);
				Destroy (toRemove.projection);
			}
		}
	}

	public Transform getTarget() {
		return target;
	}

//	private void newTarget() {
//		// resort the hostiles and get the closest one
//		hostiles.rebuild();
//		RadarBlip targetBlip = hostiles.peek();
//		// if there aren't any hostiles left, turn targeting off
//		if (targetBlip != null && targetBlip.realObject != null) {
////			target = null;
////			Debug.Log ("No target found");
//////			if(targetObject)targetObject.renderer.enabled = false;
////		} else {
////			// if we can see it, make it the new target
//			if (Vector3.Angle (transform.forward, targetBlip.realObject.transform.position - transform.position) <= targetConeAngle) {
//				target = targetBlip.realObject.transform;
//				targetObject.renderer.enabled = true;
//			}
//		} else {
//			if(targetObject)targetObject.renderer.enabled = false;
//			target = null;
//		}
//	}

	void OnTriggerEnter(Collider other) {
		if (networkView.isMine) {
			if (other.tag != "Radar" && other.tag != "Player") {
				RadarBlip blip;
				switch (other.tag) {
				case "Hostile":
					blip = new RadarBlip(other.gameObject, hostileBlip, hostileBlipDot);
					blip.factionNumber = HOSTILE;
					Debug.Log ("Found hostile " + blip.realObject.name + " " + Time.frameCount);
	//				hostiles.add (blip);
					break;
				case "Friendly":
					blip = new RadarBlip(other.gameObject, friendlyBlip, friendlyBlipDot);
					blip.factionNumber = FRIENDLY;
					break;
				case "Neutral":
					blip = new RadarBlip(other.gameObject, neutralBlip, neutralBlipDot);
					blip.factionNumber = NEUTRAL;
					break;
				case "Scenery":
					blip = new RadarBlip(other.gameObject, sceneryBlip, sceneryBlipDot);
					blip.factionNumber = SCENERY;
					break;
				default:
					blip = new RadarBlip(other.gameObject, neutralBlip, neutralBlipDot);
					blip.factionNumber = NEUTRAL;
					break;
				}
				blip.projection.transform.parent = radarTransform;
				blip.marker.transform.parent = radarTransform;
				blip.marker.transform.localPosition = Vector3.zero;
				blip.projection.transform.localPosition = Vector3.zero;
				blip.projection.transform.LookAt (cameraTransform);
				blip.marker.transform.LookAt (cameraTransform);
				blip.projection.layer = 16;
				blip.marker.layer = 16;
				blip.projection.name = blip.realObject.name;
				blip.marker.name = blip.realObject.name;
				contacts.Add (blip);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (networkView.isMine) {
			RadarBlip toRemove = null;
			foreach(RadarBlip blip in contacts) {
				if (blip.realObject.Equals(other.gameObject)) {
					toRemove = blip;
					break;
				}
			}
			if (toRemove != null) {
	//			hostiles.remove (toRemove);
				contacts.Remove (toRemove);
				Destroy (toRemove.marker);
				Destroy (toRemove.projection);
			}
		}
	}
}
