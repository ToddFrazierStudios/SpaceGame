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

		private string getAxisPrefix(){
			return "joystick "+playerNum+" axis ";
		}
		private string getButtonPrefix(){
			return "joystick "+playerNum+" button ";
		}
		public Controller(int playerNum){
			this.playerNum = playerNum;
		}

		public float LeftTrigger{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (beginningLeft && Input.GetAxisRaw(getAxisPrefix()+ "5") == 0) {
					return 0;
				} else {
					beginningLeft = false;
					return (Input.GetAxisRaw(getAxisPrefix()+ "5") + 1)/2f;
				}
				#else
				return Input.GetAxisRaw(getAxisPrefix()+ "9");
				#endif
			}
//
//			set{
//				leftTrigger = value;
//			}
		}

		public float RightTrigger{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				if (beginningRight && Input.GetAxisRaw(getAxisPrefix()+ "6") == 0) {
					return 0;
				} else {
					beginningRight = false;
					return (Input.GetAxisRaw(getAxisPrefix()+ "6") + 1)/2f;
				}
				#else
				return Input.GetAxisRaw(getAxisPrefix()+ "10");
				#endif
			}
//			set{
//				rightTrigger = value;
//			}
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
		public float RightStickX{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				float val = Input.GetAxisRaw(getAxisPrefix()+ "3");
				#else
				float val = Input.GetAxisRaw(getAxisPrefix()+ "4");
				#endif
				if(Mathf.Abs(val)<DEAD_ZONE)return 0;
				return val;
			}
		}
		public float RightStickY{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				float val = Input.GetAxisRaw(getAxisPrefix()+ "4");
				#else
				float val = Input.GetAxisRaw(getAxisPrefix()+ "5");
				#endif
				if(Mathf.Abs(val)<DEAD_ZONE)return 0;
				return val;
			}
		}
		public bool A{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "11"); // 360: 16
				#else
				return Input.GetKey(getButtonPrefix()+ "0");
				#endif
			}
		}
		public bool Adown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "11"); // 360: 16
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "0");
				#endif
			}
		}
		public bool Aup{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "11"); // 360: 16
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "0");
				#endif
			}
		}
		public bool B{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "12"); // 360: 17
				#else
				return Input.GetKey(getButtonPrefix()+ "1");
				#endif
			}
		}
		public bool Bdown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "12"); // 360: 17
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "1");
				#endif
			}
		}
		public bool Bup{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "12"); // 360: 17
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "1");
				#endif
			}
		}
		public bool X{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "13"); // 360: 18
				#else
				return Input.GetKey(getButtonPrefix()+ "2");
				#endif
			}
		}
		public bool Xdown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "13"); // 360: 18
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "2");
				#endif
			}
		}
		public bool Xup{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "13"); // 360: 18
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "2");
				#endif
			}
		}
		public bool Y{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "14"); // 360: 19
				#else
				return Input.GetKey(getButtonPrefix()+ "3");
				#endif
			}
		}
		public bool Ydown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "14"); // 360: 19
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "3");
				#endif
			}
		}
		public bool Yup{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "14"); // 360: 19
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "3");
				#endif
			}
		}

		public bool LeftBumper{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "8"); // 360: 13
				#else
				return Input.GetKey(getButtonPrefix()+ "4");
				#endif
			}
		}
		public bool LeftBumperDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "8"); // 360: 13
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "4");
				#endif
			}
		}
		public bool LeftBumperUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "8"); // 360: 13
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "4");
				#endif
			}
		}

		public bool RightBumper{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "9");  // 360: 14
				#else
				return Input.GetKey(getButtonPrefix()+ "5");
				#endif
			}
		}
		public bool RightBumperDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "9"); // 360: 14
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "5");
				#endif
			}
		}
		public bool RightBumperUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "9");  // 360: 14
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "5");
				#endif
			}
		}

		public bool Back{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "5"); // 360: 10
				#else
				return Input.GetKey(getButtonPrefix()+ "6");
				#endif
			}
		}
		public bool BackDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "5"); // 360: 10
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "6");
				#endif
			}
		}
		public bool BackUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "5"); // 360: 10
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "6");
				#endif
			}
		}
		public bool Start{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "4"); // 360: 9
				#else
				return Input.GetKey(getButtonPrefix()+ "7");
				#endif
			}
		}
		public bool StartDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "4"); // 360: 9
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "7");
				#endif
			}
		}
		public bool StartUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "4"); // 360: 9
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "7");
				#endif
			}
		}

		public bool LS{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "6"); // 360: 11
				#else
				return Input.GetKey(getButtonPrefix()+ "8");
				#endif
			}
		}
		public bool LSDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "6"); // 360: 11
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "8");
				#endif
			}
		}
		public bool LSUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "6"); // 360: 11
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "8");
				#endif
			}
		}
		public bool RS{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "7"); // 360: 12
				#else
				return Input.GetKey(getButtonPrefix()+ "9");
				#endif
			}
		}
		public bool RSDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "7"); // 360: 12
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "9");
				#endif
			}
		}
		public bool RSUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "7"); // 360: 12
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "9");
				#endif
			}
		}
		public bool D_Down {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "1"); // 360: 6
				#else
				return false;
				#endif
			}
		}
		public bool D_DownDown {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "1"); // 360: 6
				#else
				return false;
				#endif
			}
		}
		public bool D_DownUp {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "1"); // 360: 6
				#else
				return false;
				#endif
			}
		}
		public bool D_Right {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "3"); // 360: 8
				#else
				return false;
				#endif
			}
		}
		public bool D_RightDown {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "3"); // 360: 8
				#else
				return false;
				#endif
			}
		}
		public bool D_RightUp {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "3"); // 360: 8
				#else
				return false;
				#endif
			}
		}
		public bool D_Left {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "2"); // 360: 7
				#else
				return false;
				#endif
			}
		}
		public bool D_LeftDown {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "2"); // 360: 7
				#else
				return false;
				#endif
			}
		}
		public bool D_LeftUp {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "2"); // 360: 7
				#else
				return false;
				#endif
			}
		}
		public bool D_Up {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "0"); // 360: 5
				#else
				return false;
				#endif
			}
		}
		public bool D_UpDown {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "0"); // 360: 5
				#else
				return false;
				#endif
			}
		}
		public bool D_UpUp {
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "0"); // 360: 5
				#else
				return false;
				#endif
			}
		}
	}

}
