using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class Level : MonoBehaviour {
    private int _rows, _cols;

    private int numLayers;
    public TileMap[] layers;

    private LevelContainer _xmlContainer;
	// Use this for initialization
	void Start () {


	}

    public void init(int rows, int cols, int numLayer) {
        _rows = rows;
        _cols = cols;
        numLayers = numLayer;
        layers = new TileMap[numLayer];
        for (int i = 0; i < numLayers; ++i) {
            GameObject tileMap = new GameObject();
            tileMap.transform.SetParent(this.transform);
            tileMap.AddComponent<TileMap>();
            layers[i] = (tileMap.GetComponent<TileMap>());
            layers[i].layerNumber = i;
            layers[i].transform.position = transform.position;
            switch (i)
            {
                case (0):
                    layers[i].color = new Color(0.3f, 0.3f, 0.3f);
                    break;
                case (1):
                    layers[i].color = new Color(0.3f, 0.3f, 0.3f);
                    break;
                case (2):
                    layers[i].color = new Color(0.6f, 0.6f, 0.6f);
                    break;
                case (3):
                    layers[i].color = new Color(0.9f, 0.9f, 0.9f);
                    break;
                case (4):
                    layers[i].color = new Color(1.0f, 1.0f, 1.0f);
                    break;
                case (5):
                    layers[i].color = new Color(1.0f, 1.0f, 1.0f);
                    break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
    public void SaveLevel(string levelFilePath) {
        LevelContainer newLevel = new LevelContainer();
        newLevel.rows = _rows;
        newLevel.cols = _cols;
        newLevel.numLayers = numLayers;
        newLevel.layers = new TileMapContainer[numLayers];
        for (int x = 0; x < numLayers; ++x) {
            newLevel.layers[x] = new TileMapContainer();
            newLevel.layers[x].rows = _rows;
            newLevel.layers[x].cols = _cols;
            newLevel.layers[x].cells = new Cell[_rows * _cols];
            for(int i = 0; i < _rows; ++i) {
                for (int j = 0; j < _cols; ++j) {
                    Tile workingTile = layers[x]._tileMap[i, j].GetComponent<Tile>();
                    Cell workingCell = new Cell();
                    workingCell.x = i;
                    workingCell.y = j;
                    workingCell.scaleX = (int)workingTile.transform.localScale.x;
                    workingCell.scaleY = (int)workingTile.transform.localScale.y;
                    workingCell.rotation = (int)workingTile.transform.rotation.eulerAngles.z;
                    workingCell.tileType = (int)workingTile.GetTileType();
                    workingCell.tileShape = (int)workingTile.GetTileShape();
                    workingCell.spriteID = (int)workingTile.GetSpriteID();
                    newLevel.layers[x].cells[i + (j * _rows)] = workingCell;
                }
            }
            
        }
        _xmlContainer = newLevel;
        Debug.Log(_xmlContainer.layers.Length);
        _xmlContainer.Save(Path.Combine(Application.dataPath + "/Resources", levelFilePath + ".txt"));
    }

    public void LoadLevel(string levelFilePath) {
        for(int i = numLayers - 1; i >= 0; --i)
        {
            if (layers[i] != null)
                Destroy(layers[i].gameObject);
        }
        TextAsset container = Resources.Load<TextAsset>(levelFilePath);
        XmlSerializer serializer = new XmlSerializer(typeof(LevelContainer));
        Stream stream = new MemoryStream(container.bytes);
        _xmlContainer = serializer.Deserialize(stream) as LevelContainer;
        init(_xmlContainer.rows, _xmlContainer.cols, _xmlContainer.numLayers);
        for (int i = 0; i < numLayers; ++i)
        {
            layers[i].LoadMap(_xmlContainer.layers[i]);
            layers[i].ColorTiles(layers[i].color, 1.0f);
        }
    }

    public void LoadEmpyLevel(int rows, int col) {
        for (int i = 0; i < numLayers; ++i) {
            layers[i].LoadEmptyMap(rows, col);
            layers[i].ColorTiles(layers[i].color, 1.0f);
        }
    }
}
