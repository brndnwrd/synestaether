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
    private Transform _transform;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _transform = GetComponent<Transform>();
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

    void OnMouseDown()
    {
        String clicked_object = this.transform.parent.name;
        GameObject editor_object = GameObject.Find("Editor");
        Editor editor = editor_object.GetComponent <Editor> ();
        if(clicked_object == "QFloor(Clone)" || clicked_object== "QFloor (1)")
            editor.PlaceQubit(_transform.position);
        else if (clicked_object == "QFloor (5)")
            editor.PlaceQubit(new Vector3(_transform.position.x,_transform.position.y - 5,_transform.position.z - 5));
        else if (clicked_object == "QFloor (2)")
            editor.PlaceQubit(new Vector3(_transform.position.x + 5, _transform.position.y - 5, _transform.position.z));
        else if (clicked_object == "QFloor (3)")
            editor.PlaceQubit(new Vector3(_transform.position.x - 5, _transform.position.y - 5, _transform.position.z));
        else if (clicked_object == "QFloor (4)")
            editor.PlaceQubit(new Vector3(_transform.position.x, _transform.position.y - 5, _transform.position.z + 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
