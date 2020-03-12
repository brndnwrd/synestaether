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
    private bool shouldUpdateCam = true;
    
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
    }

    public void SetRotationDestination(float newAngle)
    {
        if (shouldUpdateCam)
        {
            destRot = newAngle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldUpdateCam)
        {
            UpdateCam();
        }
    }

    private void UpdateCam()
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

    /*
     * This function sets whether or not players interaction
     * will effect the camera rotation. (state == true) => UI will work.
     * The value sets where the camera's "destination" will be when it
     * is allowed to update again.
     */
    public void SetUpdateCam(bool state, float value)
    {
        shouldUpdateCam = state;
        destRot = value;
    }

    private void OnMouseOver()
    {
        var newCol = GetComponent<Renderer>().material.color;
        newCol.a = 1.0f;
        GetComponent<Renderer>().material.color = newCol;
    }

    private void OnMouseExit()
    {
        if (!_isDrag)
        {
            var newCol = GetComponent<Renderer>().material.color;
            newCol.a = 0.6f;
            GetComponent<Renderer>().material.color = newCol;
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
        var newCol = GetComponent<Renderer>().material.color;
        newCol.a = 0.6f;
        GetComponent<Renderer>().material.color = newCol;
    }
}
