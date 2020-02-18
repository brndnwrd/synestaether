using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cardinal : MonoBehaviour
{
    public float transparency = 0.5f;
    public float angle = 0.0f;
    // Start is called before the first frame update
    private RotationRing rotationRing;
    private Material _material;
    void Start()
    {
        rotationRing = FindObjectOfType<RotationRing>();
        _material = GetComponent<MeshRenderer>().material;
        BeTransparent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
       BeSolid();
    }
    
    private void OnMouseExit() 
    {
        BeTransparent();
    }

    private void BeTransparent()
    {
        var newCol = _material.color;
        newCol.a = transparency;
        _material.color = newCol;
    }

    private void BeSolid()
    {
        var newCol = _material.color;
        newCol.a = 1f;
        _material.color = newCol;
    }

    private void OnMouseDown()
    {
        rotationRing.SetRotationDestination(angle); 
    }


}
