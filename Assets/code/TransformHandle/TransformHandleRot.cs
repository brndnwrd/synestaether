using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHandleRot : MonoBehaviour
{
    // +1 or -1
    public int rotAngle;
    private Editor _editor;
    private MeshRenderer _renderer;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _editor = FindObjectOfType<Editor>();
       beTransparent(); 
    }

    private void OnMouseDown()
    {
        _editor._selected.GetComponent<Qubit>().Rotate(rotAngle);
        _editor.UpdateTransformHandle();
    }

    private void OnMouseEnter()
    {
        beOpaque();
    }

    private void OnMouseExit()
    {
        beTransparent();
    }

    private void beTransparent()
    {
        var newCol = _renderer.material.color;
        newCol.a = 0.5f;
        _renderer.material.color = newCol;
    }

    private void beOpaque()
    {
        var newCol = _renderer.material.color;
        newCol.a = 1f;
        _renderer.material.color = newCol;
    }
}
