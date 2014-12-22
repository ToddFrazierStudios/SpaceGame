using UnityEngine;
using System.Collections;

public class UnityConsoleTester : MonoBehaviour {

	private float time = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		if(time>=2f){
			Debug.Log ("Log Message!");
			Debug.Log("Log Message with context", this);
			Debug.LogError("Log error message!");
			Debug.LogError("Log error message with context!", this);
			Debug.LogWarning("Log Warning!");
			Debug.LogWarning("Log Warning with context!", this);
			Debug.LogException(new UnityException("Log Exception!"));
			Debug.LogException(new UnityException("Log Exception with context!"), this);
			this.enabled = false;
		}
	}
}
