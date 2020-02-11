using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TreeEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
    private Color hoverColor;
    private Color restColor;
    private Color editColor;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _transform = GetComponent<Transform>();
        // DONE: transparency
        hoverColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        restColor = new Color(0.0f, 0.0f, 1.0f, 0.2f);
        editColor = new Color(0.0f, 1.0f, 0.0f, 0.4f);
        _renderer.material.color = restColor;
    }

    void OnMouseEnter()
    {
        GameObject editor_object = GameObject.Find("Editor");
        Editor editor = editor_object.GetComponent<Editor>();
        editState state = editor.GetState();
        if (state == editState.Create)
        {
            _renderer.material.color = hoverColor;
        }
        else if (_renderer.material.color == hoverColor)
        {
            _renderer.material.color = restColor;
        }
    }

    void OnMouseExit()
    {
        GameObject editor_object = GameObject.Find("Editor");
        Editor editor = editor_object.GetComponent<Editor>();
        editState state = editor.GetState();
        if (state == editState.Create)
        {
            _renderer.material.color = restColor;
        }
    }

    private void OnMouseOver()
    {
        GameObject editor_object = GameObject.Find("Editor");
        Editor editor = editor_object.GetComponent<Editor>();
        editState state = editor.GetState();
        if (state == editState.Create)
        {
            _renderer.material.color = hoverColor;
        }
        else if (_renderer.material.color == hoverColor)
        {
            _renderer.material.color = restColor;
        }
    }

    public void Deselect()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.GetComponent<availableFace>()._renderer.material.color = restColor;
        }
    }

    void OnMouseDown()
    {
        
        GameObject editor_object = GameObject.Find("Editor");
        Editor editor = editor_object.GetComponent<Editor>();
        editState state = editor.GetState();
        if(state == editState.Create) {
            if (this.transform.parent.name == "QFloor(Clone)")
            {
                String clicked_object = this.transform.parent.name;
                if (clicked_object == "QFloor(Clone)" || clicked_object == "QFloor (1)")
                    editor.PlaceQubit(_transform.position + Vector3.up * 5f);
                else if (clicked_object == "QFloor (5)")
                    editor.PlaceQubit(new Vector3(_transform.position.x, _transform.position.y,
                        _transform.position.z - 5));
                else if (clicked_object == "QFloor (2)")
                    editor.PlaceQubit(new Vector3(_transform.position.x + 5, _transform.position.y - 5,
                        _transform.position.z));
                else if (clicked_object == "QFloor (3)")
                    editor.PlaceQubit(new Vector3(_transform.position.x - 5, _transform.position.y - 5,
                        _transform.position.z));
                else if (clicked_object == "QFloor (4)")
                    editor.PlaceQubit(new Vector3(_transform.position.x, _transform.position.y - 5,
                        _transform.position.z + 5));
            }
            else
            {
                Vector3 newPos = transform.position;
                float dPos = 5f;
                switch (transform.name)
                {
                    case "availableFront":
                        newPos.x += dPos;
                        break;
                    case "availableBack":
                        newPos.x -= dPos;
                        break;
                    case "availableTop":
                        newPos.y += dPos;
                        break;
                    case "availableBottom":
                        newPos.y -= dPos;
                        break;
                    case "availableSide2":
                        newPos.z += dPos;
                        break;
                    case "availableSide1":
                        newPos.z -= dPos;
                        break;
                    default:
                        Debug.LogWarning("Bad Face Name, No Qubit Placed");
                        return;
                }
                editor.PlaceQubit(newPos);
            }
            _renderer.material.color = restColor;
            //editor.SetState(editState.Edit);
        }
        else if (state == editState.Edit)
        {
            if (editor._selected)
            {
                editor._selected.transform.Find("availableFaces").transform.GetChild(0).gameObject.GetComponent<availableFace>().Deselect();
            }

            // Make sure we're not selecting part of the editor
            if (!this.transform.parent.transform.parent.gameObject.GetComponent<Editor>())
            {
                editor._selected = this.transform.parent.transform.parent.gameObject;

                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    transform.parent.GetChild(i).gameObject.GetComponent<availableFace>()._renderer.material.color = editColor;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
