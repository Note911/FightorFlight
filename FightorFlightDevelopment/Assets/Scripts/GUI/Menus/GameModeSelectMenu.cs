using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameModeSelectMenu : MonoBehaviour {

	void OnGUI() {
        if(GUI.Button(new Rect(10,100,100,30), "VS")){
            SceneManager.LoadScene("CharacterSelect");
        }
        if(GUI.Button(new Rect(10,140,100,30), "Back")){
            SceneManager.LoadScene("MainMenu");
        }
    }
}
