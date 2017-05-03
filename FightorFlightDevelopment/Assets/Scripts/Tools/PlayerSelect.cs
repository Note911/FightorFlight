using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour {
    public Text[] readyText;
    public Text readyToFightText;

    public Sprite[] fighter;

    public GameObject[] characterPreview;

    private enum Stage { INACTIVE, STANDBY, READY };
    private Stage[] playerStage = new Stage[4];

    private bool[] changedSkinRecently = new bool[4];

    private float bButtonHoldtime = 0.0f;

	// Use this for initialization
	void Start () {
        
        Vector3 offset = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 4.0f, Screen.height / 4.0f, 0.0f));
        readyText[0].rectTransform.position = new Vector3(offset.x, -offset.y, 0.0f);
        readyText[1].rectTransform.position = new Vector3(- offset.x, -offset.y, 0.0f);
        readyText[2].rectTransform.position = new Vector3(offset.x, offset.y, 0.0f);
        readyText[3].rectTransform.position = new Vector3(-offset.x, offset.y, 0.0f);
        readyToFightText.rectTransform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < 4; ++i)
        {
            GameControl.control.UnregisterPlayer(i);
            playerStage[i] = Stage.INACTIVE;
            changedSkinRecently[i] = false;
            characterPreview[i].transform.position = readyText[i].rectTransform.position;
        }


        readyToFightText.text = " ";
    }
	
	// Update is called once per frame
	void Update () {

        for(int i = 0; i < 4; ++i) {
            switch (playerStage[i])
            {
                case (Stage.INACTIVE):
                    readyText[i].text = "Press Start";
                    break;
                case (Stage.STANDBY):
                    readyText[i].text = "A to Accept";
                    break;
                case (Stage.READY):
                    readyText[i].text = "READY!";
                    break;
            }
        }


        if (CountPlayers() >= 2)
            readyToFightText.text = "Press Start or A to Fight";
        else
            readyToFightText.text = "";

        if (Input.GetButtonDown("Start_P1") || Input.GetButtonDown("Jump_P1"))
            Accept(0);
        if (Input.GetButtonDown("Start_P2") || Input.GetButtonDown("Jump_P2"))
            Accept(1);
        if (Input.GetButtonDown("Start_P3") || Input.GetButtonDown("Jump_P3"))
            Accept(2);
        if (Input.GetButtonDown("Start_P4") || Input.GetButtonDown("Jump_P4"))
            Accept(3);


        //skin selection
        float h1 = Input.GetAxis("Horizontal_P1");
        if (h1 < -0.01f)
            CycleSkinUp(0);
        else if (h1 > 0.01f)
            CycleSkinDown(0);
        float h2 = Input.GetAxis("Horizontal_P2");
        if (h2 < -0.01f)
            CycleSkinUp(1);
        else if (h2 > 0.01f)
            CycleSkinDown(1);
        float h3 = Input.GetAxis("Horizontal_P3");
        if (h3 < -0.01f)
            CycleSkinUp(2);
        else if (h3 > 0.01f)
            CycleSkinDown(2);
        float h4 = Input.GetAxis("Horizontal_P4");
        if (h4 < -0.01f)
            CycleSkinUp(3);
        else if (h4 > 0.01f)
            CycleSkinDown(3);

        //skin selection
        if (Input.GetButtonDown("Attack_P1"))
            CycleSkinDown(0);
        if (Input.GetButtonDown("Attack_P2"))
            CycleSkinDown(1);
        if (Input.GetButtonDown("Attack_P3"))
            CycleSkinDown(2);
        if (Input.GetButtonDown("Attack_P4"))
            CycleSkinDown(3);

        //skin selection
        if (Input.GetButtonDown("LightAttack_P1"))
            CycleSkinDown(0);
        if (Input.GetButtonDown("LightAttack_P2"))
            CycleSkinDown(1);
        if (Input.GetButtonDown("LightAttack_P3"))
            CycleSkinDown(2);
        if (Input.GetButtonDown("LightAttack_P4"))
            CycleSkinDown(3);

        //skin selection
        if (Input.GetButtonDown("Dodge_P1"))
            UnregisterPlayer(0);    
        if (Input.GetButtonDown("Dodge_P2"))
            UnregisterPlayer(1);    
        if (Input.GetButtonDown("Dodge_P3"))
            UnregisterPlayer(2);    
        if (Input.GetButtonDown("Dodge_P4"))
            UnregisterPlayer(3);

        if (Input.GetButton("B_Button"))
        {
            bButtonHoldtime += Time.deltaTime;
            if (bButtonHoldtime >= 2.0f)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
            bButtonHoldtime = 0.0f;
    }

    private int CountPlayers()
    {
        int x = 0;
        for(int i = 0; i < 4; i++)
        {
            if (playerStage[i] == Stage.READY)
                    x++;
            if (playerStage[i] == Stage.STANDBY) { 
                x = 0;
                break;
            }
        }
        Debug.Log(x);
        return x;
    }

    void Accept(int player)
    {
        switch (playerStage[player]) {
            case (Stage.READY):
                if (CountPlayers() >= 2)
                    SceneManager.LoadScene("Test_Scene_01");
                break;
            case (Stage.STANDBY):
                RegisterPlayer(player);
                playerStage[player] = Stage.READY;
                break;
            case (Stage.INACTIVE):
                int skin = GameControl.control.playerSettings[player].skinIndex;
                characterPreview[player].SetActive(true);
                characterPreview[player].GetComponent<SpriteRenderer>().sprite = fighter[skin];
                playerStage[player] = Stage.STANDBY;
                break;
        }
    }

    void CycleSkinUp(int player)
    {
        if(playerStage[player] == Stage.STANDBY && !changedSkinRecently[player])
        {
            changedSkinRecently[player] = true;
            int x = GameControl.control.playerSettings[player].skinIndex;
            x++;
            if(x >= fighter.Length)
            {
                x = 0;
            }
            GameControl.control.playerSettings[player].skinIndex = x;
            characterPreview[player].GetComponent<SpriteRenderer>().sprite = fighter[x];
            StartCoroutine(changeSkinCD(player, 0.5f));
        }
    }

    void CycleSkinDown(int player)
    {
        if (playerStage[player] == Stage.STANDBY && !changedSkinRecently[player])
        {
            changedSkinRecently[player] = true;
            int x = GameControl.control.playerSettings[player].skinIndex;
            x--;
            if (x < 0)
            {
                x = fighter.Length - 1;
            }
            GameControl.control.playerSettings[player].skinIndex = x;
            characterPreview[player].GetComponent<SpriteRenderer>().sprite = fighter[x];
            StartCoroutine(changeSkinCD(player, 0.5f));

        }
    }

    IEnumerator changeSkinCD(int i, float delay)
    {
        yield return new WaitForSeconds(delay);
        changedSkinRecently[i] = false;
    }

    void RegisterPlayer(int player)
    {
        GameControl.control.RegisterPlayer(player);
    }

    void UnregisterPlayer(int player)
    {
        playerStage[player] = Stage.INACTIVE;
        characterPreview[player].SetActive(false);
        GameControl.control.UnregisterPlayer(player);
    }
}
