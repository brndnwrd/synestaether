using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Qubit : MonoBehaviour
{
    public bool editable;
    //public Vector2 orientation; //we don't need this, can just use unity transform
    public Vector3 index;
    public List<availableFace> availableFaces;

    // a general startup method for Qubits
    // You must add this MANUALLY to your startup method
    public void Initialize()
    {
        availableFaces = GetComponents<availableFace>().ToList();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }
}
