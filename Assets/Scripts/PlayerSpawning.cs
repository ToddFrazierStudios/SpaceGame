using UnityEngine;
using System.Collections;

public class PlayerSpawning : MonoBehaviour {
	
	public GameObject playerCamera;

	void Awake () {
		playerCamera.camera.enabled = false;
	}

	void OnNetworkInstantiate(NetworkMessageInfo info) {
//		networkView.RPC ("spawn", RPCMode.AllBuffered, Network.player);
		if (networkView.owner != Network.player) {
			gameObject.layer = 15;
//			foreach (Camera camera in Camera.allCameras) {
//				if (camera != playerCamera) {
//					camera.enabled = false;
//				}
//			}
//			playerCamera.camera.enabled = false;
//			playerCamera.GetComponent<AudioListener>().enabled = false;
		} else {
			gameObject.layer = 13;
			playerCamera.camera.enabled = true;
		}
	}
	[RPC]
	public void spawn(NetworkPlayer player) {
		Debug.Log ("trying to fix it");
//		if (networkView.isMine) {
			playerCamera.camera.enabled = true;
//		}
		if (player != Network.player) {
			gameObject.layer = 15;
			//			foreach (Camera camera in Camera.allCameras) {
			//				if (camera != playerCamera) {
			//					camera.enabled = false;
			//				}
			//			}
						playerCamera.camera.enabled = false;
						playerCamera.GetComponent<AudioListener>().enabled = false;
		} else {
			playerCamera.camera.enabled = true;
			
			playerCamera.GetComponent<AudioListener>().enabled = true;
			gameObject.layer = 13;
//			playerCamera.camera.enabled = true;
		}
	}
}
