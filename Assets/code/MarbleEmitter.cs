using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MarbleEmitter : MonoBehaviour
{
    public GameObject marblePrefab;
    public float rateOfFire = 0.3f; //in seconds
    public float lifeTime = 30f;
    public float initialVelocity = 0f;

    private GameObject marbleparent;
    // Start is called before the first frame update
    void Start()
    {
        marbleparent = GameObject.Find("MarbleParent");
    }

    // called in QEmmiter.cs
    public void StartShooting()
    {
        InvokeRepeating(nameof(Shoot), 2.0f, rateOfFire);
    }

    void Shoot()
    {
        GameObject newbie = (GameObject)Instantiate(marblePrefab);
        newbie.transform.position = gameObject.transform.position;
        newbie.GetComponent<Rigidbody>().velocity = transform.right * initialVelocity;
        if (marbleparent != null)
        {
            newbie.transform.SetParent(marbleparent.transform);
        }
        Destroy(newbie, lifeTime);
    }
}
