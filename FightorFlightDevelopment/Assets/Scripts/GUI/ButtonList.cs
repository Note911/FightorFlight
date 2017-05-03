using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonList : MonoBehaviour {

    public MenuButton[] buttons;
    public int selectedButton;

    private bool changeSelect = false;
    public bool active = false;

    // Use this for initialization
    void Start () {
        selectedButton = 0;
        buttons[selectedButton].Select();
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (Input.GetButtonDown("A_Button"))
            {
                if (buttons[selectedButton].active)
                    buttons[selectedButton].OnClick();
            }

            if (Input.GetAxis("Vertical") > 0.2f)
                SelectUp();

            if (Input.GetAxis("Vertical") < -0.2f)
                SelectDown();
        }
    }

    void SelectUp() {
        if (!changeSelect)
        {
            changeSelect = true;
            buttons[selectedButton].UnSelect();
            selectedButton++;
            if (selectedButton >= buttons.Length)
            {
                selectedButton = 0;
            }
            buttons[selectedButton].Select();
            Invoke("Unselect", 0.3f);
        }
    }

    void SelectDown() {
        if (!changeSelect)
        {
            changeSelect = true;
            buttons[selectedButton].UnSelect();
            selectedButton--;
            if (selectedButton < 0)
            {
                selectedButton = buttons.Length - 1;
            }
            buttons[selectedButton].Select();
            Invoke("Unselect", 0.3f);
        }
    }

    void Unselect()
    {
        changeSelect = false;
    }

    public void Activate()
    {
        active = true;
        foreach (MenuButton button in buttons)
            button.active = true;
    }
}
