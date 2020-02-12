using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public enum editState
{
    Create,
    Rest,
    Edit,
}
public class Editor : MonoBehaviour
{
    public editState _state;
    public GameObject _selected;
    public GameObject CubePrefab;
    private Qubit[,,] _grid;
    private int size; // number of rows/columns
    public GameObject placingQubit; // the Qubit from the menu were about to place
    
    public GameObject QFloorPrefab;
    public GameObject QRailsPrefab;
    public GameObject QEmiterPrefab;
    public GameObject QTurnPrefab;
    public GameObject QSlantPrefab;
    
    void Start()
    {
        size = 15;
        _grid = new Qubit[size,size,size];
        MakeFloor();
        _state = editState.Rest;
    }

    void Update()
    {
       KeyControls(); 
    }

    private Vector3 indexToPosition(Vector3 index)
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
                Qubit q = newbie.GetComponent<Qubit>();
                _grid[(int)indx.x, (int)indx.y, (int)indx.z] = q;
                q.index = indx;
            }
        }
    }

    private void MoveQubit()
    {
        throw new NotImplementedException();
    }

    private void RotateQubit()
    {

    }

    //ATM this function gets called by availableFace.cs
    public void PlaceQubit(Vector3 position)
    {
        if (placingQubit.name == "QRails")
        {
            position.y -= 5f;
            //this is a quick fix, need to change the prefab
            // cant figure it out atm
        }
        Transform parent = GameObject.Find("QBlocks").GetComponent<Transform>();
        var newCube = Instantiate(placingQubit, parent);
        newCube.transform.position = position;
    }

    public void PlaceQubitByIndex(Vector3 newIndex)
    {
        Vector3 newPos = indexToPosition(newIndex);
        // I think this has something to do with QFloor being
        // centered in the center of its floor piece. It should
        // be centered somehow above it. 0.5 above it I guess...
        //newPos.y += 5f; 
        Transform par = GameObject.Find("QBlocks").GetComponent<Transform>();
        var newbie = Instantiate(placingQubit, par);
        newbie.GetComponent<Qubit>().index = newIndex;
        newbie.transform.position = newPos;
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

    public void SetState(editState state)
    {
        _state = state;
    }

    // for until we have working GUI
    void KeyControls()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _state = editState.Edit;
        } 
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _state = editState.Create;
            if (_selected) { _selected.transform.Find("availableFaces").transform.GetChild(0).gameObject.GetComponent<availableFace>().Deselect();}
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _state = editState.Rest;
            if (_selected) { _selected.transform.Find("availableFaces").transform.GetChild(0).gameObject.GetComponent<availableFace>().Deselect(); }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            placingQubit = QRailsPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            placingQubit = QTurnPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            placingQubit = QEmiterPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            placingQubit = QSlantPrefab;
        }
        else if (Input.GetMouseButton(1))
        {
            if(_state == editState.Edit)
            {
                _state = editState.Rest;
            }
        }
        //Edit Mode keyboard controls
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if (_state == editState.Edit)
            {
                if (_selected)
                {
                    for (int i = 0; i < _selected.transform.childCount; i++)
                    {
                        Transform thisChild = _selected.transform.GetChild(i);
                        if (thisChild.gameObject.tag != "no-rotate")
                        {
                            thisChild.transform.Rotate(new Vector3(0, 90, 0));
                        }
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            if (_state == editState.Edit)
            {
                if (_selected)
                {
                    for (int i = 0; i < _selected.transform.childCount; i++)
                    {
                        Transform thisChild = _selected.transform.GetChild(i);
                        if (thisChild.gameObject.tag != "no-rotate")
                        {
                            thisChild.transform.Rotate(new Vector3(0, -90, 0));
                        }
                    }
                }
            }
        }
    }
}

