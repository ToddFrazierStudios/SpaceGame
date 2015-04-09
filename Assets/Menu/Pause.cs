using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	public int playerNumber = 0;
	private ShipController player;
	private Canvas canvas;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("PlayerNoNetwork").GetComponent<ShipController>();
		canvas = gameObject.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerInput.PollDigitalControl(playerNumber, Controls.PAUSE_BUTTON)) {
			loadPauseMenu();
		}
	}

	public void loadPauseMenu() {
		Cursor.visible = true;
		player.isAi = true;
		canvas.enabled = true;
	}

	public void OnResume() {
		Cursor.visible = false;
		player.isAi = false;
		canvas.enabled = false;
	}

	public void OnLeave() {
		Cursor.visible = false;
		Application.LoadLevel ("testMenu");
	}
}
