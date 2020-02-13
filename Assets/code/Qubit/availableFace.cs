using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TreeEditor;
using UnityEngine;
using UnityEngine.Assertions;
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


    private GameObject editor_object;
    private Editor editor;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _transform = GetComponent<Transform>();
        // DONE: transparency
        hoverColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        restColor = new Color(0.0f, 0.0f, 1.0f, 0.0f);
        editColor = new Color(0.0f, 1.0f, 0.0f, 0.2f);
        _renderer.material.color = restColor;
        if ( transform.parent.name == "QFloor(Clone)" || transform.parent.name == "QFloor")
        {
            // this makes floor visible, but other availableFace objects invisible unless hovered
            restColor.a = 0.1f;
        } 
        _renderer.material.color = restColor;


        editor_object = GameObject.Find("Editor");
        editor = editor_object.GetComponent<Editor>();
    }

    void OnMouseEnter()
    {
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
        editState state = editor.GetState();
        if (state == editState.Create)
        {
            _renderer.material.color = restColor;
        }
    }

    private void OnMouseOver()
    {
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

    public void DeselectColor()
    {
        _renderer.material.color = restColor;
    }

    void OnMouseDown()
    {
        editState state = editor.GetState();
        if(state == editState.Create) {
            if (this.transform.parent.name == "QFloor(Clone)")
            {
                // spa - ghet - EEEE
                String clicked_object = this.transform.parent.name;
                if (clicked_object == "QFloor(Clone)" || clicked_object == "QFloor (1)")
                {
                    //editor.PlaceQubit(_transform.position + Vector3.up * 5f);
                    editor.PlaceQubitByIndex(transform.parent.GetComponent<Qubit>().index);
                }
            }
            else
            {
                Vector3 newPos = transform.position;
                Qubit _qubit;
                if (transform.parent.name == "availableFaces")
                {  //any Qubit except QFloor
                    _qubit = transform.parent.parent.gameObject.GetComponent<Qubit>();
                }
                else
                { //QFloor
                    _qubit = transform.parent.gameObject.GetComponent<Qubit>();
                }
                var newIndex = _qubit.index;
                float dPos = 5f;
                switch (transform.name)
                {
                    case "availableFront":
                        newPos.x += dPos;
                        newIndex.x += 1.0f;
                        break;
                    case "availableBack":
                        newPos.x -= dPos;
                        newIndex.x -= 1.0f;
                        break;
                    case "availableTop":
                        newPos.y += dPos;
                        newIndex.y += 1.0f;
                        break;
                    case "availableBottom":
                        newPos.y -= dPos;
                        newIndex.y -= 1.0f;
                        break;
                    case "availableSide2":
                        newPos.z += dPos;
                        newIndex.z += 1.0f;
                        break;
                    case "availableSide1":
                        newPos.z -= dPos;
                        newIndex.z -= 1.0f;
                        break;
                    default:
                        Debug.LogWarning("Bad Face Name, No Qubit Placed");
                        return;
                }
                //editor.PlaceQubit(newPos);
                editor.PlaceQubitByIndex(newIndex);
            }
            _renderer.material.color = restColor;
            //editor.SetState(editState.Edit);
        }
        else if (state == editState.Edit)
        {
            if (editor._selected)
            {
                editor._selected.GetComponent<Qubit>().Deselect();
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
