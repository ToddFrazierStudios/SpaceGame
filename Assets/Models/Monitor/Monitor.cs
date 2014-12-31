using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class Monitor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.O)){
			animation.Play ("Open",PlayMode.StopAll);
		}
		if(Input.GetKeyDown(KeyCode.P)){
			animation.Play("Close",PlayMode.StopAll);
		}
	}
}
