using UnityEngine;
using System.Collections;

public class TileMapGenerator : Level {

    public string levelName = "Documents/CombatTesting.xml";

    // Use this for initialization
    void Start () {
        LoadLevel(levelName);
        GameControl.control.SpawnPlayers();
        CameraFollow.RegisterPlayers();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
