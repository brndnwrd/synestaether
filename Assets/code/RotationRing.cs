using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationRing : MonoBehaviour
{

    public float invRotSpeed = 20f;
    
    private Collider _collider;
    private MeshRenderer _renderer;
    private Transform _transform;
    private Camera _camera;
    private GameObject cameraParent;
    private Color baseColor;
    private Color hoverColor;

    private float destRot; // destination rotation, lerp to this number
    
    private bool _isDrag;
    private Vector3 _dragLast;

    // Start is called before the first frame update
    void Start()
    {
        
        destRot = transform.rotation.eulerAngles.y;
        cameraParent = GameObject.Find("CameraParent");
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();
        _transform = GetComponent<Transform>();
        _camera = GameObject.FindObjectOfType<Camera>();
        _isDrag = false;
        // you can instead use this to toggle visibility
        // atleast for debug I'm using colors green/red
        // _renderer.enabled = false; will make it disappear
        // ideally we get a wireframe shader material thing, there are free ones around
        baseColor = new Color(0.2f, 0.7f, 1.0f, 0.7f);
        hoverColor = new Color(0.3f, 0.8f, 1.0f, 0.9f);
        _renderer.material.color = baseColor;
    }

    public void SetRotationDestination(float newAngle)
    {
        destRot = newAngle;
    }

    // Update is called once per frame
    void Update()
    {
        float currRot = cameraParent.transform.rotation.eulerAngles.y;
        
        if (_isDrag)
        {
            destRot += (_dragLast.x - Input.mousePosition.x)/100f;
        }

        var newRotQuat = Quaternion.Slerp(
            Quaternion.Euler(0f, currRot, 0f),
            Quaternion.Euler(0f, destRot, 0f),
            1f/invRotSpeed);
        transform.parent.transform.rotation = newRotQuat;
        cameraParent.transform.rotation = newRotQuat;
    }

    private void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        if (!_isDrag)
        {
            GetComponent<Renderer>().material.color = baseColor;
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
        GetComponent<Renderer>().material.color = baseColor;
    }
}
