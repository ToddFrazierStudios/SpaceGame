#if false
using UnityEngine;
using System.Collections;

public class ParsedInput{
	public static float DEAD_ZONE = 0.19f;

	public static Controller[] controller = {
		new Controller(1),
		new Controller(2),
		new Controller(3),
		new Controller(4)
	};

	public sealed class Controller{
		private bool beginningLeft = true;
		private bool beginningRight = true;
		public bool invertY = false;

		private int playerNum = 0;

		public int PlayerNumber{
			get{
				return playerNum;
			}
		}

		public void ResetAllAxes() {
			Input.ResetInputAxes();
			beginningLeft = true;
			beginningRight = true;
		}

		public bool AnyKey() {
			return Input.anyKey;
		}

		public int AxisPressed() {
			if (AnyKey()) {
				if (LeftTrigger > 0f) {
					return LeftTriggerAxis;
				}
			}
			return 0;
		}

		private string getAxisPrefix(){
			return "joystick "+playerNum+" axis ";
		}
		private string getButtonPrefix(){
			return "joystick "+playerNum+" button ";
		}
		public Controller(int playerNum){
			this.playerNum = playerNum;
		}

		public enum CONTROLLERTYPE {
			XBOXONE, XBOX360, OTHER
		}

		public CONTROLLERTYPE controllerType = CONTROLLERTYPE.XBOXONE;

