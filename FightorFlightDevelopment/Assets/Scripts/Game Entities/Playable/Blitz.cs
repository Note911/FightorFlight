using UnityEngine;
using System.Collections;

public class Blitz : PlayableEntity {

    private string[] hitSFX;

    protected override void Awake() {
        base.Awake();

        hitSFX = new string[36];
        for (int i = 0; i < 36; ++i)
            hitSFX[i] = "Hits_" + i;
    }

    // Use this for initialization
    protected override void Start() {

        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

 
}
