using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelEditor : MonoBehaviour
{
    enum ToolMode { TILE_EDIT, PLACE, INSPECT, PARALLAX };
    ToolMode toolMode = ToolMode.TILE_EDIT;
    public Camera camera;
    public int rows;
    public int cols;

    public int layers;
    //"Documents/ExampleLevelXML.xml"
    public string levelName = "Documents/CombatTesting.xml";


    private Level map;
    public int selectedLayer = 0;  //Pointer to the layer being editied

    public GameObject selectedTile;
    private Color selectedTileOriginalColor;
    private bool rotateRight = true;
    private bool rotateLeft = true;

    private bool canChangeShape = true;
    private bool canChangeType = true;
    private bool canChangeSprite = true;
    // Use this for initialization
    void Start()
    {
        //Create a child gameobject with the tile map attached
        GameObject level = new GameObject();
        level.transform.SetParent(this.transform);
        level.AddComponent<Level>();
        map = level.GetComponent<Level>();
        map.init(rows, cols, layers);
        map.LoadEmpyLevel(rows, cols);
        SelectLayer(0);
        //if (levelName != "")
        //    map.LoadLevel(levelName);
        //else

    }

    void CanRotate() { 
        rotateLeft = true;
        rotateRight = true;
    }

    void CanChangeType() {
        canChangeType = true;
    }
    void CanChangeShape() {
        canChangeShape = true;
    }       
    void CanChangeSprite() {
        canChangeSprite = true;
    }                         

    // Update is called once per frame
    void Update() {
        Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        //Sync mouse z with the z position of this game object,  since the level editor is attached the z should always remain constaint (2D)
        mousePos.z = this.transform.position.z;
        foreach(GameObject cell in map.layers[selectedLayer].GetTileMap()) {
            if(cell.GetComponent<PolygonCollider2D>().bounds.Contains(mousePos)) {
                SelectTile(cell);
                break;
            }          
        }
        GetUserInput();
        if(selectedTile != null)
            if (!selectedTile.GetComponent<PolygonCollider2D>().bounds.Contains(mousePos))
                DeselectTile();
    }

    private void SelectLayer(int n) {
        if (n >= layers)
            n = 0;
        else if (n < 0)
            n = layers - 1;
        selectedLayer = n;

        //clear all layers to their default color , for now just pass white
        for(int i = 0; i < layers; ++i) {
            map.layers[i].ColorTiles(map.layers[i].color, 1.0f);
        }
        //drop the alpha of the layers infront of the selected
        for(int i = selectedLayer + 1; i < layers; ++i) {
            map.layers[i].ColorTiles(map.layers[i].color, 0.2f);
        }
    }

    //Helper function to select a tile, gotta make sure its exclusive
    private void SelectTile(GameObject cell) {
        selectedTileOriginalColor = cell.GetComponent<SpriteRenderer>().color;
        if(selectedTile != null && selectedTile != cell) {
            selectedTile.GetComponent<SpriteRenderer>().color = selectedTileOriginalColor;
        }
    
        cell.GetComponent<SpriteRenderer>().color = Color.yellow;
        selectedTile = cell;
    }

    private void DeselectTile() {
        selectedTile.GetComponent<SpriteRenderer>().color = Color.white;
        selectedTile = null;
    }

    void OnGUI() {
        if(GUI.Button(new Rect(10,100,100,30), "Save")){
            SaveLevel(levelName);
        }
         if(GUI.Button(new Rect(10,140,100,30), "Load")){
            LoadLevel(levelName);
        }
        if(GUI.Button(new Rect(10,180,100,30), "Back")){
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void SaveLevel(string levelName) {
        map.SaveLevel(levelName);
    }

    private void LoadLevel(string levelName) {
        map.LoadLevel(levelName);
    }

    private void GetUserInput() {

        switch (toolMode) {
            case (ToolMode.TILE_EDIT):
                Vector3 newPosition = camera.transform.position;
                //Camera Controls
                newPosition.x += Input.GetAxis("Right_Stick_X") * 5.0f;
                newPosition.y -= Input.GetAxis("Right_Stick_Y") * 5.0f;
                camera.transform.position = newPosition;
                //Cycle tyle shapes
                if (selectedTile != null)
                {
                    //Grab the selected Tile
                    Tile workingTile = selectedTile.GetComponent<Tile>();
                    float triggerAxis = Input.GetAxis("Triggers");
                    float leftStickX = Input.GetAxis("Horizontal");
                    float leftStickY = Input.GetAxis("Vertical");
                    //Get the larger input axis and favor taht one
                    //Rotate Right
                    if(triggerAxis < -0.01f)/*right*/{
                        if(rotateRight && rotateLeft) {
                            rotateRight = false;
                            Invoke("CanRotate", 0.35f);
                            selectedTile.transform.Rotate(0.0f, 0.0f, 90.0f);
                        }
                    }
                    //Rotate Left
                    else if (triggerAxis > 0.01f)/*left*/{
                        if(rotateRight && rotateLeft) {
                            rotateLeft = false;
                            Invoke("CanRotate", 0.35f);
                            selectedTile.transform.Rotate(0.0f, 0.0f, -90.0f);
                        }
                    }
                            
                    //Flip
                    float dpadx = Input.GetAxis("D_Pad_X");
                    if (dpadx > 0.01f) {
                        if(canChangeShape) { 
                            if(selectedTile.GetComponent<Tile>().GetTileType() == Tile.Type.AIR) {
                                selectedTile.GetComponent<Tile>().SetTileType(Tile.Type.GROUND);
                            }
                            int i = (int)selectedTile.GetComponent<Tile>().GetTileShape();
                            ++i;
                            if (i < 3)
                                workingTile.SetTileShape((Tile.Shape)i);
                            else
                                workingTile.SetTileShape((Tile.Shape)0);
                            canChangeShape = false;
                            Invoke("CanChangeShape", 0.1f);
                        }
                    }
                    else if (dpadx < -0.01f) {
                        if(canChangeShape) { 
                            if(selectedTile.GetComponent<Tile>().GetTileType() == Tile.Type.AIR) {
                                selectedTile.GetComponent<Tile>().SetTileType(Tile.Type.GROUND);
                            }
                            int i = (int)selectedTile.GetComponent<Tile>().GetTileShape();
                            --i;
                            if (i >= 0)
                                workingTile.SetTileShape((Tile.Shape)i);
                            else
                                 workingTile.SetTileShape((Tile.Shape)2);
                            canChangeShape = false;
                            Invoke("CanChangeShape", 0.1f);
                        }
                       
                    }
                    float dpady = Input.GetAxis("D_Pad_Y");
                    if (dpady > 0.01f || dpady < -0.01f) {
                        if(canChangeType) { 
                            int i = (int)selectedTile.GetComponent<Tile>().GetTileType();
                            ++i;
                            if (i < 2)
                                workingTile.SetTileType((Tile.Type)i);
                            else
                                workingTile.SetTileType((Tile.Type)0);
                            canChangeType = false;
                            Invoke("CanChangeType", 0.1f);
                        }
                    }

                    if (Input.GetButtonDown("Right_Bumper")) /*Right*/
                    {
                        Vector3 scale = selectedTile.transform.localScale;
                        scale = new Vector3(-scale.x, scale.y, scale.z);
                        selectedTile.transform.localScale = scale;
                    }
                    if (Input.GetButtonDown("Left_Bumper"))
                    {
                        Vector3 scale = selectedTile.transform.localScale;
                        scale = new Vector3(scale.x, -scale.y, scale.z);
                        selectedTile.transform.localScale = scale;
                        
                    }
                    if (Input.GetButtonDown("A_Button"))
                    {
                        int i = (int)selectedTile.GetComponent<Tile>().GetSpriteID();
                        ++i;
                        switch(selectedTile.GetComponent<Tile>().GetTileType()) {
                            case (Tile.Type.AIR):
                                switch(selectedTile.GetComponent<Tile>().GetTileShape()) {
                                    case (Tile.Shape.NONE):
                                        i = 0;
                                        break;
                                    case (Tile.Shape.FULL):
                                        i = 0;
                                        break;
                                    case (Tile.Shape.FULL_TRI):
                                        i = 0;
                                        break;
                                }
                                break;
                            case (Tile.Type.GROUND):
                                   switch(selectedTile.GetComponent<Tile>().GetTileShape()) {
                                    case (Tile.Shape.NONE):
                                        if (i >= 9)
                                            i = 0;
                                        break;
                                    case (Tile.Shape.FULL):
                                        if (i >= 13)
                                            i = 0;
                                        break;
                                    case (Tile.Shape.FULL_TRI):
                                        if (i >= 0)
                                            i = 0;
                                        break;
                                }
                                break;
                        }
                          workingTile.SetSpriteID(i);
                    }

                    if(Input.GetButtonDown("Y_Button")) {
                        selectedLayer++;
                        SelectLayer(selectedLayer);

                    }
                }

                break;
        } 
    }
}
