using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public enum editState
{
    Create,
    Rest,
    Edit,
}

public class Editor : MonoBehaviour
{
    /*[HideInInspector] commented this for debugging*/
    public editState _state;

    [HideInInspector] public GameObject _selected;

    //public GameObject CubePrefab;
    public GameObject[,,] _grid;
    private Camera _camera;
    [HideInInspector] public int size; // number of rows/columns
    public GameObject placingQubit; // the Qubit from the menu were about to place
    public GameObject transformPrefab;
    private GameObject transformInstance;

    public GameObject QFloorPrefab;
    public GameObject QRailsPrefab;
    public GameObject QEmiterPrefab;
    public GameObject QTurnPrefab;
    public GameObject QSlantPrefab;
    public GameObject QFunnelPrefab;
    public GameObject QCubePrefab;

    public GameObject QBottleNeckPrefab;

    [HideInInspector] public GameObject GhostBlock;
    public Material GhostBlockMaterial;
    private float currentRotation = 0f; //store the angles

    public Level level;
    private List<String> unplaceables;
    private int unplaceableIndex = 0;

    void Start()
    {
        // you can add names here, but make sure they work in 
        // Switch Qubit and Serilaize...
        unplaceables = new List<String> {"QCube", "QEmitter", "QFunnel", "QBucket", "QBottleNeck"};
        size = 15;
        _grid = new GameObject[size, size, size];
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
                    level.SetResource(0, -1);
                    GameObject.Find("Button_Rail").GetComponent<CreateButton>().ChangeText(level.GetResource(0));
                }
            }
            else if (placingQubit.name == "QTurn-v3")
            {
                if (level.GetResource(1) == 0)
                    return;
                else
                {
                    level.SetResource(1, -1);
                    GameObject.Find("Button_Turn").GetComponent<CreateButton>().ChangeText(level.GetResource(1));
                }
            }
            else if (placingQubit.name == "QSlant-v5")
            {
                if (level.GetResource(2) == 0)
                    return;
                else
                {
                    level.SetResource(2, -1);
                    GameObject.Find("Button_Slant").GetComponent<CreateButton>().ChangeText(level.GetResource(2));
                }
            }
            else if (placingQubit.name == "QBottleneck")
            {
                if (level.GetResource(3) == 0)
                    return;
                else
                {
                    level.SetResource(3, -1);
                    GameObject.Find("Button_BottleNeck").GetComponent<CreateButton>().ChangeText(level.GetResource(3));
                }
            }
        }

        Vector3 newPos = indexToPosition(newIndex);
        Transform par = GameObject.Find("QBlocks").GetComponent<Transform>();
        var newbie = Instantiate(placingQubit, par);
        var newQ = newbie.GetComponent<Qubit>();
        newQ.index = newIndex;
        newbie.transform.position = newPos;
        if (GhostBlock != null)
        {
            newQ.SetRotation(currentRotation);
        }

        _grid[(int) newIndex.x, (int) newIndex.y, (int) newIndex.z] = newbie;
        newQ.OnPlace();
    }

    public void SelectQubit(GameObject go)
    {
        if (go.GetComponent<Qubit>().editable == true)
            return;
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
            case "QBottleNeck":
                placingQubit = QBottleNeckPrefab;
                break;
            case "QFunnel":
                placingQubit = QFunnelPrefab;
                break;
            case "QCube":
                placingQubit = QCubePrefab;
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
        //Debug.Log("state now equal to " + newState);
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
        else // (newState != editState.Create)
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
            GhostBlock.GetComponent<Qubit>().SetRotation(currentRotation);
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
        //newGhost.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
        newGhost.GetComponent<Qubit>().SetRotation(currentRotation);
        return newGhost;
    }

    /*
     *  This function allows users to rotate
     *  before placing a block
     *  and to rotate a selected block
     *  with keys.
     *  Makes block rotation persistent 
     *  Makes block rotation persistent
     *  between placements
     */
    public void RotateKeys(bool CW) // or left
    {
        int angle = 90;
        if (CW)
        {
            angle *= -1;
        }

        // ok. Ik this is weird, but hear me out:
        currentRotation = Quaternion.Euler(0f, currentRotation + angle, 0f).eulerAngles.y;

        GameObject toRotate = null;
        if (GhostBlock != null)
        {
            toRotate = GhostBlock;
        }
        else if (_selected != null)
        {
            toRotate = _selected;
        }
        else
        {
            // nothing to rotate
            return;
        }

        toRotate.GetComponent<Qubit>().SetRotation(currentRotation);
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
            RotateKeys(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateKeys(false);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SetState(editState.Create);
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
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
            if (level != null)
            {
                string name = level.GetHotkey(1);
                if (name != null)
                {
                    SwitchQubit(name);
                    if (_state != editState.Create)
                        SetState(editState.Create);
                }
                else
                    return;
            }
            else
            {
                SwitchQubit("QRails");
                if (_state != editState.Create)
                    SetState(editState.Create);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //placingQubit = QTurnPrefab;
            if (level != null)
            {
                string name = level.GetHotkey(2);
                if (name != null)
                {
                    SwitchQubit(name);
                    if (_state != editState.Create)
                        SetState(editState.Create);
                }
                else
                    return;
            }
            else
            {
                SwitchQubit("QTurns");
                if (_state != editState.Create)
                    SetState(editState.Create);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //placingQubit = QEmiterPrefab;
            //placingQubit = QSlantPrefab;
            if (level != null)
            {
                string name = level.GetHotkey(3);
                if (name != null)
                {
                    SwitchQubit(name);
                    if (_state != editState.Create)
                        SetState(editState.Create);
                }
                else
                    return;
            }
            else
            {
                SwitchQubit("QSlants");
                if (_state != editState.Create)
                    SetState(editState.Create);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            unplaceableIndex = (unplaceableIndex + 1) % unplaceables.Count;
            SwitchQubit(unplaceables[unplaceableIndex]);
        }

        //Edit Mode keyboard controls
        if (_state == editState.Edit && _selected)
        {
            //setResource first, then ChangeText.

            /*
            if (Input.GetKeyDown(KeyCode.X))
            {
                int resourceType;
                string name = _selected.name;
                if (level != null)
                {
                    if (name == "QRails-v4(Clone)")
                    {
                        resourceType = 0;
                        GameObject.Find("Button_Rail").GetComponent<CreateButton>().ChangeText(level.GetResource(0));
                    }
                    else if (name == "QTurn-v3(Clone)")
                    {
                        resourceType = 1;
                        GameObject.Find("Button_Turn").GetComponent<CreateButton>().ChangeText(level.GetResource(1));
                    }
                    else if (name == "QSlant-v4(Clone)")
                    {
                        resourceType = 2;
                        GameObject.Find("Button_Slant").GetComponent<CreateButton>().ChangeText(level.GetResource(2));
                    }
                    else
                    {
                        //its unplaceable so no resource thing
                        return;
                    }

                    level.SetResource(resourceType, 1);
                }


                DestroyImmediate(_selected);
                SetState(editState.Rest);
            }
            */

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (level != null)
                {
                    string name = _selected.name;
                    if (name == "QRails-v4(Clone)")
                    {
                        level.SetResource(0, 1);
                        GameObject.Find("Button_Rail").GetComponent<CreateButton>().ChangeText(level.GetResource(0));
                    }
                    else if (name == "QTurn-v3(Clone)")
                    {
                        level.SetResource(1, 1);
                        GameObject.Find("Button_Turn").GetComponent<CreateButton>().ChangeText(level.GetResource(1));
                    }
                    else if (name == "QSlant-v5(Clone)")
                    {
                        level.SetResource(2, 1);
                        GameObject.Find("Button_Slant").GetComponent<CreateButton>().ChangeText(level.GetResource(2));
                    }
                    else if (name == "QBottleneck(Clone)")
                    {
                        level.SetResource(3, 1);
                        GameObject.Find("Button_BottleNeck").GetComponent<CreateButton>().ChangeText(level.GetResource(3));
                    }
                }
                DestroyImmediate(_selected);
                SetState(editState.Rest);
            }

            /* This is now mouse stuff
            else if (Input.GetKeyDown(KeyCode.J))
            {
                //This approach is deprecated use RotateKeys
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
            */
        }
    }
}