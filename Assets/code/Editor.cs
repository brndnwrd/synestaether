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
    public GameObject QFloorPrefab;
    private Qubit[,,] _grid;
    private int size; // number of rows/columns
    
    void Start()
    {
        size = 15;
        _grid = new Qubit[size,size,size];
        makeFloor();
    }

    void Update()
    {
        
    }
    
    private void makeFloor() 
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                var newbie = Instantiate(QFloorPrefab, transform);
                newbie.transform.position = new Vector3(x*10,0, z*10);
                // organizing these as close to Unity space
                // as possible, x/z is ground, y is up
                _grid[x, 0, z] = newbie.GetComponent<Qubit>();
            }
        }
    }

    private void MoveQubit()
    {
        throw new NotImplementedException();
    }

    private void PlaceQubit()
    {
        throw new NotImplementedException();
    }
    
    /*
     * Make sure every qubit is where it thinks it is.
     * Check Qubit.index == Editor's index, else throw
     */
    private void Verify()
    {
        throw new NotImplementedException();
    }
}

