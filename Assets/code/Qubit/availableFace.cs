using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is for the availableFace prefab object
 * This object will sit on every available face in the game,
 * and will allow us and the player to know where they are
 * placing their next block
 */

public class availableFace : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _renderer;
    
    void Start()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        // you can instead use this to toggle visibility
        // atleast for debug I'm using colors green/red
        // _renderer.enabled = false; will make it disappear
        // ideally we get a wireframe shader material thing, there are free ones around
        _renderer.material.color = Color.red;
    }

    void OnMouseEnter()
    {
        _renderer.material.color = Color.green;
    }

    void OnMouseExit()
    {
        _renderer.material.color = Color.red;
    }

    private void OnMouseOver()
    {
        _renderer.material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
