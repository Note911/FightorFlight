using UnityEngine;
using System.Collections;

[System.Serializable]
public class Frame{

    public Sprite sprite;

    public float interval;

    public Frame(Sprite sprite) {
        this.sprite = sprite;
    }
}
