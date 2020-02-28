﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public enum editState
{
    Create,
    Rest,
    Edit,
}
public class Editor : MonoBehaviour
{ 
    /*[HideInInspector] commented this for debugging*/ public editState _state;
    [HideInInspector] public GameObject _selected;
    //public GameObject CubePrefab;
    public GameObject[,,] _grid;
    private Camera _camera;
    [HideInInspector] public int size; // number of rows/columns
    [HideInInspector] public GameObject placingQubit; // the Qubit from the menu were about to place
    public GameObject transformPrefab;
    private GameObject transformInstance;
    
    public GameObject QFloorPrefab;
    public GameObject QRailsPrefab;
    public GameObject QEmiterPrefab;
    public GameObject QTurnPrefab;
    public GameObject QSlantPrefab;
    public GameObject QFunnelPrefab;
    [HideInInspector] public GameObject GhostBlock;
    public Material GhostBlockMaterial;
    
    private Level level;

    void Start()
    {
        size = 15;
        _grid = new GameObject[size,size,size];
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        MakeFloor();
        SetState(editState.Rest);
        placingQubit = QRailsPrefab;
    }

    void Update()
    {
       KeyControls(); 
    }

    public Vector3 indexToPosition(Vector3 index)
    {
        return index * 10.0f;
    } 
    private void MakeFloor() 
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                var newbie = Instantiate(QFloorPrefab, transform);
                Vector3 indx = new Vector3(x, 0, z);
                newbie.transform.position = indexToPosition(indx);
                // organizing these as close to Unity space...
                // as possible, x/z is ground, y is up
                newbie.GetComponent<Qubit>().index = indx;
            }
        }
    }

    private void MoveQubit()
    {
        throw new NotImplementedException();
    }

    private void RotateQubit()
    {
        throw new NotImplementedException();
    }

    /*public void PlaceQubit(Vector3 position)
    {
        if (placingQubit.name == "QRails2")
        {
            if (Resource[0] == 0)
                return;
            else
            {
                position.y -= 5f;
                Resource[0] -= 1;
            }
            //this is a quick fix, need to change the prefab
            // cant figure it out atm
        }
        else if (placingQubit.name == "QTurn")
        {
            if (Resource[1] == 0)
                return;
            else
            {
                Resource[1] -= 1;
            }
        }
        else if (placingQubit.name == "QSlant-stepped")
        {
            if (Resource[2] == 0)
                return;
            else
            {
                Resource[2] -= 1;
            }
        }
        Transform parent = GameObject.Find("QBlocks").GetComponent<Transform>();
        var newCube = Instantiate(placingQubit, parent);
        newCube.transform.position = position;
    }*/

    public void PlaceQubitByIndex(Vector3 newIndex)
    {
        if (level != null)
        {
            if (placingQubit.name == "QRails-v4")
            {
                if (level.GetResource(0) == 0)
                    return;
                else
                {
                    level.SetResource(0);
                    GameObject.Find("Button_Rail").GetComponent<CreateButton>().ChangeText(level.GetResource(0));
                }
            }
            else if (placingQubit.name == "QTurn-v3")
            {
                if (level.GetResource(1) == 0)
                    return;
                else
                {
                    level.SetResource(1);
                    GameObject.Find("Button_Turn").GetComponent<CreateButton>().ChangeText(level.GetResource(1));
                }
            }
            else if (placingQubit.name == "QSlant-v4")
            {
                if (level.GetResource(2) == 0)
                    return;
                else
                {
                    level.SetResource(2);
                    GameObject.Find("Button_Slant").GetComponent<CreateButton>().ChangeText(level.GetResource(2));
                }
            }
        }
        Vector3 newPos = indexToPosition(newIndex);
        // I think this has something to do with QFloor being
        // centered in the center of its floor piece. It should
        // be centered somehow above it. 0.5 above it I guess...
        //newPos.y += 5f; 
        Transform par = GameObject.Find("QBlocks").GetComponent<Transform>();
        var newbie = Instantiate(placingQubit, par);
        var newQ = newbie.GetComponent<Qubit>();
        newQ.index = newIndex;
        newbie.transform.position = newPos;
        _grid[(int) newIndex.x, (int) newIndex.y, (int) newIndex.z] = newbie;
        newQ.OnPlace();
    }

    public void SelectQubit(GameObject go)
    {
        _selected = go;

        if (transformInstance == null)
        {
            transformInstance = Instantiate(transformPrefab);
        }
        transformInstance.transform.position = _selected.transform.position;
        
    }
    public void SwitchQubit(String name)
    {
        switch (name)
        {
            case "QRails":
                placingQubit = QRailsPrefab;
                break;
            case "QTurns":
                placingQubit = QTurnPrefab;
                break;
            case "QSlants":
                placingQubit = QSlantPrefab;
                break;
            case "QEmitter":
                placingQubit = QEmiterPrefab;
                break;
            case "QFunnel":
                placingQubit = QFunnelPrefab;
                break;
        }

        UpdateGhostBlock();
    }
    /*
     * Make sure every qubit is where it thinks it is.
     * Check Qubit.index == Editor's index, else throw
     */
    private void Verify()
    {
        throw new NotImplementedException();
    }

    public editState GetState()
    {
        return _state;
    }

    public void SetState(editState newState)
    {
//        Debug.Log("state now equal to " + newState);
        var oldState = _state;

        Ray rayToFace = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayToFace, out RaycastHit currentFace))
        {
            if (currentFace.transform.gameObject.GetComponent<availableFace>())
            {
                currentFace.transform.gameObject.GetComponent<availableFace>().OnModeSwitch(oldState);
            }
        }

        if (newState != editState.Edit)
        {
            if (_selected)
            {
                _selected.GetComponent<Qubit>().Deselect();
                _selected = null;
            }
            Destroy(transformInstance);
        }

        if (newState == editState.Create)
        {
            Destroy(GhostBlock);
            GhostBlock = MakeGhostBlock(placingQubit);
        }
        else// (newState != editState.Create)
        {
            Destroy(GhostBlock);
            GhostBlock = null;
        }

        if (newState == editState.Rest && oldState == editState.Create)
        {
            Destroy(transformInstance);
        }

        _state = newState;
        
    }

    public void UpdateGhostBlock()
    {
        if (_state == editState.Create)
        {
            var oldPos = GhostBlock.transform.position;
            Destroy(GhostBlock);
            GhostBlock = MakeGhostBlock(placingQubit);
            GhostBlock.transform.position = oldPos;
            //GhostBlock.transform.position = GhostBlockInitPosition;
        }
    }
    
    private GameObject MakeGhostBlock(GameObject prefab)
    {
        GameObject newGhost = Instantiate(prefab);
        availableFace[] faces = newGhost.GetComponentsInChildren<availableFace>();
        for (var i = 0; i < faces.Length; i++)
        {
            //faces[i].enabled = false;
            Destroy(faces[i].gameObject);
        }
        newGhost.GetComponentInChildren<Collider>().enabled = false;
        newGhost.GetComponentInChildren<MeshRenderer>().material = new Material(GhostBlockMaterial);
        //MeshRenderer[] renderer = newGhost.GetComponentsInChildren<MeshRenderer>();
        //renderer.material.color = Color.blue;//new Color(0.0f, 0.0f, 1.0f, 0.2f);

        return newGhost;
    }

    public void UpdateTransformHandle()
    {
        transformInstance.transform.position = _selected.transform.position;
    }

    public void UpdateLevel(Level new_level = null)
    {
        level = new_level;
    }

    // for until we have working GUI... and beyond?
    void KeyControls()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetState(editState.Edit);
        } 
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SetState(editState.Create);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_state)
            {
                case editState.Create:
                    SetState(editState.Rest);
                    break;
                case editState.Rest:
                case editState.Edit:
                    SetState(editState.Create);
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SetState(editState.Rest);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //placingQubit = QRailsPrefab;
            SwitchQubit("QRails");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //placingQubit = QTurnPrefab;
            SwitchQubit("QTurns");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //placingQubit = QEmiterPrefab;
            //placingQubit = QSlantPrefab;
            SwitchQubit("QSlants");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //placingQubit = QFunnelPrefab;
            SwitchQubit("QFunnel");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SwitchQubit("QEmitter");
        }
        //else if (Input.GetMouseButton(1))
        //{
        //    if(_state == editState.Edit)
        //    {
        //        _state = editState.Rest;
        //    }
        //}
        //Edit Mode keyboard controls
        if (_state == editState.Edit && _selected)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                _selected.GetComponent<Qubit>().Rotate(1);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                _selected.GetComponent<Qubit>().Rotate(-1);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                _selected.GetComponent<Qubit>().Translate(directions.North);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _selected.GetComponent<Qubit>().Translate(directions.East);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _selected.GetComponent<Qubit>().Translate(directions.South);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _selected.GetComponent<Qubit>().Translate(directions.West);
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                _selected.GetComponent<Qubit>().Translate(directions.Up);
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                _selected.GetComponent<Qubit>().Translate(directions.Down);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                DestroyImmediate(_selected);
                SetState(editState.Rest);
            }
        }
    }
}

