using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
/// <summary>
/// <para>An extensible Console for use with Unity3D.  Unity 4.6 or higher is required as DebugConsole makes use of the new UI systems.  </para>
/// <para>DebugConsole </para>
/// </summary>
public class DebugConsole : MonoBehaviour {
	private const string VERSION = "Alpha 0.1";

	//the text field that the user types into
	public InputField inputField;
	//the Text area that the console dumps into
	public Text outputField;
	//the Canvas that contains the console
	public Canvas canvas;
	//the scrollbar for the outputField (optional)
	public Scrollbar scrollbar;

	//set to true if the console itself should fix bad GameObject names, or just complain about them
	public bool fixBadGameObjectNames = false;

	//the delegate that is used to parse commands
	public delegate void parseCommand(string[] split, out bool showUsage);

	private struct Command{
		public string commandTag;
		public string usage;
		public string helpText;
		public parseCommand del;
	}

	private List<Command> commands;

	void Awake(){
		commands = new List<Command>();
	}

	//Initializes the DebugConsole
	void Start(){
		DontDestroyOnLoad(gameObject);
		Application.RegisterLogCallback(logHandler);

		//first we must check the Hierarchy for any potential problems
		int problems = 0;
		string warningString = "";
		foreach(GameObject g in GameObject.FindObjectsOfType<GameObject>()){
			string name = g.name;
			if(name.Contains(" ")){
				problems++;
				if(fixBadGameObjectNames){
					string oldname = g.name;
					string oldpath = getGameObjectPath(g);
					g.name = oldname.Replace(' ','_');
					warningString+="\nGameObject \'"+oldpath+"\' renamed to "+g.name;
				}else{
					string problem = "GameObject \""+getGameObjectPath(g)+"\" has a space in its name; " +
						"it will be impossible to interact with this GameObject or any of its children using the DebugConsole!";
					warningString+="\n<color=yellow>"+problem+"</color>";
				}
			}
		}

		//now we add the default commands that must be in this file
		addCommand("clear","clear","Clears the console.",clearConsoleDelegate);
		addCommand("help", "help", "Displays this message.", helpDelegate);
		addCommand("exit", "exit", "Shuts down the engine.",exitDelegate);
		addCommand("usage", "usage <command>", "Prints the usage information for the given command.",usageDelegate);

		//tell the user the result of the initialization
		outputField.text = "DebugConsole v"+VERSION+" loaded";
		if(problems == 0){
			outputField.text+=" successfully!";
		}else{
			outputField.text+=" with <color=red>"+problems+"</color> warning(s):"+warningString;
		}
	}

	//Did you know that Update is called every frame?
	void Update(){
		if(canvas.enabled && Input.GetKey(KeyCode.Escape)){
			canvas.enabled = false;
		}
		if(!canvas.enabled && Input.GetKey(KeyCode.BackQuote)){
			canvas.enabled = true;
			inputField.text = "";
		}
		if(canvas.enabled){
			if(EventSystem.current.currentSelectedGameObject!=inputField.gameObject){
				EventSystem.current.SetSelectedGameObject(inputField.gameObject);
//				inputField.MoveTextStart(false);
			}
		}
	}
	//Called whenever the user is "done editing" by Unity's definition; actually called whenever the user hits enter or clicks on anything else.  
	//ehh... whatever, it works
	void OnEndEdit(){
		string input = inputField.text;
		printLine("><color=green>"+input+"</color>");
		inputField.text = "";
		try{
			executeCommand(input);
		}catch(System.Exception e){
			printLine("<color=red>An Exception occured while executing the command:");
			printLine(e.GetType().Name+": "+e.Message);
			printLine("in "+e.Source);
			printLine(e.StackTrace+"</color>");
		}

	}
	/// <summary>
	/// This function is a delegate used to recieve things output into the Unity Console.  
	/// </summary>
	/// <param name="logString">Log string.</param>
	/// <param name="stackTrace">Stack trace.</param>
	/// <param name="logType">Log type.</param>
	void logHandler(string logString, string stackTrace, LogType logType){
		string message = "[<color=teal>Log</color>]";
		switch(logType){
		case LogType.Log:
			message+=stackTrace+logString;
			break;
		case LogType.Warning:
			message+="<color=yellow>"+stackTrace+logString+"</color>";
			break;
		case LogType.Error:
			message+="<color=red>"+stackTrace+logString+"</color>";
			break;
		case LogType.Exception:
			message+="<color=red>"+stackTrace+logString+"</color>";
			break;
		case LogType.Assert:
			message+="<color=red>A Unity assertion has failed! "+stackTrace+logString+"</color>";
			break;
		}
		printLine(message);
	}

