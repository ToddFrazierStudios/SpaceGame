using UnityEngine;
using System;
using System.Collections;

public class RadarBlip : IComparable<RadarBlip> {

	public GameObject realObject;
	public int factionNumber;
	public GameObject marker;
	public GameObject projection;

	public RadarBlip (GameObject foundObject, Material blip, Material blipDot) {
		realObject = foundObject;
		marker = GameObject.CreatePrimitive (PrimitiveType.Quad);
		marker.tag = "Radar";
		marker.layer = 16;
		marker.transform.localScale = new Vector3(.2f, .2f, .2f);
		marker.collider.enabled = false;
		marker.GetComponent<MeshRenderer>().material = blipDot;
		projection = GameObject.CreatePrimitive (PrimitiveType.Quad);
		projection.transform.localScale = new Vector3(1f, 1f, 1f);
		projection.tag = "Radar";
		projection.layer = 16;
		projection.collider.enabled = false;
		projection.GetComponent<MeshRenderer>().material = blip;
	}

	public float getDistance() {
		return Vector3.Magnitude(marker.transform.position);
	}

	public int CompareTo(RadarBlip obj) {
		if (obj == null) {
			return 1;
		}
//		if (factionNumber == obj.factionNumber) {
			if (getDistance() == obj.getDistance()) {
				return 0;
			} else {
				return (int)(obj.getDistance() - getDistance());
			}
//		} else {
//			return factionNumber - obj.factionNumber;
//		}
	}
}
