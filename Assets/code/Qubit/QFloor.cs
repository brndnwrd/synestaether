using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class QFloor : Qubit
{
    // Start is called before the first frame update
    public override void Start()
    {
        Initialize();
        editable = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}
