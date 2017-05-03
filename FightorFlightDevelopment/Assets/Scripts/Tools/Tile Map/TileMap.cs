using UnityEngine;
using System.Collections;
using System.IO;

public class TileMap : MonoBehaviour {

    private int _rows, _cols;
    private float _tileWidth = 1;
    private float _tileHeight = 1;

    public int layerNumber = 0;
    public int orderInLayer = 0;

    public GameObject[,] _tileMap;
    public Color color;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void initMap(int rows, int cols) {
        _rows = rows;
        _cols = cols;
        _tileMap = new GameObject[_rows, _cols];
        
    }


    public void LoadMap(TileMapContainer xmlContainer) {
        ClearMap();
        initMap(xmlContainer.rows, xmlContainer.cols);

        for(int i = 0; i < _rows; ++i) {
            for (int j = 0; j < _cols; ++j) {
                Cell currentCell = xmlContainer.cells[i + (j * _rows)];
                _tileMap[i,j] = Tile.GenerateTile(this.transform, _tileWidth, _tileHeight, j, i, currentCell.scaleX, currentCell.scaleY, currentCell.rotation, (Tile.Type)(currentCell.tileType), (Tile.Shape)(currentCell.tileShape), currentCell.spriteID, layerNumber);
            }
        }
    }

    public void LoadEmptyMap(int rows, int cols) {
        ClearMap();
        initMap(rows, cols);
        for (int i = 0; i < _rows; ++i) {
            for (int j = 0; j < _cols; ++j) {
                if((i == 0 || i == _rows - 1 || j == 0 || j == _cols - 1) && layerNumber == 2)
                    _tileMap[i,j] = Tile.GenerateTile(this.transform, _tileWidth, _tileHeight, j, i, 1, 1, 0, Tile.Type.GROUND, Tile.Shape.FULL, 0, layerNumber);
                else
                    _tileMap[i,j] = Tile.GenerateTile(this.transform, _tileWidth, _tileHeight, j, i, 1, 1, 0, Tile.Type.AIR, Tile.Shape.NONE, 0, layerNumber);
            }
        }
    }

    public void ClearMap() {
         for(int i = 0; i < _rows; ++i) {
            for (int j = 0; j < _cols; ++j) {
                Destroy(_tileMap[i, j]);
            }
        }
    }

    public GameObject[,] GetTileMap() {
        return _tileMap;
    }

    public void ColorTiles(Color color, float alpha) {
        for(int x = 0; x < _rows; ++x)
            for(int y = 0; y < _cols; ++y) {
                color.a = alpha;
               _tileMap[x, y].GetComponent<SpriteRenderer>().color = color;
            }
    }
}
