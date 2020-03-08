using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    [HideInInspector]
    public float forceAmount = 0f;


    private void OnTriggerStay(Collider other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(forceAmount * transform.forward);
        }
    }
    
}
