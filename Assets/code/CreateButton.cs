using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : Button
{
    Editor editor;
    TextMeshProUGUI buttonText;
    LevelMenu menu;
    CanvasGroup group;
    int index;
    void Start()
    {
        onClick.AddListener(ButtonOnClickEvent);
        editor = GameObject.Find("Editor").GetComponent<Editor>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        menu = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
        group = this.GetComponent<CanvasGroup>();
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
        else if (this.name == "Button_BottleNeck")
        {
            editor.SwitchQubit("QBottleNeck");
        }
        else if (this.name == "Serialize")
        {
            ResourceMenu res_menu= GameObject.Find("ResourceMenu").GetComponent<ResourceMenu>();
            res_menu.show();
            SwitchState();
        }
        else if (this.name == "Level_Menu_Button")
        {
            if (menu.getState() == 0)
            {
                if (CompareTag("main-menu"))
                {
                    var mainMenu = GameObject.Find("MainMenu").GetComponent<CanvasGroup>();
                    mainMenu.alpha = 0;
                    mainMenu.interactable = false;
                    mainMenu.blocksRaycasts = false;
                }
                menu.OpenMenu();
                SwitchState();
            }
            else
            {
                if (CompareTag("main-menu"))
                {
                    var mainMenu = GameObject.Find("MainMenu").GetComponent<CanvasGroup>();
                    mainMenu.alpha = 1;
                    mainMenu.interactable = true;
                    mainMenu.blocksRaycasts = true;
                }
                menu.CloseMenu();
                SwitchState();
            }
        }
        else if (this.name == "Reset")
        {
            editor.SetState(editState.Rest);
            Level currentLevel = GameObject.Find("Level").GetComponent<Level>();
            int index = currentLevel.GetIndex();
            if (index == 0)
                return;
            currentLevel.Initialize(index);
        }
    }

    public void SwitchState()
    {
        editor.SetState(editState.Rest);
    }

    public void ChangeText(int num)
    {
        buttonText.text = "x " + num.ToString();
    }

    public int ChangeLocation(int[] resource, int j)
    {
        if(index == 0)
        {
            index = j + 1;
        }
        int res = 0;
        if(j < resource.Length)
            res = resource[j];
        if(res == 0)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
            return 0;
        }
        else
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
            int zero = 0;
            for(int i = 0; i < j; i++)
            {
                if(resource[i] == 0)
                {
                    zero++;
                }
            }
            Transform trans = this.transform;
            int coe = zero + index - j - 1;
            trans.Translate(new Vector3(0, 19.833f * coe, 0));
            index -= coe;
            ChangeText(res);
            return index;
        }
    }
}
