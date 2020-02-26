using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class TransformHandle : MonoBehaviour
{

    public directions pointsTowards;
    private Editor _editor;
    private MeshRenderer _renderer;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _editor = FindObjectOfType<Editor>();
    }

    private void OnMouseDown()
    {
        var selQ = _editor._selected.GetComponent<Qubit>();
        selQ.Translate(pointsTowards);
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
