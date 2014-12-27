using UnityEngine;
using System.Collections;

public class Targeting : MonoBehaviour {

	public Material targetMaterial;
	public GameObject marker;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		marker = GameObject.CreatePrimitive (PrimitiveType.Quad);
		marker.tag = "Radar";
		marker.layer = 16;
		marker.transform.localScale = new Vector3(.2f, .2f, .2f);
		marker.collider.enabled = false;
		marker.GetComponent<MeshRenderer>().material = targetMaterial;
	}
}
