#pragma strict
private var BGC : Transform;
private var Player : Transform;
var speed : Vector2 = Vector2(50,50);

function Start(){
Player = GameObject.Find("_Player").transform;
BGC = GameObject.Find("Background_Camera").transform;
}

function Update(){
var DTime : float = Time.smoothDeltaTime;
var H = Input.GetAxis("Horizontal") * speed.x * DTime;
var V = Input.GetAxis("Vertical") * speed.y * DTime;

Player.Rotate(Vector3(V,H,0));
BGC.rotation = Player.rotation;
}

function OnGUI () {
GUI.Label (Rect(10,10,300,320), " E : Closest Hostile \n R : Next Hostile \n F : Previous Hostile \n \n T : Next target \n Y : Previous target \n U : Closest target \n \n M : Next SubComponent Target \n N: Previous SubComponent Target \n \n B: Clear SubComponent Target  \n C : Clear target \n \n K : Display NAV List \n L : Display Target Scrolling List \n H: Display Only Hostile Contacts \n \n Z : Switch 3D <--> 2D Radar \n X : Toggle Target Indicators");
GUI.Label (Rect(10,350,300,300), " A : Rotate left \n D : Rotate Right \n S : Rotate Up \n W : Rotate Down");
GUI.Label (Rect(Screen.width - 100 ,Screen.height - 20,300,300), " Version 1.1.9a");
}
