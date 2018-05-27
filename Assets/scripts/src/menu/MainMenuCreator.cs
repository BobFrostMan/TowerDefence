using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCreator : MonoBehaviour {

	void OnGUI(){
		if (GUI.Button(new Rect(Screen.width/2.5f, Screen.height/3, Screen.width/5, Screen.height/10), "Classical TD")) {
			Application.LoadLevel(1); 
		}

		if (GUI.Button(new Rect(Screen.width/2.5f, Screen.height/2, Screen.width/5, Screen.height/10), "I like drugs TD")){
			Application.LoadLevel(2); 
		}

		if (GUI.Button(new Rect(Screen.width/2.5f, Screen.height/1.5f, Screen.width/5, Screen.height/10), "Exit")){
			Application.Quit(); 
		}

	}
}
