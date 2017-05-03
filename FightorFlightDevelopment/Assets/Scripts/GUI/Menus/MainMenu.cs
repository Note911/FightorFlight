using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class MainMenu : MonoBehaviour {

    public DynamicGUIElement borderBot, borderTop, treeCluster, logo;
    public GameObject scrollingBackground;
    public GameObject Background2;
    public Text startText;

    private bool userHitStart = false;

    public DynamicGUIElement[] howtoPlayScreens;
    private bool howToPlay = false;
    private int howToPlayIndex = 0;

    public ButtonList buttonList;
    

    void Start()
    {
        TempSpriteManager.GetInstance().PlayAnimation("Title_Intro", new Vector2(0,0), new Vector2(1.1f , 1), "UI", 4);
        startText.enabled = false;
        Invoke("EnableStartText", 4.5f);
    }


    void Update()
    {
        if (howToPlay)
            buttonList.active = false;
        else
            buttonList.active = true;

        if(Input.GetButtonDown("A_Button") && buttonList.selectedButton == 1) {
            MoveInHowToPlayScreen(howToPlayIndex);
            Debug.Log(howToPlayIndex);
        }

        if (Input.GetButtonDown("Start_Button"))
        {
            if (!userHitStart)
            {
                Transition();
                buttonList.Activate();
                userHitStart = true;
            }
        }


    }

    void Transition()
    {
        borderBot.MoveTo(borderBot.transform.position + Vector3.down * 100);
        borderTop.MoveTo(borderBot.transform.position + Vector3.up * 100);
        startText.enabled = false;
        Invoke("Transition2", 0.5f);
    }

    void Transition2()
    {
        treeCluster.MoveTo(treeCluster.onScreenPos);
        Invoke("Transition3", 1.0f);
        Invoke("DisableBackground", 0.75f);
    }
    void Transition3()
    {
        logo.MoveTo(logo.transform.position + Vector3.up * 2);
        DropinButtonList();
    }

    void DisableBackground()
    {
        scrollingBackground.SetActive(false);
    }

    void EnableStartText()
    {
        if(!userHitStart)
            startText.enabled = true;
    }


    void DropinButtonList()
    {
        foreach (MenuButton button in buttonList.buttons)
        {
            button.MoveTo(button.onScreenPos);
        }
    }


    public void PlayButton()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void LevelEditorButton()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void MoveInHowToPlayScreen(int index) {
        ClearHowToPlay();
        if (index >= howtoPlayScreens.Length) { 
            howToPlay = false;
            howToPlayIndex = 0;
        }
        else {
            howToPlay = true;
            howtoPlayScreens[index].MoveTo(howtoPlayScreens[index].onScreenPos);
            howToPlayIndex++;
        }

    }

    public void ClearHowToPlay() {
        foreach (DynamicGUIElement element in howtoPlayScreens) {
                element.MoveTo(element.startPos);
        }
    }


    //void OnGUI() {
    //    if (GUI.Button(new Rect(10, 100, 100, 30), "Play")) {
    //        EditorSceneManager.LoadScene("GameModeSelect");
    //    }
    //    if (GUI.Button(new Rect(10, 140, 100, 30), "Achievements")) {
    //        EditorSceneManager.LoadScene("AchievementList");
    //    }
    //    if (GUI.Button(new Rect(10, 180, 100, 30), "Level Editor")) {
    //        EditorSceneManager.LoadScene("LevelEditor");
    //    }
    //    if (GUI.Button(new Rect(10, 220, 100, 30), "Options")) {
    //         EditorSceneManager.LoadScene("OptionsMenu");
    //    }
    //    if (GUI.Button(new Rect(10, 260, 100, 30), "Exit Game")) {
    //        Application.Quit();
    //    }
    //}
}
