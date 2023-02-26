using UnityEngine;
using System.Collections;

public class FreeflightGUI : MonoBehaviour {
	
	[SerializeField][HideInInspector]
	Freeflight freeflight;
	[SerializeField]
	Texture2D background;
	GUIStyle style = new GUIStyle();
	
	void Start (){
		freeflight = GetComponent<Freeflight>();	
	}
		
	void OnGUI (){
		style.normal.background = background;		
		GUILayout.BeginArea (new Rect(16,16, 192, 24));				
		if(freeflight.enabled){
			GUILayout.Box("Press escape to exit freeflight");
			if(Input.GetKey (KeyCode.Escape)){
				freeflight.enabled = false;	
				Cursor.visible = true;
				Screen.lockCursor = false;
			}
		}else{
			if(GUILayout.Button ("Click to active freeflight")){
				freeflight.enabled = true;	
				Cursor.visible = false;
				Screen.lockCursor = true;
			}
		}
		GUILayout.EndArea ();
		
		GUI.backgroundColor = new Color(0,0,0, 0.8f);
		GUI.Box (new Rect(16, Screen.height - 128, 260, 108), "");
		GUILayout.BeginArea (new Rect(16, Screen.height - 128, 260, 108), style);		
		GUILayout.Label ("Freeflight controls:");
		GUILayout.Label ("WASD: move forward/backward and strafe");
		GUILayout.Label ("QE: move up and down");
		GUILayout.Label ("Mouse Wheel: +/- movement speed");
		GUILayout.EndArea ();
	}
}
