using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameControl : MonoBehaviour {

    //Properties
    public static GameControl control;
    public GameObject[] characterPrefabs; //Defined in editor
    public PlayerTemplate[] playerSettings = new PlayerTemplate[4];
    public GameObject[] activePlayers = new GameObject[4];
    //[HideinInspector]
    public GameObject[] playersInScene = new GameObject[4];
    
	// Use this for initialization
	void Awake () {
	    if(control == null) {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        //if control exists but isnt equal to this, there is a copy and that copy should distroy itself
        else if (control != this) {
            Destroy(gameObject);
        }
	}

    //After Awake
    void Start() {
        ContentLoader loader = GetComponentInChildren<ContentLoader>();
        loader.LoadContent();
        if (loader.HasLoaded()) {
            Destroy(loader.gameObject);
            SceneManager.LoadScene("MainMenu");
        }
        //Create the playerSettings object in memory and then add 4 fighters to it. as fighter is the first character in the list

        for (int i = 0; i < 4; ++i) {
            playerSettings[i] = new PlayerTemplate(PlayerTemplate.CharacterIndex.FIGHTER, 0); //When skins are implemented, pass i instead of 0. atm skins do nothing
        }

        ////Until Character Select sreen is created this is where we define our active players
        //RegisterPlayer(0);
        //RegisterPlayer(1);
    }
	
	// Update is called once per frame
	void Update () {
	    TempSpriteManager.GetInstance().Update();
        EffectManager.GetInstance().Update();
	}
    
    public void RegisterPlayer(int index) {
        activePlayers[index] = GameObject.Instantiate(characterPrefabs[(int)playerSettings[index].characterIndex]);
        activePlayers[index].GetComponent<PlayableEntity>().skinIndex = playerSettings[index].skinIndex;
        activePlayers[index].SetActive(false);
        DontDestroyOnLoad(activePlayers[index]);

        //NOTE We gotta add the children of the player prefab aswell 
        activePlayers[index].GetComponent<PlayableEntity>().playerIndex = index;
        activePlayers[index].GetComponent<PlayableEntity>().AssignInputAxis();
    }

    public void UnregisterPlayer(int index)
    {
        if(activePlayers[index] != null)
        {
            playersInScene[index] = null;
            Destroy(activePlayers[index].gameObject);
            activePlayers[index] = null;
        }
    }

    //Loops through active players and spawns then into the scene while saving them into this list for reference
    public void SpawnPlayers() {
        for(int i = 0; i < 4; ++i) {
            if (activePlayers[i] != null) {
                playersInScene[i] = GameObject.Instantiate(activePlayers[i], Vector3.zero, Quaternion.identity);
                playersInScene[i].SetActive(true);
            }
        }
    }
}
