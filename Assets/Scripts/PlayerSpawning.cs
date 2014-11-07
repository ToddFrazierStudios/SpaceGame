using UnityEngine;
using System.Collections;

public class PlayerSpawning : MonoBehaviour {
	
	public GameObject playerCamera;

	void OnNetworkInstantiate(NetworkMessageInfo info) {
		if (networkView.owner != Network.player) {
			gameObject.layer = 13;
			playerCamera.camera.enabled = false;
			playerCamera.GetComponent<AudioListener>().enabled = false;
		} else {
			gameObject.layer = 15;
		}
	}
}
