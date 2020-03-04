using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float forceAmount = 1.0f;


    private void OnTriggerStay(Collider other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("collision " + other);
            rb.AddForce(forceAmount * transform.forward);
        }
    }
    
}
