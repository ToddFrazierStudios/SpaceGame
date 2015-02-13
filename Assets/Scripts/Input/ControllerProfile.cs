using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System;

public class ControllerProfile {
    #region nonStatic

    private Dictionary<string, string> table;
    private string[] names;
    private bool osxTriggerCompNeeded;
    private bool analogDPad;
    private bool invertedRightStick;
    private Platform platform;
    private bool isExternal;
    private string displayName;

    private ControllerProfile(Dictionary<string, string> tab, string[] nams, bool osx, bool dpad, bool invRS, Platform plat, bool external, string dispname) {
        table = tab;
        names = nams;
        osxTriggerCompNeeded = osx;
        analogDPad = dpad;
        invertedRightStick = invRS;
        platform = plat;
        isExternal = external;
        displayName = dispname;
    }

    public string this[string s] {
        get {
            return table[s];
        }
    }

    public string DisplayName {
        get {
            return displayName;
        }
    }

    public bool IsExternalProfile {
        get { return isExternal; }
    }

    public bool hasName(string other) {
        foreach (string name in names) {
            if (name == other) return true;
        }
        return false;
    }

    public bool OSXTriggerCompNeeded {
        get { return osxTriggerCompNeeded; }
    }
    public bool AnalogDPad {
        get { return analogDPad; }
    }
    public bool InvertedRightStick {
        get { return invertedRightStick; }
    }

    public Platform PlatformRequired {
        get { return platform; }
    }

    public enum Platform {
        ANY,WINDOWS,OSX,OTHER
    }
    #endregion
    #region static
    ////STATIC////
    private static List<ControllerProfile> profiles;
    private static void loadBundledProfiles(){
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>("ControllerProfiles");
        Debug.Log(textAssets.Length + " TextAsset(s) found!");
        foreach (TextAsset ta in textAssets) {
            Debug.Log("Parsing TextAsset " + ta.name);
            parseJSON(ta.text, false);
        }
    }

    private static void loadNonBundledProfiles() {
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(),"*.json");
        Debug.Log("Searching for .json files in " + Directory.GetCurrentDirectory());
        Debug.Log(files.Length + " external json file(s) found: " + string.Join(",", files));
        foreach (string file in files) {
            parseJSON(File.ReadAllText(file), true);
        }
    }

    public static void ReloadProfiles() {
        if (profiles == null) 
            profiles = new List<ControllerProfile>();
        else
          profiles.Clear();

        loadBundledProfiles();
        loadNonBundledProfiles();
    }

    public static List<ControllerProfile> GetAllProfilesForName(string name) {
        List<ControllerProfile> list = new List<ControllerProfile>();
        foreach (ControllerProfile p in profiles) {
            if (p.hasName(name)) list.Add(p);
        }
        return list;
    }

    public static ControllerProfile[] GetAllProfiles() {
        return profiles.ToArray();
    }

    public static List<ControllerProfile> GetAllProfilesForPlatform(Platform p) {
        if (p == Platform.ANY) {
            return new List<ControllerProfile>(GetAllProfiles());
        }
        List<ControllerProfile> list = new List<ControllerProfile>();
        foreach (ControllerProfile cp in profiles) {
            if (cp.platform == p || cp.platform == Platform.ANY) list.Add(cp);
        }
        return list;
    }

    public static List<ControllerProfile> GetAllProfilesForPlatformWithName(Platform p, String name) {
        List<ControllerProfile> list = new List<ControllerProfile>();
        foreach (ControllerProfile cp in profiles) {
            if ((cp.platform == p || cp.platform == Platform.ANY) && cp.hasName(name)) list.Add(cp);
        }
        return list;
    }
    public static List<ControllerProfile> GetAllProfilesForCurrentPlatformWithName(string name) {
        Platform p = Platform.OTHER;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        p = Platform.WINDOWS;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        p = Platform.OSX;
#endif
        return GetAllProfilesForPlatformWithName(p, name);
    }

    public static List<ControllerProfile> GetAllProfilesForCurrentPlatform() {
        Platform p = Platform.OTHER;
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        p = Platform.WINDOWS;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        p = Platform.OSX;
#endif
        return GetAllProfilesForPlatform(p);
    }

    private static void parseJSON(string json, bool isEx) {
        JSONNode root = JSON.Parse(json);
        int nameCount = root["names"].AsArray.Count;
        string[] names = new string[nameCount];
        for (int i = 0; i<nameCount; i++) {
            names[i] = root["names"].AsArray[i];
            Debug.Log("Name found: " + names[i]);
        }

        string displayName = root["displayname"].Value;

        bool osxTriggerCompNeeded = root["OSXTriggerCompensationRequired"].AsBool;
        //Debug.Log("OSX trigger comp is " + (osxTriggerCompNeeded ? "" : "not") + " needed");

        bool analogDPad = root["analogDpad"].AsBool;
        //Debug.Log("DPad is " + (analogDPad ? "" : "not") + " analog");

        bool invertedRightStick = root["invertedRightStick"].AsBool;
       // Debug.Log("Right stick is " + (invertedRightStick ? "" : "not") + " inverted");

        Platform platform;
        switch (root["platform"]) {
            case "WIN":
                platform = Platform.WINDOWS;
                break;
            case "OSX":
                platform = Platform.OSX;
                break;
            case "OTHER":
                platform = Platform.OTHER;
                break;
            default:
                platform = Platform.ANY;
                break;
        }
        //Debug.Log("Set Platform to " + Enum.GetName(typeof(Platform), platform));

        //time to fill out the table...
        Dictionary<string, string> table = new Dictionary<string, string>();
        //Debug.Log(root["table"].GetType().Name);
        foreach (KeyValuePair<string,JSONNode> pair in root["table"] as JSONClass) {
            //Debug.Log(string.Format("Adding pair ({0},{1}) to table", pair.Key, pair.Value));
            table.Add(pair.Key, pair.Value.Value);
        }

        profiles.Add(new ControllerProfile(table, names, osxTriggerCompNeeded, analogDPad, invertedRightStick, platform, isEx, displayName));
    }
    #endregion static
}