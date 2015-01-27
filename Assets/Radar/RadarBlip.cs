using UnityEngine;
using System;
using System.Collections;

public class RadarBlip : IComparable<RadarBlip> {

	public GameObject realObject;
	public int factionNumber;
	public float distance;
	public GameObject marker;
	public Vector3 markerScale = Vector3.one * 0.2f;
	public GameObject projection;

	public RadarBlip (GameObject foundObject, Material blip, Material blipDot) {
//		int layer = LayerMask.NameToLayer("Radar");
		realObject = foundObject;
		marker = GameObject.CreatePrimitive (PrimitiveType.Quad);
		marker.AddComponent<NetworkView>();
		marker.tag = "Radar";
//		marker.layer = layer;
		marker.transform.localScale = new Vector3(.01f, .01f, .01f);
		marker.collider.enabled = false;
		marker.GetComponent<MeshRenderer>().material = blipDot;
		projection = GameObject.CreatePrimitive (PrimitiveType.Quad);
		projection.AddComponent<NetworkView>();
		projection.transform.localScale = new Vector3(0.12f, 0.1f, 0.1f);
		projection.tag = "Radar";
//		projection.layer = layer;
		projection.collider.enabled = false;
		projection.GetComponent<MeshRenderer>().material = blip;
	}

	public int CompareTo(RadarBlip obj) {
		if (obj == null) {
			return 1;
		}
		if (factionNumber == obj.factionNumber) {
			if (distance == obj.distance) {
				return 0;
			} else {
				return (int)(obj.distance - distance);
			}
		} else {
			return factionNumber - obj.factionNumber;
		}
	}
}
