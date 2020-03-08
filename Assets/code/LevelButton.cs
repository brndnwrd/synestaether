using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    Button level;
    int index;
    Level QLevel;
    private LevelMenu _levelMenu;
    void Start()
    {
        _levelMenu = GetComponentInParent<LevelMenu>();
        level = this.GetComponent<Button>();
        level.onClick.AddListener(selectLevel);
        index = System.Convert.ToInt32(this.GetComponentInChildren<TextMeshProUGUI>().text);
        QLevel = GameObject.Find("Level").GetComponent<Level>();
    }

    void selectLevel()
    {
        QLevel.Initialize(index);
        _levelMenu.CloseMenu();
    }
}
