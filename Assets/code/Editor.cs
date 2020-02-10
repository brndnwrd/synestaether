using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum editState
{
    Create,
    Rest,
    Edit,
}
public class Editor : MonoBehaviour
{
    public editState _state;
    public GameObject CubePrefab;
    private Qubit[,,] _grid;
    private int size; // number of rows/columns
    public GameObject placingQubit; // the Qubit from the menu were about to place
    private GameObject selectedQubit; // the QUbit in the WORLD we are about to edit
    
    public GameObject QFloorPrefab;
    public GameObject QRailsPrefab;
    public GameObject QEmiterPrefab;
    public GameObject QTurnPrefab;
    
    void Start()
    {
        size = 15;
        _grid = new Qubit[size,size,size];
        makeFloor();
        _state = editState.Rest;
    }

    void Update()
    {
       debugKeyControls(); 
    }

    private Vector3 indexToPosition(Vector3 index)
    {
        return index * 10;
    } 
    private void makeFloor() 
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
                _grid[x, 0, z] = q;
                q.index = indx;
            }
        }
    }

    private void MoveQubit()
    {
        throw new NotImplementedException();
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
    void debugKeyControls()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _state = editState.Edit;
        } 
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _state = editState.Create;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            _state = editState.Rest;
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
        else if (Input.GetMouseButton(1))
        {
            if(_state == editState.Edit)
            {
                _state = editState.Rest;
            }
        }
    }
}

