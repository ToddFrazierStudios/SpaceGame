using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour {
	
	public const int HOSTILE=3, FRIENDLY=2, NEUTRAL = 1, SCENERY = 0;
	
	[Header("Targeting:")]
	[Range(0f, 180f)]
	public float targetConeAngle;
	public float range;
	public int maxBlips;
	public Transform cameraTransform;
	[Space(10)]
	[Header("Radar:")]
	public Material playerBlip;
	public Material playerBlipDot;
	public Material hostileBlip;
	public Material hostileBlipDot;
	public Material friendlyBlip;
	public Material friendlyBlipDot;
	public Material neutralBlip;
	public Material neutralBlipDot;
	public Material sceneryBlip;
	public Material sceneryBlipDot;
	
	private List<RadarBlip> contacts;
	
	
	// Use this for initialization
	void Start () {
		contacts = new List<RadarBlip>(maxBlips);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 projectionPosition;
		Vector3 markerPosition;
		RadarBlip toRemove = null;
		foreach(RadarBlip blip in contacts) {
			if (blip.realObject != null && blip.marker != null && blip.projection != null) {
				blip.marker.renderer.enabled = false;
				blip.projection.name = blip.realObject.name;
				blip.marker.name = blip.realObject.name;
				blip.projection.transform.position = blip.realObject.transform.position;
				blip.projection.transform.rotation = cameraTransform.rotation;
//				blip.projection.transform.eulerAngles = transform.eulerAngles;
				blip.projection.transform.localScale = Vector3.one * 200f;
				blip.projection.layer = 18;
			} else {
				toRemove = blip;
			}
		}
		if (toRemove != null) {
			contacts.Remove (toRemove);
			Destroy (toRemove.marker);
			Destroy (toRemove.projection);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag != "Radar" && other.gameObject.layer != 19) {
			RadarBlip blip;
			switch (other.tag) {
			case "Player":
				blip = new RadarBlip(other.gameObject, playerBlip, playerBlipDot);
				blip.factionNumber = FRIENDLY;
				break;
			case "Hostile":
				blip = new RadarBlip(other.gameObject, hostileBlip, hostileBlipDot);
				blip.factionNumber = HOSTILE;
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
