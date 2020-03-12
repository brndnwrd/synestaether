using System.Collections;
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
    private Editor _editor;

    public float closeZoom = 56f;
    public float camZoomTime = 5f;
    public Material LevelBlock;
    public Material LevelBlockFade;
    
    void Start()
    {
        UpdateObjective();
        _editor = GameObject.Find("Editor").GetComponent<Editor>();
    }
    
    public void Initialize(int num)
    {
        Number = num;
        Serialize serialize = new Serialize();
        //Resource = serialize.Load_Level(num, LevelBlock);
        Resource = serialize.LoadLevel(num, LevelBlock, LevelBlockFade);
        hotkeys = new string[5];
        //GameObject.Find("Editor").GetComponent<Editor>().UpdateLevel(this);
        hotkeys[GameObject.Find("Button_Rail").GetComponent<CreateButton>().ChangeLocation(Resource, 0)] = "QRails";
        hotkeys[GameObject.Find("Button_Turn").GetComponent<CreateButton>().ChangeLocation(Resource, 1)] = "QTurns";
        hotkeys[GameObject.Find("Button_Slant").GetComponent<CreateButton>().ChangeLocation(Resource, 2)] = "QSlants";
        hotkeys[GameObject.Find("Button_BottleNeck").GetComponent<CreateButton>().ChangeLocation(Resource, 3)] = "QBottleNeck";
        KillMarbles();
        //GameObject.Find("Editor").GetComponent<Editor>().SwitchQubit();
    }

    public void OnFinishLoading()
    {
        // called AFTER the level is totally finished loading
        UpdateObjective();
        KillMarbles();
        string[] qubits = { "QRails", "QTurns", "QSlants", "QBottleNeck"};
        for (var i = 0; i < Resource.Length; i++)
        {
            if (Resource[i] != 0)
            {
                Debug.Log(qubits[i]);
                _editor.SwitchQubit(qubits[i]);
                break;
            }
        }
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
        currLevelFinished = false;
        _buckets = FindObjectsOfType<QBucket>();
        //Debug.Log("objective updated, this many buckets: " + _buckets.Length);
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
        StartCoroutine(MoveCamera(cam, closeZoom, camZoomTime));
        var hangTime = camZoomTime + 3f; // this is the amount of time the UI is away b4 it comes back
        StartCoroutine(MoveCamBack(cam, hangTime, camOldSize, camZoomTime));
        StartCoroutine(FadeUIBack(cg,  hangTime, 0f, 1f, 2f));
        Invoke(nameof(BringUpLevelMenu), hangTime+camZoomTime+1f);
    }

    
    public int GetResource(int type)
    {
        return Resource[type];
    }

    public void SetResource(int type, int num)
    {
        Resource[type] += num;
    }

    public string GetHotkey(int num)
    {
        return hotkeys[num];
    }

    private void KillMarbles()
    {
        Marble[] marbles = FindObjectsOfType<Marble>();
        foreach (Marble m in marbles)
        {
            m.Die();
        }
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

    IEnumerator MoveCamera(Camera cam, float newZoom, float moveTime)
    {
        
        camOldSize = cam.orthographicSize;
        //float newZoom = 70f;

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

    IEnumerator MoveCamBack(Camera cam, float delayTime, float newZoom, float moveTime )
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(MoveCamera(cam, newZoom, moveTime));
    }

    IEnumerator FadeUIBack(CanvasGroup cg, float delayTime, float startAlpha, float endAlpha, float fadeTime)
    {
        yield return new WaitForSeconds(delayTime);

        StartCoroutine(FadeUI(cg, startAlpha, endAlpha, fadeTime));
    }

    private void BringUpLevelMenu()
    {
        var levelMenu = FindObjectOfType<LevelMenu>();
        levelMenu.OpenMenu();
    }

    public int GetIndex()
    {
        return Number;
    }
}
