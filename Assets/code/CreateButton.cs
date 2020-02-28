using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : Button
{
    Editor editor;
    Text text;
    LevelMenu menu;
    void Start()
    {
        onClick.AddListener(ButtonOnClickEvent);
        editor = GameObject.Find("Editor").GetComponent<Editor>();
        text = GetComponentInChildren<Text>();
        menu = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
    }

    void Update()
    {
        //editState _state = GameObject.Find("Editor").GetComponent<Editor>().GetState();
        //if (this.currentSelectionState == SelectionState.Normal && _state == editState.Create)
            //SwitchState();
    }

    public void ButtonOnClickEvent()
    {
        editor.SetState(editState.Create);
        if(this.name == "Button_Rail")
        {
            editor.SwitchQubit("QRails");
        }
        else if(this.name == "Button_Turn")
        {
            editor.SwitchQubit("QTurns");
        }
        else if(this.name == "Button_Slant")
        {
            editor.SwitchQubit("QSlants");
        }
        else if (this.name == "Serialize")
        {
            Serialize save_level = new Serialize();
            save_level.Save_Level();
        }
        else if (this.name == "Level_Menu_Button")
        {
            if (menu.getState() == 0)
            {
                menu.OpenMenu();
                SwitchState();
            }
            else
            {
                menu.CloseMenu();
                SwitchState();
            }
        }
    }

    public void SwitchState()
    {
        editor.SetState(editState.Rest);
    }

    public void ChangeText(int num)
    {
        text.text = "x " + num.ToString();
    }
}
