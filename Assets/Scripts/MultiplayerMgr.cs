using UnityEngine;
using System.Collections;

[RequireComponent(typeof (NetworkView))]
public class MultiplayerMgr : MonoBehaviour {

	// The player prefab
	public GameObject playerPrefab;

	// Where the player spawns
	public Transform spawnPoint;

	// Connection button placement and dimensions
	public float buttonX;
	public float buttonY;
	public float buttonWidth;
	public float buttonHeight;
	
	// Use this to refer to the player after it is spawned
	private GameObject go;
	private GameObject player;

	// IP address
	private string ip = "129.22.50.124";

	// This lets you wait for the refresh to finish so you don't get an empty list
	private bool refreshing;

	// The host list
	private HostData[] hostData = null;

	// This is apparently needed
	private string gameName = "ToddFrazierSpace";

	// Don't pay attention to this yet
//	public string[] supportedNetworkLevels = {"testMultiplayer"};
//	public string disconnectedLevel = "loader";
//	private int lastLevelPrefix = 0;

	public void CreatePlayer() {
		// Instantiate the ship, with an offset based on how many are connected.
		go = (GameObject) Network.Instantiate (playerPrefab, spawnPoint.position + new Vector3(Network.connections.Length * 20, 0, 0), Quaternion.identity, 0);
		go.networkView.SetScope (go.networkView.owner, true);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer) {

		} else if (info == NetworkDisconnection.LostConnection) {
			Network.DestroyPlayerObjects (Network.player);
		} else {
			Network.DestroyPlayerObjects (Network.player);
		}
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.DestroyPlayerObjects (player);
	}

	void OnConnectedToServer() {
		CreatePlayer ();
	}

	void OnServerInitialized() {
		Debug.Log ("Server initialized");
		MasterServer.RegisterHost(gameName, "test 1", "this is a test multiplayer game");
		CreatePlayer();
	}

	void OnMasterEvent(MasterServerEvent mse) {
		if (mse == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registered server");
		}
	}

	void Awake () {
		// Set the button positions and dimensions
		buttonX = Screen.width * 0.05f;
		buttonY = Screen.width * 0.05f;
		buttonWidth = Screen.width * 0.1f;
		buttonHeight = Screen.width * 0.1f;
//		DontDestroyOnLoad (this);
//		networkView.group = 1;
//		Application.LoadLevel(disconnectedLevel);
	}

	public void startServer () {
		Network.InitializeServer (32, 25001, !Network.HavePublicAddress());
	}

	public void refreshHostList () {
		MasterServer.RequestHostList (gameName);
		refreshing = true;
	}

	void OnGUI () {
		if (!Network.isClient && !Network.isServer) { // If not already connected
			if(GUI.Button (new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Start Server")) { // Make Server button
				Debug.Log ("Starting Server");
				startServer();
			}
			if(GUI.Button (new Rect(buttonX, buttonY * 1.2f + buttonHeight, buttonWidth, buttonHeight), "Refresh Hosts")) { // Choose host button
				Debug.Log ("Refreshing");
				refreshHostList();
			}
			if (hostData != null) {
				for (int i = 0; i < hostData.Length; i++) { // Host choices
					if (GUI.Button (new Rect(buttonX * 2 + buttonWidth, buttonY * 1.2f + buttonHeight * i, buttonWidth * 3, buttonHeight * 0.5f), hostData[i].gameName)) {
						Network.Connect (hostData[i]);
					}
				}
		}
		}
//		if (Network.peerType != NetworkPeerType.Disconnected) {
//			GUILayout.BeginArea (new Rect(0, Screen.height - 30, Screen.width, 30));
//			GUILayout.BeginHorizontal();
//
//			foreach (string level in supportedNetworkLevels) {
//				if (GUILayout.Button(level)) {
//					Network.RemoveRPCsInGroup(0);
//					Network.RemoveRPCsInGroup(1);
//					networkView.RPC("LoadLevel", RPCMode.AllBuffered, level, lastLevelPrefix + 1);
//				}
//			}
//
//			GUILayout.FlexibleSpace();
//			GUILayout.EndHorizontal();
//			GUILayout.EndArea();
//		}
	}

	[RPC]
	public void LoadLevel (string level, int levelPrefix) {
//		lastLevelPrefix = levelPrefix;
//		Network.SetSendingEnabled (0, false);
//		Network.isMessageQueueRunning = false;
//		Network.SetLevelPrefix (levelPrefix);
//		Application.LoadLevel(level);
//
//		Network.isMessageQueueRunning = true;
//		Network.SetSendingEnabled (0, true);
//		foreach (GameObject go in FindObjectsOfType(typeof (GameObject))) {
//			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
//		}
	}

	// Update is called once per frame
	void Update () {
		// Give the host list a chance to fill up
		if (refreshing) {
			if (MasterServer.PollHostList().Length > 0) {
				refreshing = false;
				Debug.Log (MasterServer.PollHostList ().Length);
				hostData = MasterServer.PollHostList ();
			}
		}
	}
}
