using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CharacterSelectMenu : MonoBehaviour {

	void OnGUI() {
        if(GUI.Button(new Rect(10,100,100,30), "LevelSelect")){
            SceneManager.LoadScene("LevelSelect");
        }
        if(GUI.Button(new Rect(10,140,100,30), "Back")){
            SceneManager.LoadScene("GameModeSelect");
        }
    }
}
