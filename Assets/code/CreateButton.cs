using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    public Button c_Button;
    void Start()
    {
        c_Button.onClick.AddListener(ButtonOnClickEvent);
    }

    void Update()
    {
        //if (!c_Button.isSelected)
        //{
          //  Debug.Log(2);
        //}
    }

    public void ButtonOnClickEvent()
    {
        GameObject.Find("Editor").GetComponent<Editor>().SetState(editState.Create);
    }
}
