using UnityEngine;
using UnityEditor;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public void OnNewGame() {
		Application.LoadLevel ("TestMultiplayer 1");
	}

	public void OnLoadGame() {
		// go to load game screen eventually
		Application.LoadLevel ("TestMultiplayer 1");
	}

	public void OnQuit() {
		//save game
		// probably some other stuff
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

}
