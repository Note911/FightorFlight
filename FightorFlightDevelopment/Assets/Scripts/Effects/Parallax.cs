using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

    public Transform anchor;
    public Vector3 startPos;
    public float depth;
    void Start() {
        startPos = anchor.position;
    }               
}
