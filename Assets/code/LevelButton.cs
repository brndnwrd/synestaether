using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    Button level;
    int index;
    Level QLevel;
    void Start()
    {
        level = this.GetComponent<Button>();
        level.onClick.AddListener(selectLevel);
        index = System.Convert.ToInt32(this.GetComponentInChildren<Text>().text);
        QLevel = GameObject.Find("Level").GetComponent<Level>();
    }

    void selectLevel()
    {
        QLevel.Initialize(index);
    }
}
