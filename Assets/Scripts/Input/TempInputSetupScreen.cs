using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TempInputSetupScreen : MonoBehaviour {
	public InputTester tester;

    private int setupMode = 0;
	private int selectedPlayer = 0;
	private Vector2 scrollPosition = Vector2.zero;
	private const int LINE_HEIGHT = 25;
	private int controllerNumber = 0;

	private int lastDrawHeight = Screen.height;

    private InputUtils.Implementations selectedControllerType;

	private bool stringEditorOpen = false;
	private string stringEditorDefaultValue = "";
	private Controls stringEditorControlBeingEdited;
	private string stringEditorCurrentValue = "";
	private string[] stringEditorBindsToUpdate;
	private string stringEditorMetaChar;

	void Start(){
		tester.readOnly = true;
		tester.gameObject.SetActive(false);
		selectedControllerType = PlayerInput.GetControllerImplementationForPlayer(selectedPlayer);
        controllerNumber = PlayerInput.GetControllerNumber(selectedPlayer);
	}

	void OnGUI(){
		if(stringEditorOpen){
			doStringEditPopup();
			return;
		}



		scrollPosition = GUI.BeginScrollView(new Rect(0,0,Screen.width,Screen.height),scrollPosition,new Rect(0,0,Screen.width,lastDrawHeight));
		GUI.Box (new Rect(10,10,Screen.width-20,lastDrawHeight-15),"Input Setup");
		int currentLineHeight = LINE_HEIGHT*2;

        //Mode tabs at the top
        int newMode = GUI.SelectionGrid(new Rect(20, currentLineHeight, Screen.width - 40, LINE_HEIGHT), setupMode, keyboardControllerTabs, keyboardControllerTabs.Length);
        currentLineHeight += LINE_HEIGHT + 5;
        if (newMode != setupMode) {
            setupMode = newMode;
            if (setupMode == 0) {
                controllerNumber = -1;
                selectedPlayer = 0;
                selectedControllerType = InputUtils.Implementations.KEYBOARD_CONTROLLER;
                tester.playerNumber = selectedPlayer;
            } else {
                selectedPlayer = 0;
                controllerNumber = PlayerInput.GetControllerNumber(selectedPlayer);
                selectedControllerType = PlayerInput.GetControllerImplementationForPlayer(selectedPlayer);
                tester.playerNumber = selectedPlayer;
            }
        }

        if (setupMode != 0) {
            //Player tabs, if needed
            int newSelectedPlayer = GUI.SelectionGrid(new Rect(20, currentLineHeight, Screen.width - 40, LINE_HEIGHT), selectedPlayer, playerTabs, playerTabs.Length);
            currentLineHeight += LINE_HEIGHT + 5;
            if (newSelectedPlayer != selectedPlayer) {
                selectedPlayer = newSelectedPlayer;
                controllerNumber = PlayerInput.GetControllerNumber(selectedPlayer);
                if (controllerNumber < 0) controllerNumber = 0;
                selectedControllerType = PlayerInput.GetControllerImplementationForPlayer(selectedPlayer);
                tester.playerNumber = selectedPlayer;
            }

            //now for the controller type...
            float contTypeWidth = GUI.skin.label.CalcSize(new GUIContent(SELECT_CONTROLLER_TYPE_STRING)).x;
            GUI.Label(new Rect(20, currentLineHeight, contTypeWidth, LINE_HEIGHT), SELECT_CONTROLLER_TYPE_STRING);
            Rect contrTypeGridRect = new Rect(20 + contTypeWidth, currentLineHeight, Screen.width - 40 - contTypeWidth, LINE_HEIGHT);
            InputUtils.Implementations newSelectedControllerType =
                (InputUtils.Implementations)GUI.SelectionGrid(contrTypeGridRect, (int)selectedControllerType, controllerTypeTabs, controllerTypeTabs.Length);
            currentLineHeight += LINE_HEIGHT + 5;
            if (newSelectedControllerType != selectedControllerType) {
                selectedControllerType = newSelectedControllerType;
                controllerNumber = PlayerInput.GetControllerNumber(selectedPlayer);
                if (controllerNumber < 0) controllerNumber = 0;
                PlayerInput.AssignControllerToPlayer(selectedPlayer, selectedControllerType, controllerNumber);
            }
        } else {
            selectedControllerType = InputUtils.Implementations.KEYBOARD_CONTROLLER;
            selectedPlayer = -1;
            controllerNumber = 0;
        }

		//now that we know what kind of controller we are dealing with, we can move on
        if (selectedControllerType != InputUtils.Implementations.NONE) {
            if (selectedControllerType != InputUtils.Implementations.KEYBOARD_CONTROLLER) {
				//allow for selecting the controller number
				float contNumWidth = GUI.skin.label.CalcSize(new GUIContent(SELECT_CONTROLLER_NUMBER_STRING)).x;
				GUI.Label(new Rect(20,currentLineHeight,contNumWidth,LINE_HEIGHT),SELECT_CONTROLLER_NUMBER_STRING);
				Rect contrNumGridRect = new Rect(20+contNumWidth,currentLineHeight,Screen.width-40-contNumWidth,LINE_HEIGHT);
				int newControllerNumber = GUI.SelectionGrid(contrNumGridRect,controllerNumber,controllerNumTabs,controllerNumTabs.Length);
				if(newControllerNumber != controllerNumber){
					controllerNumber = newControllerNumber;
					PlayerInput.AssignControllerToPlayer(selectedPlayer,selectedControllerType,controllerNumber);
				}
				currentLineHeight+=LINE_HEIGHT+5;

                if (selectedControllerType == InputUtils.Implementations.UNITY_CONTROLLER) {
					string[] names = Input.GetJoystickNames();
					string cont = "None";
					if(controllerNumber<names.Length) cont = names[controllerNumber];
					string connectedControllerString = CONNECTED_CONTROLLER_NAME_STRING + cont;

					float conStrWidth = GUI.skin.label.CalcSize(new GUIContent(connectedControllerString)).x;
					GUI.Label (new Rect(20,currentLineHeight,conStrWidth,LINE_HEIGHT),connectedControllerString);
					currentLineHeight+=LINE_HEIGHT+5;

                    string profileString = CURRENT_PROFILE_STRING + PlayerInput.GetProfileName(selectedPlayer);
                    float profStrWidth = GUI.skin.label.CalcSize(new GUIContent(profileString)).x;
                    GUI.Label(new Rect(20, currentLineHeight, profStrWidth, LINE_HEIGHT), profileString);
                    currentLineHeight += LINE_HEIGHT + 5;
				}
			}

			//now it's time to list the bindings!
			float widthUsed = 0.0f;
			currentLineHeight = doBindingUI(currentLineHeight, out widthUsed);

			//Add a reset button
			float resetStringWidth = GUI.skin.label.CalcSize(new GUIContent(RESET_BINDINGS_STRING)).x;
			if(GUI.Button (new Rect(20,currentLineHeight,resetStringWidth+10,LINE_HEIGHT),RESET_BINDINGS_STRING)){
				PlayerInput.ResetBindingsToDefault(selectedPlayer);
			}
			currentLineHeight+=LINE_HEIGHT+5;

			//Add a toggle DebugHUD button
			float toggleDebugWidth = GUI.skin.label.CalcSize(new GUIContent(TOGGLE_DEBUG_STRING)).x;
			if(GUI.Button (new Rect(20,currentLineHeight,toggleDebugWidth+10,LINE_HEIGHT),TOGGLE_DEBUG_STRING)){
				tester.gameObject.SetActive(!tester.gameObject.activeInHierarchy);
			}
			currentLineHeight+=LINE_HEIGHT+5;
			
		}
		lastDrawHeight = currentLineHeight+20;
		GUI.EndScrollView();
	}

	private int doBindingUI(int currentLineHeight, out float maxWidthUsed){
		maxWidthUsed = 0.0f;
		for(int i = 0; i<(int)Controls.NUMBER_OF_CONTROLS; i++){
			float widthUsed = 0.0f;
			int newLineHeight;
			doControlLine ((Controls)i, currentLineHeight, out widthUsed, out newLineHeight);
			currentLineHeight = newLineHeight;
			if(widthUsed>maxWidthUsed)maxWidthUsed = widthUsed;
		}
		return currentLineHeight;
	}

	private void doControlLine(Controls control, int currentLineHeight, out float widthUsed, out int newLineHeight){
		int maxHeightUsed = LINE_HEIGHT+5;

        bool isAnalog = !InputUtils.IsADigitalControl(control);

		bool bindsUpdated = false;

		//starting label
		float nameSize = GUI.skin.label.CalcSize(new GUIContent(controlNames[(int)control])).x;
		GUI.Label(new Rect(20,currentLineHeight,nameSize,LINE_HEIGHT), controlNames[(int) control]);

		float currentX = 20+nameSize+10;
		float maxWidthUsed = currentX;
		string fullBindString = PlayerInput.GetBindingsForControl(selectedPlayer, control);
		if(fullBindString!="NOBIND"){
            string[] binds = fullBindString.Split(InputUtils.BIND_SEPERATOR, System.StringSplitOptions.RemoveEmptyEntries);
			string[] outBinds = binds.Clone() as string[];
			for(int i = 0; i<binds.Length; i++){
				int heightUsed = 0;
				string newBind = "";
				float width = 0.0f;
				int status = doBindBox(binds[i], isAnalog, currentX, currentLineHeight, out newBind, out heightUsed, out width);
				if(heightUsed>maxHeightUsed)maxHeightUsed = heightUsed;
				maxWidthUsed+=width;
				currentX+=width;
				if(status==EDIT_BIND_STRING){
					stringEditorOpen = true;
					stringEditorDefaultValue = binds[i].Substring(1);
					stringEditorControlBeingEdited = control;
					stringEditorCurrentValue = stringEditorDefaultValue;
					stringEditorBindsToUpdate = outBinds;
					stringEditorMetaChar = binds[i].Substring(0,1);
					stringEditorBindsToUpdate[i] = "STRINGEDIT";
				}
				if(status == EDIT_BIND_META){
					outBinds[i] = newBind;
					bindsUpdated = true;
				}
				if(status == DELETE_BIND){
					outBinds[i] = "REMOVED";
					bindsUpdated = true;
				}
			}
			if(bindsUpdated){
				Stack<string> stringStack = new Stack<string>();
				for(int i = outBinds.Length-1; i>=0; i--){
					if(outBinds[i]!="REMOVED"){
						stringStack.Push (outBinds[i]);
					}
				}
				string newBinds = "NOBIND";
                if (stringStack.Count != 0) newBinds = string.Join(new string(InputUtils.BIND_SEPERATOR), stringStack.ToArray());
				PlayerInput.RebindControl(selectedPlayer, control,newBinds);
				Debug.Log("Binds updated!");
			}
		}
		float addButtonWidth = GUI.skin.label.CalcSize(new GUIContent(ADD_BUTTON_STRING)).x+20;
		if(GUI.Button (new Rect(currentX,currentLineHeight,addButtonWidth,LINE_HEIGHT),ADD_BUTTON_STRING)){
			stringEditorOpen = true;
			stringEditorDefaultValue = "";
			stringEditorControlBeingEdited = control;
			stringEditorCurrentValue = stringEditorDefaultValue;
			stringEditorBindsToUpdate = new string[]{fullBindString};
			stringEditorMetaChar = "0";
		}


		widthUsed = maxWidthUsed;
		newLineHeight = currentLineHeight + maxHeightUsed;
	}

	private int doBindBox(string bind, bool isAnalogControl, float startX, float startY, out string newBind, out int heightUsed, out float width){
		int returnValue = NO_CHANGES;


		//step 1: extract the metadata
		string metaChar = bind.Substring(0,1);
		int meta = System.Int32.Parse(metaChar,System.Globalization.NumberStyles.AllowHexSpecifier);//parse the meta char
		bool inverted = (meta & 8) != 0;//isolate the inversion
		meta = meta & 7;//strip out the inversion bit
		string bindString = bind.Substring(1);//strip out the meta char
		int newMeta = meta;
		bool newInverted = inverted;

		//step 1.5 figure out box dimensions
		bool isAnalogBind = isAnAnalogBind(bindString);
		int linesNeeded = 5;
		if(isAnalogBind==isAnalogControl){
			linesNeeded = 4;
		}
		int boxHeight = linesNeeded*(LINE_HEIGHT+5)+5;
		float deleteButtonWidth = GUI.skin.label.CalcSize(new GUIContent(DELETE_BUTTON_STRING)).x + 20;
		float bindStringWidth = GUI.skin.label.CalcSize(new GUIContent(bindString)).x;
		float widthNeeded = Mathf.Max (deleteButtonWidth,bindStringWidth) + 20;
		width = widthNeeded+5;
		heightUsed = boxHeight+10;

		//step 2: display the bindString in the box

		GUI.Box(new Rect(startX,startY,widthNeeded,boxHeight),bindString);
		startY+=LINE_HEIGHT+5;
		startX+=5;

		//step 3: display the list of BindTypes
		if(isAnalogControl && isAnalogBind){//Analog binding for an analog control, meta must be 3
			if(meta!=3)newMeta = 3;
		}else if(!isAnalogControl && !isAnalogBind){//digital binding for a digital control, meta must be 0
			if(meta!=0)newMeta = 0;
		}else if(isAnalogControl){//digital binding for an analog control, meta must be 4 or 5
			if(meta!=4 && meta!=5)newMeta = 4;
			float toggleWidth = GUI.skin.label.CalcSize(new GUIContent(PUSH_NEGATIVE_STRING)).x + 25;
			if(GUI.Toggle(new Rect(startX,startY,toggleWidth,LINE_HEIGHT),newMeta==5,PUSH_NEGATIVE_STRING)){
				newMeta = 5;
			}else{
				newMeta = 4;
			}
			startY+=LINE_HEIGHT+5;
		}else {//analog binding for a digital control, meta must be 1 or 2
			if(meta!=1 && meta!=2)newMeta = 1;
			float toggleWidth = GUI.skin.label.CalcSize(new GUIContent(ACTIVATE_ON_NEGATIVE_STRING)).x + 25;
			if(GUI.Toggle(new Rect(startX,startY,toggleWidth,LINE_HEIGHT),newMeta==2,ACTIVATE_ON_NEGATIVE_STRING)){
				newMeta = 2;
			}else{
				newMeta = 1;
			}
			startY+=LINE_HEIGHT+5;
		}

		//step 4: display the 'inverted' checkbox
		float invertCheckWidth = GUI.skin.label.CalcSize(new GUIContent(INVERTED_STRING)).x + 25;
		newInverted = GUI.Toggle(new Rect(startX, startY, invertCheckWidth, LINE_HEIGHT), inverted, INVERTED_STRING);
		startY+=LINE_HEIGHT+5;

		//step 5: display the 'Edit' button
		float editButtonWidth = GUI.skin.label.CalcSize(new GUIContent(EDIT_BUTTON_STRING)).x + 20;
		if(GUI.Button(new Rect(startX,startY,editButtonWidth,LINE_HEIGHT),EDIT_BUTTON_STRING)){
			returnValue = EDIT_BIND_STRING;
		}
		startY+=LINE_HEIGHT+5;

		//step 6: display the 'Remove' button

		if(GUI.Button(new Rect(startX,startY,deleteButtonWidth,LINE_HEIGHT),DELETE_BUTTON_STRING)){
			returnValue = DELETE_BIND;
		}
		startY+=LINE_HEIGHT+5;

		//step 7: determine return values
		//rebuild the meta char, if needed
		if(newMeta!=meta || newInverted!=inverted){
			returnValue = EDIT_BIND_META;
			//reinsert the inversion flag
			if(newInverted){
				newMeta = newMeta | 8;
			}else{
				newMeta = newMeta & 7;
			}
			//convert that to the proper char
			metaChar = newMeta.ToString("X1");
		}
		newBind = metaChar + bindString;
		return returnValue;
	}

	private void doStringEditPopup(){
		Vector2 center = new Vector2(Screen.width/2f,Screen.height/2f);
		Rect editorRect = new Rect(center.x-(STRING_EDITOR_WIDTH/2),center.y-(STRING_EDITOR_HEIGHT),STRING_EDITOR_WIDTH,STRING_EDITOR_HEIGHT);
		GUI.Box(editorRect,"Enter Binding Name");
		GUI.BeginGroup(editorRect);
		Rect textAreaRect = new Rect(10,LINE_HEIGHT+5, STRING_EDITOR_WIDTH-20, LINE_HEIGHT);
		stringEditorCurrentValue = GUI.TextField(textAreaRect,stringEditorCurrentValue);

		Rect cancelButtonRect = new Rect(10,(LINE_HEIGHT+5)*2,STRING_EDITOR_WIDTH/3, LINE_HEIGHT);
		if(GUI.Button(cancelButtonRect,"Cancel")){
			stringEditorCurrentValue = stringEditorDefaultValue;
			writeBackFromStringEditor();
			stringEditorOpen = false;
		}
		Rect okButtonRect = new Rect(STRING_EDITOR_WIDTH-(STRING_EDITOR_WIDTH/3)-10,(LINE_HEIGHT+5)*2,STRING_EDITOR_WIDTH/3,LINE_HEIGHT);
		if(GUI.Button(okButtonRect,"Ok")){
			writeBackFromStringEditor();
			stringEditorOpen = false;
		}

		GUI.EndGroup();
	}

	private void writeBackFromStringEditor(){
		string stringToWrite = stringEditorMetaChar+stringEditorCurrentValue;
		bool found = false;
		for(int i = 0; i<stringEditorBindsToUpdate.Length; i++){
			if(stringEditorBindsToUpdate[i]=="STRINGEDIT"){
				stringEditorBindsToUpdate[i] = stringToWrite;
				found = true;
				break;
			}
		}
        string output = string.Join(new string(InputUtils.BIND_SEPERATOR), stringEditorBindsToUpdate);
		if(!found)output = output+";"+stringToWrite;
		if(output==";"||string.IsNullOrEmpty(output))output = "NOBIND";
		PlayerInput.RebindControl(selectedPlayer, stringEditorControlBeingEdited,output);
	}

	private static bool isAnAnalogBind(string bindString){
		switch(bindString){
		case "Mouse X":
		case "Mouse Y":
		case "MouseAbs X":
		case "MouseAbs Y":
		case "Mouse Wheel":
		case "Triggers.Right":
		case "Triggers.Left":
		case "ThumbSticks.Left.X":
		case "ThumbSticks.Left.Y":
		case "ThumbSticks.Right.X":
		case "ThumbSticks.Right.Y":
			return true;
		default:
			return false;
		}
	}


	//static strings
	private const string SELECT_CONTROLLER_TYPE_STRING = "Controller Type: ";
	private const string CONNECTED_CONTROLLER_NAME_STRING = "Connected Controller: ";
	private const string SELECT_CONTROLLER_NUMBER_STRING = "Controller Number: ";
	private const string RESET_BINDINGS_STRING = "Reset all bindings to defaults";
	private const string TOGGLE_DEBUG_STRING = "Toggle Controller Input Display";
	private const string INVERTED_STRING = "Inverted";
	private const string PUSH_NEGATIVE_STRING = "Push Negative";
	private const string ACTIVATE_ON_NEGATIVE_STRING = "Activate on Negative";
	private const string EDIT_BUTTON_STRING = "Edit Binding";
	private const string DELETE_BUTTON_STRING = "Remove Binding";
	private const string ADD_BUTTON_STRING = "Add Binding";
    private const string CURRENT_PROFILE_STRING = "Profile: ";
    private static string[] keyboardControllerTabs = new string[] { "Keyboard Setup", "Controller Setup" };
	private static string[] playerTabs = new string[]{"Player 1", "Player 2", "Player 3", "Player 4"};
	private static string[] controllerNumTabs = new string[]{"Controller 1", "Controller 2", "Controller 3", "Controller 4"};
	private static string[] controllerTypeTabs = new string[]{
		"None",
		"Unity Controller"
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		,"XInput Controller"
#endif
	};
	private static string[] controlNames = new string[]{
		"Look X",
		"Look Y",
		"Strafe X",
		"Strafe Y",
		"Roll",
		"Throttle",
		"Head Turn X",
		"Head Turn Y",
		"Primary Fire",
		"Alternate Fire",
		"Dampeners",
		"Boost",
		"Radar Mode",
		"Camera Mode",
		"Pause",
		"Targeting",
		"Next Weapon",
		"Previous Weapon",
		"Select Weapon 1",
		"Select Weapon 2",
		"Select Weapon 3",
		"Select Weapon 4",
		"Head Turn"
	};

	private const int NO_CHANGES = 0;
	private const int EDIT_BIND_STRING = 1;//implies EDIT_BIND_META
	private const int EDIT_BIND_META = 2;
	private const int DELETE_BIND = 3;

	private const int STRING_EDITOR_WIDTH = 500;
	private const int STRING_EDITOR_HEIGHT = (LINE_HEIGHT+5)*3;
}
