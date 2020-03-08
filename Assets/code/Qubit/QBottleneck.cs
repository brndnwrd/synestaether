using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QBottleneck : Qubit
{
    public float forceAmount = 2000f;

    private ForceField _forceField;
    
    void Start()
    {
        Initialize();
    }

    public override void OnPlace()
    {
        _forceField = transform.GetChild(0).transform.GetChild(0).GetComponent<ForceField>();
        _forceField.forceAmount = forceAmount;
    }
}