		private int LeftTriggerAxis {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 5;
				} else {
					return LeftTriggerAxis;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 9;
				} else {
					return LeftTriggerAxis;
				}
				#endif
			}
			set {
				LeftTriggerAxis = value;
			}
		}

		public float LeftTrigger{
			get{
				float trigger = Input.GetAxisRaw (getAxisPrefix () + LeftTriggerAxis);
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
					if (beginningLeft && trigger == 0) {
						trigger = 0;
					} else {
						beginningLeft = false;
						trigger = (trigger + 1)/2f;
					}
				#endif
				return trigger;

//				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
//				if (beginningLeft && Input.GetAxisRaw(getAxisPrefix()+ "5") == 0) {
//					return 0;
//				} else {
//					beginningLeft = false;
//					return (Input.GetAxisRaw(getAxisPrefix()+ "5") + 1)/2f;
//				}
//				#else
//				return Input.GetAxisRaw(getAxisPrefix()+ "9");
//				#endif
			}
//
//			set{
//				leftTrigger = value;
//			}
		}

		private int RightTriggerAxis {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
					if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
						return 6;
					} else {
						return RightTriggerAxis;
					}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 10;
				} else {
					return RightTriggerAxis;
				}
				#endif
			}
			set {
				RightTriggerAxis = value;
			}
		}

		public float RightTrigger{
			get{
				float trigger = Input.GetAxisRaw (getAxisPrefix () + RightTriggerAxis);
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (beginningRight && trigger == 0) {
					trigger = 0;
				} else {
					beginningRight = false;
					trigger = (trigger + 1)/2f;
				}
				#endif
				return trigger;
//				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
//				if (beginningRight && Input.GetAxisRaw(getAxisPrefix()+ "6") == 0) {
//					return 0;
//				} else {
//					beginningRight = false;
//					return (Input.GetAxisRaw(getAxisPrefix()+ "6") + 1)/2f;
//				}
//				#else
//				return Input.GetAxisRaw(getAxisPrefix()+ "10");
//				#endif
//			}
//			set{
//				rightTrigger = value;
			}
		}
		
		public float LeftStickX{
			get{
				float val = Input.GetAxisRaw(getAxisPrefix()+ "X");
				if(Mathf.Abs(val)<DEAD_ZONE)return 0;
				if(invertY)val = -val;
				return val;
			}
		}
		public float LeftStickY{
			get{
				float val = Input.GetAxisRaw(getAxisPrefix()+ "Y");
				if(Mathf.Abs(val)<DEAD_ZONE)return 0;
				return val;
			}
		}

		private int RightStickXAxis {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 3;
				} else {
					return RightStickXAxis;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 4;
				} else {
					return RightStickXAxis;
				}
				#endif
			}
			set {
				RightStickXAxis = value;
			}
		}
		public float RightStickX{
			get{
//				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
//				float val = Input.GetAxisRaw(getAxisPrefix()+ "3");
//				#else
//				float val = Input.GetAxisRaw(getAxisPrefix()+ "4");
//				#endif
				float val = Input.GetAxisRaw (getAxisPrefix () + RightStickXAxis);
				if(Mathf.Abs(val)<DEAD_ZONE)return 0;
				return val;
			}
		}

		private int RightStickYAxis {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 4;
				} else {
					return RightStickYAxis;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 5;
				} else {
					return RightStickYAxis;
				}
				#endif
			}
			set {
				RightStickYAxis = value;
			}
		}

		public float RightStickY{
			get{
//				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
//				float val = Input.GetAxisRaw(getAxisPrefix()+ "4");
//				#else
//				float val = Input.GetAxisRaw(getAxisPrefix()+ "5");
//				#endif
				float val = Input.GetAxisRaw (getAxisPrefix () + RightStickYAxis);
				if(Mathf.Abs(val)<DEAD_ZONE)return 0;
				return val;
			}
		}

		private int AButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 16;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 11;
				} else {
					return AButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 0;
				} else {
					return AButton;t
				}
				#endif
			}
			set {
				AButton = value;
			}
		}

		public bool A{
			get{
				return Input.GetKey(getButtonPrefix()+ AButton);
			}
		}
		public bool Adown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ AButton);
			}
		}
		public bool Aup{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ AButton);
			}
		}

		private int BButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 17;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 12;
				} else {
					return BButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 1;
				} else {
					return BButton;
				}
				#endif
			}
			set {
				BButton = value;
			}
		}

		public bool B{
			get{
				return Input.GetKey(getButtonPrefix()+ BButton);
			}
		}
		public bool Bdown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ BButton);
			}
		}
		public bool Bup{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ BButton);
			}
		}

		private int XButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 18;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 13;
				} else {
					return XButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 2;
				} else {
					return XButton;
				}
				#endif
			}
			set {
				XButton = value;
			}
		}

		public bool X{
			get{
				return Input.GetKey(getButtonPrefix()+ XButton);
			}
		}
		public bool Xdown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ XButton);
			}
		}
		public bool Xup{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ XButton);
			}
		}

		private int YButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 19;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 14;
				} else {
					return YButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 3;
				} else {
					return YButton;
				}
				#endif
			}
			set {
				YButton = value;
			}
		}

		public bool Y{
			get{
				return Input.GetKey(getButtonPrefix()+ YButton);
			}
		}
		public bool Ydown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ YButton);
			}
		}
		public bool Yup{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ YButton);
			}
		}

		private int LBButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 13;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 8;
				} else {
					return LBButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 4;
				} else {
					return LBButton;
				}
				#endif
			}
			set {
				RBButton = value;
			}
		}

		public bool LeftBumper{
			get{
				return Input.GetKey(getButtonPrefix()+ LBButton);
			}
		}
		public bool LeftBumperDown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ LBButton);
			}
		}
		public bool LeftBumperUp{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ LBButton);
			}
		}

		private int RBButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 14;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 9;
				} else {
					return RBButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 5;
				} else {
					return RBButton;
				}
				#endif
			}
			set {
				RBButton = value;
			}
		}

		public bool RightBumper{
			get{
				return Input.GetKey(getButtonPrefix()+ RBButton);
			}
		}
		public bool RightBumperDown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ RBButton);
			}
		}
		public bool RightBumperUp{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ RBButton);
			}
		}

		private int BackButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 10;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 5;
				} else {
					return XButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 6;
				} else {
					return BackButton;
				}
				#endif
			}
			set {
				BackButton = value;
			}
		}

		public bool Back{
			get{
				return Input.GetKey(getButtonPrefix()+ BackButton);
			}
		}
		public bool BackDown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ BackButton);
			}
		}
		public bool BackUp{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ BackButton);
			}
		}

		private int StartButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 9;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 4;
				} else {
					return XButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 7;
				} else {
					return XButton;
				}
				#endif
			}
			set {
				XButton = value;
			}
		}

		public bool Start{
			get{
				return Input.GetKey(getButtonPrefix()+ StartButton);
			}
		}
		public bool StartDown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ StartButton);
			}
		}
		public bool StartUp{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ StartButton);
			}
		}
		
		private int LSButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 11;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 6;
				} else {
					return LSButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 8;
				} else {
					return LSButton;
				}
				#endif
			}
			set {
				LSButton = value;
			}
		}

		public bool LS{
			get{
				return Input.GetKey(getButtonPrefix()+ LSButton);
			}
		}
		public bool LSDown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ LSButton);
			}
		}
		public bool LSUp{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ LSButton);
			}
		}

		private int RSButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 12;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 7;
				} else {
					return RSButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 9;
				} else {
					return RSButton;
				}
				#endif
			}
			set {
				RSButton = value;
			}
		}

		public bool RS{
			get{
				return Input.GetKey(getButtonPrefix()+ RSButton);
			}
		}
		public bool RSDown{
			get{
				return Input.GetKeyDown(getButtonPrefix()+ RSButton);
			}
		}
		public bool RSUp{
			get{
				return Input.GetKeyUp(getButtonPrefix()+ RSButton);
			}
		}
		
		private int D_DownButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 6;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 1;
				} else {
					return D_DownButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 7;
				} else {
					return D_DownButton;
				}
				#endif
			}
			set {
				D_DownButton = value;
			}
		}

		public bool D_Down {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ D_DownButton);
				#else
				return Input.GetAxisRaw(getAxisPrefix()+ D_DownButton) < DEAD_ZONE;
				#endif
			}
		}
		public bool D_DownDown {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ D_DownButton);
				#else
				return false;
				#endif
			}
		}
		public bool D_DownUp {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ D_DownButton);
				#else
				return false;
				#endif
			}
		}

		private int D_RightButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 8;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 3;
				} else {
					return D_RightButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 6;
				} else {
					return D_RightButton;
				}
				#endif
			}
			set {
				D_RightButton = value;
			}
		}

		public bool D_Right {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ D_RightButton);
				#else
				return Input.GetAxisRaw(getAxisPrefix()+ D_RightButton) > DEAD_ZONE;
				#endif
			}
		}
		public bool D_RightDown {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ D_RightButton);
				#else
				return false;
				#endif
			}
		}
		public bool D_RightUp {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ D_RightButton);
				#else
				return false;
				#endif
			}
		}
		
		private int D_LeftButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 7;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 2;
				} else {
					return D_LeftButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 6;
				} else {
					return D_LeftButton;
				}
				#endif
			}
			set {
				D_LeftButton = value;
			}
		}

		public bool D_Left {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ D_LeftButton);
				#else
				return Input.GetAxisRaw(getAxisPrefix()+ D_LeftButton) < DEAD_ZONE;
				#endif
			}
		}
		public bool D_LeftDown {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ D_LeftButton);
				#else
				return false;
				#endif
			}
		}
		public bool D_LeftUp {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ D_LeftButton);
				#else
				return false;
				#endif
			}
		}

		private int D_UpButton {
			get {
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (controllerType == CONTROLLERTYPE.XBOX360) {
					return 5;
				} else if (controllerType == CONTROLLERTYPE.XBOXONE) {
					return 0;
				} else {
					return D_UpButton;
				}
				#else
				if (controllerType == CONTROLLERTYPE.XBOX360 || controllerType == CONTROLLERTYPE.XBOXONE) {
					return 7;
				} else {
					return D_UpButton;
				}
				#endif
			}
			set {
				D_UpButton = value;
			}
		}

		public bool D_Up {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ D_UpButton);
				#else
				return Input.GetAxisRaw(getAxisPrefix()+ D_UpButton) > DEAD_ZONE;
				#endif
			}
		}
		public bool D_UpDown {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ D_UpButton);
				#else
				return false;
				#endif
			}
		}
		public bool D_UpUp {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ D_UpButton);
				#else
				return false;
				#endif
			}
		}
	}
}
#endif
