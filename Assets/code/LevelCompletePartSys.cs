using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletePartSys : MonoBehaviour
{
    public float duration = 5f;
    private ParticleSystem[] _particleSystems;
    // Start is called before the first frame update
    void Start()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        if (_particleSystems.Length < 1)
        {
            Debug.LogError("No part sys thingys :(");
        }
        foreach (var ps in _particleSystems)
        {
            ps.Pause();
        }
    }

    public void Activate()
    {
        foreach (var ps in _particleSystems)
        {
            ps.Play();
            StartCoroutine(stopInALil(ps));
        }
                
    }

    IEnumerator stopInALil( ParticleSystem ps)
    {
        yield return new WaitForSeconds(duration);
        ps.Stop();
    }
}
