using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class MenuButton : DynamicGUIElement {

    public Sprite unselected;
    //public Sprite selected;
    public Sprite hover;

    public string scene;

    bool isSelected = false;
    public bool active = false;


    private SpriteRenderer _renderer;
    // Use this for initialization
    protected override void Start () {
        _renderer = GetComponent<SpriteRenderer>();
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        if (isSelected)
            _renderer.sprite = hover;
        else
            _renderer.sprite = unselected;
        base.Update();
	}

    public void OnClick() {
        if (active)
        {
            if (scene == "Exit")
                Application.Quit();
            else if (scene == "")
            { }
            else
                SceneManager.LoadScene(scene);
        }
    }

    public void Select()
    {
        isSelected = true;
        transform.localScale = new Vector3(1.3f, 1.3f, 1.0f);
    }
    public void UnSelect()
    {
        isSelected = false;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
