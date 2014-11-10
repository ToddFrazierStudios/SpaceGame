using UnityEngine;
using System.Collections;

public class PlayerSpawning : MonoBehaviour {
	
	public GameObject playerCamera;
	public GameObject radarCamera;

	void Awake () {
		playerCamera.camera.enabled = false; // Have the camera disabled by default.
	}

	void OnNetworkInstantiate(NetworkMessageInfo info) {
		if (networkView.owner != Network.player) {
			gameObject.layer = 15; // If the ship spawned is not the player, put it on the enemy layer.
		} else {
			gameObject.layer = 13; // If the ship is the player, put it on the player level.

			// Assigning the player transform, player camera, radar camera, and the player camera transform in the radar script.
			GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().Transforms[0] = gameObject.transform;
			GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().Cameras[4] = playerCamera.camera;
			GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().Cameras[0] = radarCamera.camera;
			GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().Transforms[2] = playerCamera.transform;

			playerCamera.camera.enabled = true; // Turn on the player camera. This fixes a lot of problems.
		}
	}
}
