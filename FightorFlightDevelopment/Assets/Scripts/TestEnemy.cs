using UnityEngine;
using System.Collections;

public class TestEnemy : BaseGameEntity {

    public Frame[] sprite1;

    protected override void Awake()  {
        base.Awake();
        animationList.Add(new Animation2D((sprite1), 0, "test"));
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();

       
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}
}
