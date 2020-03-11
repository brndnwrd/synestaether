using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public GameObject level_button_prefab;
    public Sprite image1;
    public Sprite image2;
    public Sprite image3;
    public Sprite image4;
    public Sprite image5;
    public Sprite image6;
    [HideInInspector]
    CanvasGroup menu;
    int State;
    Sprite[] images;
    // Start is called before the first frame update
    void Start()
    {
        Button back = GameObject.Find("Back").GetComponent<Button>();
        back.onClick.AddListener(CloseMenu);
        menu = this.GetComponent<CanvasGroup>();
        State = 0;
        images = new Sprite[] {image1, image2, image3, image4, image5 ,image6};
    }

    public void CloseMenu()
    {
        Button[] buttons = this.GetComponentsInChildren<Button>();
        if(buttons.Length > 1)
        {
            for(int i = 2; i < buttons.Length; i++)
            {
                GameObject.Destroy(buttons[i].gameObject);
            }
        }
        if (GameObject.Find("MainMenu").CompareTag("main-menu"))
        {
            var mainMenu = GameObject.Find("MainMenu").GetComponent<CanvasGroup>();
            mainMenu.alpha = 1;
            mainMenu.blocksRaycasts = true;
            mainMenu.interactable = true;
        }
        menu.alpha = 0;
        menu.interactable = false;
        menu.blocksRaycasts = false;
        State = 0;
    }

    public void OpenMenu()
    {
        TextAsset[] dirs = Resources.LoadAll<TextAsset>("");
        int level_num = (int)System.Math.Ceiling((double)dirs.Length / 2);
        Transform Parent = menu.transform;
        for (int i = 0; i < level_num; i++) { 
            GameObject levelButton = GameObject.Instantiate(level_button_prefab, Parent);
            Transform levelButtonTransform = levelButton.transform;
            Button levelButtonButton = levelButton.GetComponent<Button>();
            TextMeshProUGUI buttonText = levelButtonButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = (i + 1).ToString();
            levelButtonTransform.Translate(new Vector3(-80+40*(i%5), 130-15*(int)(i/5)));
            Image buttonImage = levelButton.GetComponent<Image>();
            int type = new System.Random(System.Guid.NewGuid().GetHashCode()).Next(0, 5);
            buttonImage.sprite = images[0];
        }
        menu.alpha = 1;
        menu.interactable = true;
        menu.blocksRaycasts = true;
        State = 1;
    }

    public int getState()
    {
        return State;
    }
}
