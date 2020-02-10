using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : Button
{
    void Start()
    {
        onClick.AddListener(ButtonOnClickEvent);
    }

    void Update()
    {
        editState _state = GameObject.Find("Editor").GetComponent<Editor>().GetState();
        if (this.currentSelectionState == SelectionState.Normal && _state == editState.Create)
            SwitchState();
    }

    public void ButtonOnClickEvent()
    {
        GameObject.Find("Editor").GetComponent<Editor>().SetState(editState.Create);
    }

    public void SwitchState()
    {
        GameObject.Find("Editor").GetComponent<Editor>().SetState(editState.Rest);
    }
}
