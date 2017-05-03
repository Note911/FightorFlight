using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelectMenu : MonoBehaviour {

	
	void OnGUI() {
        if(GUI.Button(new Rect(10,100,100,30), "FIGHT!")){
            SceneManager.LoadScene("Test_Scene_01");
        }
        if(GUI.Button(new Rect(10,140,100,30), "Gameplay Options")){
            SceneManager.LoadScene("GameplayOptionsMenu");
        }
        if(GUI.Button(new Rect(10,180,100,30), "Back")){
            SceneManager.LoadScene("CharacterSelect");
        }
    }
}
