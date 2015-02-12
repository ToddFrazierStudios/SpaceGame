using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ControllerProfile {
    public Dictionary<string, string> table;
    public string[] names;
    public bool osxTriggerCompNeeded;
    public bool analogDPad;
    public bool invertedRightStick;

    private ControllerProfile() {
        table = new Dictionary<string, string>();
    }




    ////STATIC////
    private static List<ControllerProfile> profiles;
    private static void loadBundledProfiles(){
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>("ControllerProfiles");
        Debug.Log(textAssets.Length + " TextAsset(s) found!");
        foreach (TextAsset ta in textAssets) {
            parseJSON(ta.text);
        }
    }
    private static void parseJSON(string json) {
        JSONNode root = JSON.Parse(json);
        ControllerProfile profile = new ControllerProfile();
        int nameCount = root["names"].AsArray.Count;
        profile.names = new string[nameCount];
        for (int i = 0; i<nameCount; i++) {
            profile.names[i] = root["names"].AsArray[i];
        }
        profile.osxTriggerCompNeeded = root["OSXTriggerCompensationRequired"].AsBool;
        profile.analogDPad = root["analogDpad"].AsBool;
        profile.invertedRightStick = root["invertedRightStick"].AsBool;
        //time to fill out the table...
        
        foreach (JSONClass node in root["table"].AsObject) {
            
        }

        profiles.Remove(profile);
        profiles.Add(profile);
    }
}