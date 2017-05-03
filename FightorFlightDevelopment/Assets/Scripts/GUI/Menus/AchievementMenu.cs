using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AchievementMenu : MonoBehaviour {

    private string[] achievementList;

    void OnGUI() {
        if (GUI.Button(new Rect(10, 260, 100, 30), "Back")) {
             SceneManager.LoadScene("MainMenu");
        }
    }
}
