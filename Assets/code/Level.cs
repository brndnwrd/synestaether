using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    private int[] Resource;
    private int Number; //index of levels
    private QBucket[] _buckets;

    void Start()
    {
        UpdateObjective();
    }
    
    public void Initialize(int num)
    {
        Number = num;
        Serialize serialize = new Serialize();
        Resource = serialize.Load_Level(num);
        UpdateObjective();
        GameObject.Find("Editor").GetComponent<Editor>().UpdateLevel(this);
        GameObject.Find("Button_Rail").GetComponent<CreateButton>().ChangeText(Resource[0]);
        GameObject.Find("Button_Turn").GetComponent<CreateButton>().ChangeText(Resource[1]);
        GameObject.Find("Button_Slant").GetComponent<CreateButton>().ChangeText(Resource[2]);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelIsComplete())
        {
            CompleteLevel();
        }    
    }

    private bool LevelIsComplete()
    {
        int numToFill = _buckets.Length;
        int numFilled = 0;
        for (int i = 0; i < _buckets.Length; i++)
        {
            if (_buckets[i].isFull)
                numFilled++;
        }
        
        // levels with no buckets will never end, this is intentional
        return (numFilled > 0) && (numFilled >= numToFill);

    }

    public void UpdateObjective()
    {
        _buckets = GetComponents<QBucket>();
    }

    public void CompleteLevel()
    {
        
        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i].OnLevelComplete();
        }
        Debug.Log("Level Complete");
        /* here we do all the fun stuff that
           happens when you beat a level,
           eventually call NextLevel
        */
    }

    private void NextLevel()
    {
        // load next level or somethin
    }
    
    public int GetResource(int type)
    {
        return Resource[type];
    }

    public void SetResource(int type)
    {
        Resource[type] -= 1;
    }
}
