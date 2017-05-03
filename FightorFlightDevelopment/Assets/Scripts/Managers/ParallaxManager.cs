using UnityEngine;
using System.Collections;

public class ParallaxManager : MonoBehaviour {

    //Holds transforms for the layers being parallaxed
    public Parallax[] layers;
    public Transform anchor;
    private Vector2 startPos;
	// Use this for initialization
	void Start () {
        startPos = anchor.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 distance = startPos - (Vector2)anchor.position;
        foreach (Parallax layer in layers) {
            // The layer position is equal to a percentage of the difference in position form the anchor to its original position.
            // Therefore the layer position is equal to the distance vector multiplied by a percentage of that vector to shorten it between the origin and the anchor position
            Vector2 layerPos = (Vector2)layer.startPos - new Vector2(distance.x * layer.depth, distance.y / 2 * layer.depth);
            layer.anchor.position = new Vector3(layerPos.x, layerPos.y, layer.anchor.position.z);            
        }
	}
}
