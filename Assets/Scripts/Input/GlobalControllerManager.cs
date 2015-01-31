using UnityEngine;
using System.Collections;
using System.Reflection;

public class GlobalControllerManager {
	private static PlayerPref[] players = new PlayerPref[4];

	static GlobalControllerManager(){
		//Create Players
		for(int i = 0; i<players.Length; i++){
			players[i] = new PlayerPref(i);
		}

		//Create Controllers?
		//nah... I'll just make new ones as needed, let the GC get some exercise...
	}

	public static PlayerPref GetPlayer(int playerNum){
		return players[playerNum];
	}

	public static void AssignControllerToPlayer(int playerNum, Controller.Implementations type, int controllerNumber){
		PlayerPref player = players[playerNum];

		switch(type){
		case Controller.Implementations.KEYBOARD_CONTROLLER:
			player.ChangeController(new KeyboardController());
			break;
		case Controller.Implementations.UNITY_CONTROLLER:
			player.ChangeController (new UnityController(controllerNumber));
			break;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		case Controller.Implementations.XINPUT_CONTROLLER:
			player.ChangeController(new XInputController(controllerNumber));
			break;
#endif
		}
	}

	public static void RemoveControllerFromPlayer(int playerNum){
		players[playerNum].ChangeController(null);
	}
}

