using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(GUIText))]
public class DebugHUD : MonoBehaviour {
	private static Dictionary<string,System.Object> list;

	static DebugHUD(){
		list = new Dictionary<string,System.Object>();
	}

	public static void setValue(string key, System.Object value){
		list[key] = value;
	}

	public static void removeKey(string key){
		list.Remove(key);
	}
	
	void Start () {
		transform.position = Vector3.up;
		guiText.alignment = TextAlignment.Left;
		guiText.anchor = TextAnchor.UpperLeft;
		guiText.richText = true;
		guiText.enabled = Application.isEditor||Debug.isDebugBuild;
	}

	void LateUpdate () {
		if(Input.GetKeyDown(KeyCode.F3)){
			guiText.enabled = !guiText.enabled;
		}
		string s = "";
		foreach(string key in list.Keys){
			s+="<color=blue>"+key+"</color>: ";
			s+=list[key]+"\n";
		}
		guiText.text = s;
	}

    public static string FullPathToObject(GameObject obj) {
        if (obj == null) return "";
        if (obj.transform.parent == null) return obj.name;
        return FullPathToObject(obj.transform.parent.gameObject) + "/" + obj.name;
    }
}
