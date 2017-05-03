using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	    void OnGUI() {
        if (GUI.Button(new Rect(10, 100, 100, 30), "Controls")) {
            //Opens a submenu
        }
        if (GUI.Button(new Rect(10, 140, 100, 30), "Audio")) {
            //Opens a submenu
        }
        if (GUI.Button(new Rect(10, 180, 100, 30), "Video")) {
            //Opens a submenu
        }
        if (GUI.Button(new Rect(10, 220, 100, 30), "Back")) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
