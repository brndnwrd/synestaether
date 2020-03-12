using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleDetector : MonoBehaviour
{
    private QBucket parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<QBucket>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        parent.GetMarble();
    }
}
