﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    private int[] Resource;
    private int Number; //index of levels
    private QBucket[] _buckets;
    private bool currLevelFinished = false;
    private float camOldSize;
    private string[] hotkeys;
    void Start()
    {
        UpdateObjective();
    }
    
    public void Initialize(int num)
    {
        Number = num;
        Serialize serialize = new Serialize();
        Resource = serialize.Load_Level(num);
        UpdateObjective();
        hotkeys = new string[4];
        GameObject.Find("Editor").GetComponent<Editor>().UpdateLevel(this);
        hotkeys[GameObject.Find("Button_Rail").GetComponent<CreateButton>().ChangeLocation(Resource, 0)] = "QRails";
        hotkeys[GameObject.Find("Button_Turn").GetComponent<CreateButton>().ChangeLocation(Resource, 1)] = "QTurns";
        hotkeys[GameObject.Find("Button_Slant").GetComponent<CreateButton>().ChangeLocation(Resource, 2)] = "QSlants";
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelIsComplete() && !currLevelFinished)
        {
            CompleteLevel();
        }    
    }

    private bool LevelIsComplete()
    {
        int numToFill = _buckets.Length;
        int numFilled = 0;
        for (int i = 0; i < _buckets.Length; i++)
        {
            if (_buckets[i].isFull)
                numFilled++;
        }
        
        // levels with no buckets will never end, this is intentional
        return (numFilled > 0) && (numFilled >= numToFill);

    }

    private void UpdateObjective()
    {
        _buckets = FindObjectsOfType<QBucket>();
    }

    public void CompleteLevel()
    {
        currLevelFinished = true;
        
        //var cg = FindObjectOfType<CanvasGroup>();
        var cg = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
        var cam = FindObjectOfType<Camera>();
        
        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i].OnLevelComplete();
        }

        StartCoroutine(FadeUI(cg, 1f, 0f, 2f));
        StartCoroutine(MoveCamera(cam));

        /* here we do all the fun stuff that
           happens when you beat a level,
           eventually call NextLevel
        */
    }

    private void NextLevel()
    {
        // load next level or somethin
    }
    
    public int GetResource(int type)
    {
        return Resource[type];
    }

    public void SetResource(int type)
    {
        Resource[type] -= 1;
    }

    public string GetHotkey(int num)
    {
        return hotkeys[num];
    }

    IEnumerator FadeUI(CanvasGroup cg, float startAlpha, float endAlpha, float fadeTime)
    {

        float timeSinceStart = 0;
        while (true)
        {
            timeSinceStart += Time.deltaTime;
            var completion =  timeSinceStart / fadeTime;
            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, completion );
            cg.alpha = newAlpha;
            if (timeSinceStart >= fadeTime)
            {
                break;
            } 
            
            yield return new WaitForEndOfFrame();
            
        }
    }

    IEnumerator MoveCamera(Camera cam)
    {
        float moveTime = 5f; 
        
        camOldSize = cam.orthographicSize;
        float newZoom = 70f;

        FindObjectOfType<RotationRing>().SetUpdateCam(false, 0f);


        float timeSinceStart = 0f;
        var startRot = Quaternion.Euler(0f, cam.transform.parent.rotation.eulerAngles.y, 0f);
        var destRot = Quaternion.Euler(0f, 0f, 0f);
        
        while ( true )
        {
            timeSinceStart += Time.deltaTime;
            var completion = timeSinceStart / moveTime;
            cam.orthographicSize = Mathf.SmoothStep(camOldSize, newZoom, completion);
            
            var rotAmt = Mathf.SmoothStep(0.0f, 1.0f, completion);
            cam.transform.parent.rotation =
                Quaternion.Slerp(startRot, destRot, rotAmt );
            
            if (timeSinceStart > moveTime)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        
        FindObjectOfType<RotationRing>().SetUpdateCam(true, 0f);
        
    }
}
