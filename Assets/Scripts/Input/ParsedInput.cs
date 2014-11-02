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
		private int playerNum = 0;

		public int PlayerNumber{
			get{
				return playerNum;
			}
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
				return (Input.GetAxisRaw(getAxisPrefix()+ "5") + 1)/2f;
				#else
				return Input.GetAxisRaw(getAxisPrefix()+ "9");
				#endif
			}
		}

		public float RightTrigger{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return (Input.GetAxis(getAxisPrefix()+ "6") + 1)/2f;
				#else
				return Input.GetAxisRaw(getAxisPrefix()+ "10");
				#endif
			}
		}
		public float LeftStickX{
			get{
				float val = Input.GetAxisRaw(getAxisPrefix()+ "X");
				if(Mathf.Abs(val)<DEAD_ZONE)return 0;
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
		public bool B{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "17");
				#else
				return Input.GetKey(getButtonPrefix()+ "1");
				#endif
			}
		}
		public bool Bdown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "17");
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "1");
				#endif
			}
		}
		public bool Bup{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "17");
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "1");
				#endif
			}
		}
		public bool X{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "18");
				#else
				return Input.GetKey(getButtonPrefix()+ "2");
				#endif
			}
		}
		public bool Xdown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "18");
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "2");
				#endif
			}
		}
		public bool Xup{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "18");
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "2");
				#endif
			}
		}
		public bool Y{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "19");
				#else
				return Input.GetKey(getButtonPrefix()+ "3");
				#endif
			}
		}
		public bool Ydown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "19");
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "3");
				#endif
			}
		}
		public bool Yup{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "18");
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "3");
				#endif
			}
		}

		public bool LeftBumper{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "13");
				#else
				return Input.GetKey(getButtonPrefix()+ "4");
				#endif
			}
		}
		public bool LeftBumperDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "13");
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "4");
				#endif
			}
		}
		public bool LeftBumperUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "13");
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "4");
				#endif
			}
		}

		public bool RightBumper{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "14");
				#else
				return Input.GetKey(getButtonPrefix()+ "5");
				#endif
			}
		}
		public bool RightBumperDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "14");
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "5");
				#endif
			}
		}
		public bool RightBumperUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "14");
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "5");
				#endif
			}
		}

		public bool Back{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "10");
				#else
				return Input.GetKey(getButtonPrefix()+ "6");
				#endif
			}
		}
		public bool BackDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "10");
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "6");
				#endif
			}
		}
		public bool BackUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "10");
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "6");
				#endif
			}
		}
		public bool Start{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKey(getButtonPrefix()+ "9");
				#else
				return Input.GetKey(getButtonPrefix()+ "7");
				#endif
			}
		}
		public bool StartDown{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyDown(getButtonPrefix()+ "9");
				#else
				return Input.GetKeyDown(getButtonPrefix()+ "7");
				#endif
			}
		}
		public bool StartUp{
			get{
				#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
				return Input.GetKeyUp(getButtonPrefix()+ "9");
				#else
				return Input.GetKeyUp(getButtonPrefix()+ "7");
				#endif
			}
		}
	}

}
