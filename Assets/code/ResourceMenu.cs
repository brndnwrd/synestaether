using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMenu : MonoBehaviour
{
    Button Res_Back;
    Button Res_Confirm;
    InputField[] Res_Inputs;
    CanvasGroup canvas_group;
    // Start is called before the first frame update
    void Start()
    {
        Res_Back = GameObject.Find("Resource_Back").GetComponent<Button>();
        Res_Confirm = GameObject.Find("Resource_Confirm").GetComponent<Button>();
        Res_Inputs = this.GetComponentsInChildren<InputField>();
        canvas_group = this.GetComponent<CanvasGroup>();
        Res_Back.onClick.AddListener(hide);
        Res_Confirm.onClick.AddListener(confirm);
    }

    // Update is called once per frame
    public void show()
    {
        canvas_group.alpha = 1;
        canvas_group.interactable = true;
        canvas_group.blocksRaycasts = true;
    }

    public void hide()
    {
        canvas_group.alpha = 0;
        canvas_group.interactable = false;
        canvas_group.blocksRaycasts = false;
        for(int i = 0; i < Res_Inputs.Length; i++)
        {
            Res_Inputs[i].text = "";
        }
    }
    
    public void confirm()
    {
        int[] resources = new int[Res_Inputs.Length];
        for(int i = 0; i < Res_Inputs.Length; i++)
        {
            if(Res_Inputs[i].text != "")
            {
                resources[i] = int.Parse(Res_Inputs[i].text);
            }
            else
            {
                resources[i] = 0;
            }
        }
        Serialize save_level = new Serialize();
        save_level.Save_Level(resources);
    }
}
