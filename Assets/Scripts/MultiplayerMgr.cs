using UnityEngine;
using System.Collections;

[RequireComponent(typeof (NetworkView))]
public class MultiplayerMgr : MonoBehaviour {

	public GameObject playerPrefab;
	private GameObject go;
	public Transform spawnPoint;
	private string ip = "129.22.50.124";
	private bool connected = false;
	public float buttonX;
	public float buttonY;
	public float buttonWidth;
	public float buttonHeight;
	private bool refreshing;
	private HostData[] hostData = null;
	private string gameName = "ToddFrazierSpace";

	public string[] supportedNetworkLevels = {"testMultiplayer"};
	public string disconnectedLevel = "loader";
	private int lastLevelPrefix = 0;

	public void CreatePlayer() {
		go = (GameObject) Network.Instantiate (playerPrefab, spawnPoint.position, Quaternion.identity, 0);
//		go.GetComponent<RotationManager>().playerSetup (Network.player);
		networkView.RPC ("playerSetup", RPCMode.AllBuffered, Network.player);

	}

	[RPC]
	public void playerSetup(NetworkPlayer player) {
//		go.GetComponent<PlayerSpawning>().spawn(player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer) {

		} else if (info == NetworkDisconnection.LostConnection) {
			Network.DestroyPlayerObjects (Network.player);
		} else {
			Network.DestroyPlayerObjects (Network.player);
		}
//		connected = false;
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
		if (!Network.isClient && !Network.isServer) {
			if(GUI.Button (new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Start Server")) {
				Debug.Log ("Starting Server");
				startServer();
			}
			if(GUI.Button (new Rect(buttonX, buttonY * 1.2f + buttonHeight, buttonWidth, buttonHeight), "Refresh Hosts")) {
				Debug.Log ("Refreshing");
				refreshHostList();
			}
			if (hostData != null) {
				for (int i = 0; i < hostData.Length; i++) {
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
		if (refreshing) {
			if (MasterServer.PollHostList().Length > 0) {
				refreshing = false;
				Debug.Log (MasterServer.PollHostList ().Length);
				hostData = MasterServer.PollHostList ();
			}
		}
	}
}
