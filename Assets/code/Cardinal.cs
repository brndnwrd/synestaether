using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardinal : MonoBehaviour
{
    public float angle = 0.0f;
    // Start is called before the first frame update
    private RotationRing rotationRing;
    void Start()
    {
        rotationRing = FindObjectOfType<RotationRing>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        
    }

    private void OnMouseDown()
    {
        rotationRing.SetRotationDestination(angle); 
    }


}