	/// <summary>
	/// Adds a command to the DebugConsole.  
	/// </summary>
	/// <param name="tag">What the user actually has to input.</param>
	/// <param name="usage">The usage string; use the usage command to see how it is formatted.</param>
	/// <param name="helpText">A discription of what the command actually does; should only be one line long.</param>
	/// <param name="del">The delegate to be called when the command is used; it must take in one array of strings and an out bool.</param>
	public void addCommand(string tag, string usage, string helpText, parseCommand del){
		Command c = new Command();
		c.commandTag = tag.ToLower();
		c.usage = usage;
		c.helpText = helpText;
		c.del = del;
		commands.Add(c);
	}

	/// <summary>
	/// Prints the given line to the DebugConsole.
	/// </summary>
	/// <param name="s">S.</param>
	public void printLine(string s){
		outputField.text+="\n"+s;
		if(scrollbar){
			scrollbar.value = 0f;
		}
	}
	/// <summary>
	/// Prints the given line to the DebugConsole in the color provided.  See http://docs.unity3d.com/Manual/StyledText.html for
	/// the list of what can be put into the color field (either names or hex codes in the form #rrggbbaa).
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="color">Color.</param>
	public void printLine(string message, string color){
		printLine ("<color="+color+">"+message+"</color>");
	}
	/// <summary>
	/// Executes the command.
	/// </summary>
	/// <param name="input">Input.</param>
	public void executeCommand(string input){
		string[] split = input.Split(' ');
		string command = split[0].ToLower();

		foreach(Command c in commands){
			if(c.commandTag==command){
				bool showUsage = false;
				c.del(split,out showUsage);
				if(showUsage){
					printLine("Usage: "+c.usage,"yellow");
				}
				return;
			}
		}
		printLine("Command not found!","yellow");
	}

	//*** Some commands that for various reasons need to be included in the main DebugConsole script ***//

	private void clearConsoleDelegate(string[] split, out bool showUsage){
		outputField.text = "";
		showUsage = false;
	}

	private void helpDelegate(string[] split, out bool showUsage){
		showUsage = false;
		printLine("Command List: ","green");
		foreach(Command c in commands){
			printLine("<color=green>"+c.commandTag+"</color> - "+c.helpText);
		}
	}

	private void exitDelegate(string[] split, out bool showUsage){
		showUsage = false;
		if(Application.isEditor){
			printLine("Cannot quit from editor!", "yellow");
		}
		Application.Quit();
	}

	private void usageDelegate(string[] split, out bool showUsage){
		if(split.Length!=2){
			showUsage = true;
			return;
		}else{
			showUsage = false;
		}
		string searchTerm = split[1].ToLower();
		foreach(Command c in commands){
			if(c.commandTag == searchTerm){
				printLine(c.usage);
				return;
			}
		}
		printLine("Command not found!","yellow");
	}


	/// <summary>
	/// Utility method to get the full path of the given GameObject.  See the Unity Script Reference for GameObject.Find().
	/// </summary>
	/// <returns>The full GameObject path.</returns>
	/// <param name="obj">Object.</param>
	public static string getGameObjectPath(GameObject obj){
		if(obj.transform.parent==null)return obj.name;
		return getGameObjectPath(obj.transform.parent.gameObject)+"/"+obj.name;
	}
	
}
