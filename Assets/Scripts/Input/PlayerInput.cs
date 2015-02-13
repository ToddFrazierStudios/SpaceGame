using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput {
	private static PlayerPref[] players = new PlayerPref[4];
    private static PlayerPref keyboard;

	static PlayerInput(){
        ControllerProfile.ReloadProfiles();

		//Create Players
		for(int i = 0; i<players.Length; i++){
			players[i] = new PlayerPref(i);
			InputUtils.Implementations defaultImplementation = (InputUtils.Implementations)PlayerPrefs.GetInt("InputBindings.player"+i+".controllerType", 0);
			int defaultControllerNumber = PlayerPrefs.GetInt("InputBindings.player"+i+".controllerNumber",0);
			AssignControllerToPlayer(i,defaultImplementation,defaultControllerNumber);
		}

        keyboard = new PlayerPref(-1);
        keyboard.ChangeController(new KeyboardController());
	}

	private static PlayerPref GetPlayer(int playerNum){
        if (playerNum == -1) return keyboard;
        if (playerNum >= players.Length) return null;
		return players[playerNum];
	}

    public static int GetControllerNumber(int playerNum) {
        return GetPlayer(playerNum).GetControllerNumber();
    }

    public static void ResetBindingsToDefault(int playerNum) {
        GetPlayer(playerNum).ResetBindingsToDefault();
    }

    public static string GetBindingsForControl(int playerNum, Controls c) {
        return GetPlayer(playerNum).GetBindingsForControl(c);
    }

	public static void AssignControllerToPlayer(int playerNum, InputUtils.Implementations type, int controllerNumber){
        PlayerPref player;
        if (playerNum <0 || playerNum >= players.Length)
            return;
        else
            player = players[playerNum];

		switch(type){
		case InputUtils.Implementations.NONE:
			player.ChangeController(null);
			break;
        //case InputUtils.Implementations.KEYBOARD_CONTROLLER:
        //    player.ChangeController(new KeyboardController());
        //    break;
        case InputUtils.Implementations.UNITY_CONTROLLER:
            setupUnityControllerForPlayer(player, controllerNumber);
			break;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        case InputUtils.Implementations.XINPUT_CONTROLLER:
			player.ChangeController(new XInputController(controllerNumber));
			break;
#endif
        default:
            Debug.LogError("Invalid Controller Implementation " + type);
        break;
		}
		PlayerPrefs.SetInt("InputBindings.player"+playerNum+".controllerType",(int)type);
		PlayerPrefs.SetInt("InputBindings.player"+playerNum+".controllerNumber",controllerNumber);
	}

	public static InputUtils.Implementations GetControllerImplementationForPlayer(int playerNum){
		return GetPlayer(playerNum).GetControllerImplementation();
	}

    public static string GetControllerDescription(int playerNum) {
        return GetPlayer(playerNum).GetControllerDescription();
    }

    public static void SetVibration(int playerNum, float left, float right) {
        GetPlayer(playerNum).SetVibration(left, right);
    }

    public static void ResetAllAxes() {
        foreach (PlayerPref pp in players) {
            pp.ResetAllAxes();
        }
        keyboard.ResetAllAxes();
    }

    public static float PollAnalogControl(int playerNumber, Controls c) {
        float value = GetPlayer(playerNumber).GetAnalogControl(c);
        if (playerNumber == 0) {
            value = InputUtils.AbsMax(value, PollAnalogControl(-1, c));
        }
        return value;
    }

    public static bool PollDigitalControl(int playerNumber, Controls c) {
        bool value = GetPlayer(playerNumber).GetDigitalControl(c);
        if (playerNumber == 0 && PollDigitalControl(-1, c)) return true;
        return value;
    }

    public static bool PollDigitalControlPressed(int playerNumber, Controls c) {
        bool value = GetPlayer(playerNumber).GetDigitalControlPressed(c);
        if (playerNumber == 0 && PollDigitalControlPressed(-1, c)) return true;
        return value;
    }

	public static void RemoveControllerFromPlayer(int playerNum){
        if (playerNum == -1) return;//cannot remove keyboard!
		players[playerNum].ChangeController(null);
	}

    public static void RebindControl(int playerNum, Controls c, string newBind) {
        GetPlayer(playerNum).RebindControl(c, newBind);
    }

    private static void setupUnityControllerForPlayer(PlayerPref player, int controllerNumber) {
        string[] controllerNames = Input.GetJoystickNames();
        if (controllerNames==null || controllerNames.Length <= controllerNumber) {
            Debug.LogError("Could not find name for the UnityController.  Please ensure that all controllers are connected and try again.  ");
            return;
        }
        string controllerName = controllerNames[controllerNumber];
        List<ControllerProfile> profiles = ControllerProfile.GetAllProfilesForCurrentPlatformWithName(controllerName);
        if (profiles == null || profiles.Count == 0) {
            Debug.LogWarning("No profiles found! this will be a null-profile UnityController");
            player.ChangeController(new UnityController(controllerNumber, null));
            return;
        }else if (profiles.Count > 1) {
            Debug.LogWarning("Multiple Profiles found!");
        }
        player.ChangeController(new UnityController(controllerNumber, profiles[0]));
    }

    public static bool SupportsProfiles(int playerNum) {
        return GetPlayer(playerNum).ProfilesSupportedByController();
    }

    public static string GetProfileName(int playerNum) {
        return GetPlayer(playerNum).GetProfileName();
    }
}

