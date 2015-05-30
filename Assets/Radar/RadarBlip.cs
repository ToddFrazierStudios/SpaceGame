using UnityEngine;
using System;
using System.Collections;

public class RadarBlip : IComparable<RadarBlip> {

	public GameObject realObject;
	public int factionNumber;
	public float distance;
	public GameObject marker;
//	public Vector3 markerScale = Vector3.one * 0.2f;
    public GameObject projection;
    private Vector3 projectionSize = 0.15f * Vector3.one;
    private Vector3 markerSize = 0.6f * Vector3.one;

	public RadarBlip (GameObject foundObject, Material blipDot, Material blip) {
//		int layer = LayerMask.NameToLayer("Radar");
		realObject = foundObject;
		marker = GameObject.CreatePrimitive (PrimitiveType.Quad);
		marker.tag = "Radar";
//		marker.layer = layer;
//		marker.transform.localScale = new Vector3(.015f, .015f, .015f);
		marker.GetComponent<Collider>().enabled = false;
		marker.GetComponent<MeshRenderer>().material = blipDot;
		projection = GameObject.CreatePrimitive (PrimitiveType.Quad);
//		projection.transform.localScale = new Vector3(0.1f, 0.8f, 0.1f);
		projection.tag = "Radar";
//		projection.layer = layer;
		projection.GetComponent<Collider>().enabled = false;
        projection.GetComponent<MeshRenderer>().material = blip;
        marker.transform.localScale = markerSize;
        projection.transform.localScale = projectionSize;
        marker.transform.localPosition = Vector3.zero;
        projection.transform.localPosition = Vector3.zero;
        projection.transform.localRotation = Quaternion.identity;
        marker.transform.localRotation = Quaternion.identity;
        projection.layer = 16;
        marker.layer = 16;
        projection.name = realObject.name;
        marker.name = realObject.name;
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
