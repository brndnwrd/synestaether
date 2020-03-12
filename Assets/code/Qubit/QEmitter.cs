using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QEmitter : Qubit
{
    private MarbleEmitter _emitter;

    public override void Start()
    {
        Initialize();
        //_emitter = GetComponentInChildren<MarbleEmitter>();
    }
    
    public override void OnPlace()
    {
        _emitter = GetComponentInChildren<MarbleEmitter>();
        _emitter.StartShooting();
    }

}
