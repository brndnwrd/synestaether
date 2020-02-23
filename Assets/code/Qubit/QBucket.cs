using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QBucket : Qubit
{
    public float allowedSaveUp = 0.2f; 
    public float necessaryMarbleScore;
    public float decayPerSecond;
    public float animationSpeed = 0.3f;
    public Material doneMaterial;
    public Material stillWorkingMaterial;
    
    
    [HideInInspector]
    public bool isFull = false;
    private Level _level;
    private MeshRenderer _totalRenderer;
    private GameObject progressPivot;
    private float marbleScore = 0f;
    private LevelCompletePartSys _partSys;
    
    public override void Start()
    {
        Initialize();
        _level = FindObjectOfType<Level>();
        progressPivot = transform.Find("ProgressParent").Find("ProgressPivot").gameObject;
        _totalRenderer = transform.Find("ProgressParent").Find("Total").gameObject.GetComponent<MeshRenderer>();
        _partSys = transform.Find("LevelCompletePartSys").GetComponent<LevelCompletePartSys>();
    }

    // called by the MarbleDetector.cs (a child of this.gameObject)
    public void GetMarble()
    {
        marbleScore += 1f;
    }
    
    public override void Update()
    {
        marbleScore -= decayPerSecond * Time.deltaTime;
        marbleScore = Mathf.Max(0f, marbleScore);
        marbleScore = Mathf.Min(marbleScore, necessaryMarbleScore*(1f+allowedSaveUp)); //only allow 20% save up
        var scaleFactor = 1f + Mathf.Lerp(0f, 12f, marbleScore / necessaryMarbleScore);
        var oldScale = progressPivot.transform.localScale;
        var newScale = new Vector3(oldScale.x, scaleFactor, oldScale.z);
        progressPivot.transform.localScale = Vector3.Lerp(oldScale, newScale, animationSpeed);
        isFull = marbleScore >  necessaryMarbleScore;
        if (isFull)
        {
            //_partSys.Activate();
            _totalRenderer.material = doneMaterial;
        }
        else
        {
            _totalRenderer.material = stillWorkingMaterial;
        }
    }

    // que the fun level complete visuals!
    public void OnLevelComplete()
    {
        _partSys.Activate();
    }

}
