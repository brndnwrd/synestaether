using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    public float minVelocity = 1f;
    public float fadeOutSeconds = 2f;
    private Rigidbody _rigidBody;
    [HideInInspector]
    public bool isDead = false;

    private Material _material;
    private Collider _collider;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _material = GetComponent<MeshRenderer>().material;
        _collider = GetComponent<Collider>();
        if (minVelocity <= 0f)
        {
            Debug.LogWarning("minVelocity of marble must be GREATER than 0!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && _rigidBody.velocity.magnitude <= minVelocity)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        _collider.isTrigger = true;
        _rigidBody.useGravity = false;
        StartCoroutine(FadeOutAndDie());
    }

    private IEnumerator FadeOutAndDie()
    {
        float timeToPass = fadeOutSeconds;

        while (timeToPass > 0f)
        {
            timeToPass -= Time.deltaTime;
            SetTransparency(Mathf.Lerp(0.0f, 1.0f, timeToPass / fadeOutSeconds));
            yield return null;
        }
       
        Destroy(gameObject);
        
    }

    private void SetTransparency(float newTrans)
    {
        var newCol = _material.color;
        newCol.a = newTrans;
        _material.color = newCol;
    }
}
