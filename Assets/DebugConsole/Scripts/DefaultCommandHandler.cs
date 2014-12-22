using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

/// <summary>
/// <para>This class contains some basic commands for use with the DebugConsole.  To get these commands into the DebugConsole,
/// simpily add it to any GameObject (I recommend the DebugConsole itself), and  drag the DebugConsole component into
/// the corresponding field on this script.  I seperated them into a seperate class in order to
/// give an example of how to add your own commands and make them work, and because DebugConsole.cs was getting kinda full.</para>
/// <para>Notice that the delegate from DebugConsole does not provide an instance of the console itself.  I figured that since
/// you would need an instance of the console already to call addCommand, passing it to the delegate was redundant.  </para>
/// </summary>
/// Author: Steven Miller
/// You are free to use and modify this code, but please give credit!
public class DefaultCommandHandler : MonoBehaviour {
	public DebugConsole console;

	// Use this for initialization
	void Start () {
		console.printLine("Adding default commands...");
		console.addCommand(LIST_ALL_TAG,LIST_ALL_USAGE,LIST_ALL_HELP, listAllDelegate);
		console.addCommand(LIST_COMP_TAG,LIST_COMP_USAGE,LIST_COMP_HELP, listCompDelegate);
		console.addCommand(DESTROY_TAG,DESTROY_USAGE,DESTROY_HELP,destroyDelegate);
		console.addCommand(INFO_TAG,INFO_USAGE,INFO_HELP,infoDelegate);
		console.printLine("Default commands added!");
	}

	//*** COMMAND: listall ***//
	private const string LIST_ALL_TAG = "listall";
	private const string LIST_ALL_USAGE = "listall [maxDepth]";
	private const string LIST_ALL_HELP = "Lists all GameObjects in the hierarchy.  Optionally, you can provide the maximum depth to search.";
	public void listAllDelegate(string[] split, out bool showUsage){
		if(split.Length>2){
			showUsage = true;
			return;
		}else{
			showUsage = false;
		}
		int maxDepth = split.Length==2?int.Parse(split[1]):-1;
		if(maxDepth<=0)maxDepth = int.MaxValue;
		foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
			if(g.transform.parent == null)printAllGameObjectsRecur(g,"",maxDepth,1);
		}
	}
	private void printAllGameObjectsRecur(GameObject g, string surname, int maxDepth, int curDepth){
		string name = g.transform.parent ? surname+"/"+g.name : g.name;
		string nameInColor = "<b>"+g.name+"</b>";
		string coloredName = g.transform.parent ? surname+"/"+nameInColor : nameInColor;
		if(curDepth==maxDepth && g.transform.childCount>0){
			console.printLine(coloredName+"/...");
		}else{
			console.printLine(coloredName);
		}
		if(g.transform.childCount>0 && curDepth<maxDepth){
			for(int i = 0; i<g.transform.childCount;i++){
				printAllGameObjectsRecur(g.transform.GetChild(i).gameObject,name, maxDepth, curDepth+1);
			}
		}
	}

	//*** COMMAND: listcomponents ***//
	private const string LIST_COMP_TAG = "listcomponents";
	private const string LIST_COMP_USAGE = "listall <GameObjectName>";
	private const string LIST_COMP_HELP = "Lists all components of the given GameObject.";
	public void listCompDelegate(string[] split, out bool showUsage){
		if(split.Length!=2){
			showUsage = true;
			return;
		}else{
			showUsage = false;
		}
		GameObject go = GameObject.Find(split[1]);
		if(!go){
			console.printLine("GameObject not found!","red");
			return;
		}
		string name = DebugConsole.getGameObjectPath(go);//refinding the GameObject here so that it is exactly correct
		foreach(Component c in go.GetComponents<Component>()){
			console.printLine(name+"."+c.GetType().Name);
		}
	}

	//*** COMMAND: destroy ***//
	private const string DESTROY_TAG = "destroy";
	private const string DESTROY_USAGE = "destroy <GameObjectName>";
	private const string DESTROY_HELP = "Destroys the given GameObject.";
	public void destroyDelegate(string[] split, out bool showUsage){
		if(split.Length!=2){
			showUsage = true;
			return;
		}else{
			showUsage = false;
		}
		string path = split[1];
		GameObject go = GameObject.Find(path);
		if(go==null){
			console.printLine ("GameObject not found!","red");
			return;
		}
		Destroy(go);
		console.printLine("Object "+DebugConsole.getGameObjectPath(go)+" destroyed");
	}

	//*** COMMAND: component info ***//
	private const string INFO_TAG = "info";
	private const string INFO_USAGE = "info <targetGameObject>.<targetComponentType>";
	private const string INFO_HELP = "Displays as much information as possible about the target Component.";
	private void infoDelegate(string[] split, out bool showUsage){
		if(split.Length!=2){
			showUsage = true;
			return;
		}else{
			showUsage = false;
		}
		string[] split2 = split[1].Split('.');
		if(split2.Length!=2){
			showUsage = true;
			return;
		}

		GameObject go = GameObject.Find (split2[0]);
		if(!go){
			console.printLine("GameObject not found!","red");
			return;
		}
		Component comp = go.GetComponent(split2[1]);
		if(!comp){
			console.printLine("Component not found on GameObject!","red");
			return;
		}
		Type compType = comp.GetType();
		console.printLine(compType.Attributes.ToString()+" "+compType.FullName);
		FieldInfo[] fields = compType.GetFields();
		console.printLine(fields.Length+" field(s) found:","yellow");
		foreach(FieldInfo fi in fields){
			console.printLine(fi.Name+" = "+fi.GetValue(comp));
		}
		PropertyInfo[] props = compType.GetProperties();
		console.printLine(props.Length+" propert(y/ies) found:","yellow");
		foreach(PropertyInfo pi in props){
			console.printLine(pi.Name+" = "+pi.GetValue(comp,null));
		}
	}
}
