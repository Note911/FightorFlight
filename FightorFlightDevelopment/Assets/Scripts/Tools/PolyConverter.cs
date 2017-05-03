using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolyConverter : MonoBehaviour {

    public PolygonCollider2D[] polygonColliderList;

    private List<Vector2[][]> pathList;
   

	// Use this for initialization
	void Start () {
        pathList = new List<Vector2[][]>();
        foreach (PolygonCollider2D polygon in polygonColliderList) {
            int pathCount = polygon.GetTotalPointCount();
            Vector2[][] shapeArray = new Vector2[pathCount][];
            for(int i = pathCount; i <= pathCount; i++)
                shapeArray[i] = polygon.GetPath(i);
            pathList.Add(shapeArray);
        }
	}
	
    public Vector2[][] GetShape(int index) {
        return pathList[index];
    }
}
