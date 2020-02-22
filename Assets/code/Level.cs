using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    private float Win_State;
    private int[] Resource;
    private int Number;//index of levels

    public void Initialize(int num)
    {
        Win_State = 0;
        Number = num;
        Serialize serialize = new Serialize();
        Resource = serialize.Load_Level(num);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(float num)
    {
        Win_State += num;
    }
}
