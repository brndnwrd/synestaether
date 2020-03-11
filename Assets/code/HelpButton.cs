using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : Button
{
    // Start is called before the first frame update
    void Start()
    {
        onClick.AddListener(OnMouseDown);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        var mainMenu = GameObject.Find("MainMenu").GetComponent<CanvasGroup>();
        var helpMenu = GameObject.Find("HelpMenu").GetComponent<CanvasGroup>();
        if (CompareTag("main-menu"))
        {
            mainMenu.alpha = 0;
            mainMenu.blocksRaycasts = false;
            mainMenu.interactable = false;
            helpMenu.alpha = 1;
            helpMenu.blocksRaycasts = true;
            helpMenu.interactable = true;
        }
        else
        {
            mainMenu.alpha = 1;
            mainMenu.blocksRaycasts = true;
            mainMenu.interactable = true;
            helpMenu.alpha = 0;
            helpMenu.blocksRaycasts = false;
            helpMenu.interactable = false;
        }
    }
}
