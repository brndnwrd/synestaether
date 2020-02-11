using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationRing : MonoBehaviour
{

    private Collider _collider;
    private MeshRenderer _renderer;
    private Transform _transform;
    private Camera _camera;

    private bool _isDrag;
    private Vector3 _dragLast;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _transform = GetComponent<Transform>();
        _camera = GameObject.FindObjectOfType<Camera>();
        _isDrag = false;
        // you can instead use this to toggle visibility
        // atleast for debug I'm using colors green/red
        // _renderer.enabled = false; will make it disappear
        // ideally we get a wireframe shader material thing, there are free ones around
        _renderer.material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDrag)
        {
            float rotAngle = (_dragLast.x - Input.mousePosition.x);
            _camera.GetComponent<Transform>().RotateAround(new Vector3(70, 0, 70), new Vector3(0, 1, 0), -rotAngle);
            _dragLast = Input.mousePosition;
        }
    }

    private void OnMouseDown()
    {
        _isDrag = true;
        _dragLast = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        _isDrag = false;
    }
}
