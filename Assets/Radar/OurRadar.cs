using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// it's 3:30 am and im going to make some radar
// how hard can it be
public class OurRadar : MonoBehaviour {
	
	public float range;
	public int maxBlips;
	public GameObject targetObject;
	public float targetTimer;
	public Transform cameraTransform;
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


	// Use this for initialization
	void Start () {
		hostiles = new PriorityQueue<RadarBlip>(maxBlips);
		contacts = new List<RadarBlip>(maxBlips);
		target = null;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 projectionPosition;
		Vector3 markerPosition;
		RadarBlip toRemove = null;
		foreach(RadarBlip blip in contacts) {
			if (blip.realObject != null) {
				if (blip.realObject.tag != "Player") {
					Vector3 vector = (blip.realObject.transform.position - transform.position) / range * 10f;
					float x = Vector3.Dot (vector, transform.right);
					float y = Vector3.Dot (vector, transform.up);
					float z = Vector3.Dot (vector, transform.forward);
					projectionPosition = new Vector3(x, 0f, z);
					markerPosition = new Vector3(x, y, z);
					projectionPosition = Vector3.ClampMagnitude (projectionPosition, 10f);
					markerPosition  = Vector3.ClampMagnitude (markerPosition, 10f);
					blip.marker.transform.position = Vector3.ProjectOnPlane (projectionPosition, new Vector3(0f, 0.766f, -0.643f));
					markerPosition = new Vector3(blip.marker.transform.position.x, blip.marker.transform.position.y + markerPosition.y, blip.marker.transform.position.z);
					blip.projection.transform.position = markerPosition;
				}
			} else {
				toRemove = blip;
			}
		}
		if (toRemove != null) {
			contacts.Remove (toRemove);
			Destroy (toRemove.marker);
			Destroy (toRemove.projection);
		}
		if (target == null) {
			hostiles.rebuild();
			RadarBlip targetBlip = hostiles.pop();
			if (targetBlip == null) {
				target = null;
				targetObject.renderer.enabled = false;
			} else {
				target = targetBlip.realObject.transform;
			}
		} else {
			targetObject.transform.position = target.position;
			targetObject.transform.LookAt (cameraTransform);
			targetObject.transform.localRotation = transform.rotation;
			targetObject.transform.localScale = Vector3.one * Vector3.Distance (targetObject.transform.position, transform.position) / 10f;
			targetObject.renderer.enabled = true;
		}
	}

	public Transform getTarget() {
		return target;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag != "Radar" && other.tag != "Player") {
			RadarBlip blip;
			switch (other.tag) {
			case "Hostile":
				blip = new RadarBlip(other.gameObject, hostileBlip, hostileBlipDot);
				blip.factionNumber = 3;
				hostiles.add (blip);
				break;
			case "Friendly":
				blip = new RadarBlip(other.gameObject, friendlyBlip, friendlyBlipDot);
				blip.factionNumber = 2;
				break;
			case "Neutral":
				blip = new RadarBlip(other.gameObject, neutralBlip, neutralBlipDot);
				blip.factionNumber = 1;
				break;
			case "Scenery":
				blip = new RadarBlip(other.gameObject, sceneryBlip, sceneryBlipDot);
				blip.factionNumber = 0;
				break;
			default:
				blip = new RadarBlip(other.gameObject, neutralBlip, neutralBlipDot);
				blip.factionNumber = 1;
				break;
			}
			contacts.Add (blip);
		}
	}

	void OnTriggerExit(Collider other) {
		RadarBlip toRemove = null;
		foreach(RadarBlip blip in contacts) {
			if (blip.realObject.Equals(other.gameObject)) {
				toRemove = blip;
				break;
			}
		}
		if (toRemove != null) {
			contacts.Remove (toRemove);
			Destroy (toRemove.marker);
			Destroy (toRemove.projection);
		}
	}
}
