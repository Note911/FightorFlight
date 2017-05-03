using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameplayOptionsMenu : MonoBehaviour {

		void OnGUI() {
        if (GUI.Button(new Rect(10, 100, 100, 30), "Rules")) {
            //Opens a submenu
        }
        if (GUI.Button(new Rect(10, 140, 100, 30), "Items")) {
            //Opens a submenu
        }
        if (GUI.Button(new Rect(10, 180, 100, 30), "Levels")) {
            //Opens a submenu
        }
        if (GUI.Button(new Rect(10, 220, 100, 30), "Back")) {
            SceneManager.LoadScene("LevelSelect");
        }
    }
}
