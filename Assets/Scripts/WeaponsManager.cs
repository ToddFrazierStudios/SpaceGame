using UnityEngine;
using System.Collections;

public class WeaponsManager : MonoBehaviour {

	public ParticleSystem leftGun, rightGun;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space) || ParsedInput.controller[0].RightTrigger > 0) {
			leftGun.Emit(1);
			rightGun.Emit(1);
		}
	}
}
