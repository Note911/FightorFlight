using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {

    public Text winnerText;
    public Text playAgainText;


	// Use this for initialization
	void Start () {
        winnerText.text = "";
        playAgainText.text = "";
	}
	
	// Update is called once per frame
	void Update () {

        //for Stock mode
        if (CountPlayers() <= 1)
        {
            int x = GetWinner();
            if (x == -1)
                winnerText.text = "Draw";
            else
                winnerText.text = "Player " + (x+1) + " Wins!";
            playAgainText.text = "Press Start to Play again";
        }

        if(winnerText.text != "")
        {
            if (Input.GetButtonDown("Start"))
                SceneManager.LoadScene("CharacterSelect");
        }

	}

    private int CountPlayers()
    {
        int x = 0;
        for (int i = 0; i < 4; i++)
        {
            if (GameControl.control.playersInScene[i] != null)
            {
                PlayableEntity player = GameControl.control.playersInScene[i].GetComponent<PlayableEntity>();
                if (player.lives > 0)
                    x++;
            }
        }
        return x;
    }

    private int GetWinner() //Returns -1 if theres no winner.  should never happen tho
    {
        int x = -1;
        for (int i = 0; i < 4; i++)
        {
            if (GameControl.control.playersInScene[i] != null)
            {
                PlayableEntity player = GameControl.control.playersInScene[i].GetComponent<PlayableEntity>();
                if (player.lives > 0)
                {
                    x = i;
                    break;
                }
            }
        }
        return x;
    }
}
