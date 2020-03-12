using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class Serialize
{
    private int[] Block_Type;
    private float[][] Transform;
    private float[] Rotation;
    private int[] Resource;

    public void Save_Level(int[] res)
    {
        Qubit[] Qubits_Transform = GameObject.Find("QBlocks").GetComponentsInChildren<Qubit>();
        Block_Type = new int[Qubits_Transform.Length];
        Transform = new float[Qubits_Transform.Length][];
        Rotation = new float[Qubits_Transform.Length];
        Resource = res;
        int j = 0;
        for (int i = 0; i < Qubits_Transform.Length; i++, j++)
        {
            Transform body = Qubits_Transform[i].GetComponentsInChildren<Transform>()[1];
            string name = body.name;
            if (name == "QEmitter")
            {
                Block_Type[j] = 0;
                Rotation[j] = body.rotation.eulerAngles.y;
            }
            else if (name == "QRails")
            {
                Block_Type[i] = 1;
                Rotation[i] = body.rotation.eulerAngles.y;
            }
            else if (name == "QTurn")
            {
                Block_Type[i] = 2;
                Rotation[i] = body.rotation.eulerAngles.y;
            }
            else if (name == "QSlant")
            {
                Block_Type[i] = 3;
                Rotation[i] = body.rotation.eulerAngles.y;
            }
            else if (name == "QBucket")
            {
                Block_Type[i] = 4;
                Rotation[i] = body.rotation.eulerAngles.y;
            }
            else if (name == "QBottleneck (1)")
            {
                Block_Type[i] = 5;
                Rotation[i] = body.rotation.eulerAngles.y;
            }
            else if (name == "QCube")
            {
                Block_Type[i] = 6;
                Rotation[i] = body.rotation.eulerAngles.y;
            }

            Transform[i] = new float[]
            {
                Qubits_Transform[i].transform.position.x, Qubits_Transform[i].transform.position.y,
                Qubits_Transform[i].transform.position.z
            };
        }

        BinaryFormatter bf = new BinaryFormatter();
        int Level_index = 1;
        for (;; Level_index++)
        {
            if (!File.Exists(Application.dataPath+"/levels/" + Level_index.ToString()))
            {
                break;
            }
        }

        FileStream file = File.Create(Application.dataPath +"/levels/" + Level_index.ToString());
        bf.Serialize(file, this);
        file.Close();
    }


    public int[] Load_Level(int num, Material LevelBlock)
    {
        Transform[] Old_Qubits_Transform = GameObject.Find("QBlocks").GetComponentsInChildren<Transform>();
        foreach (Transform qubit in Old_Qubits_Transform)
        {
            if (qubit != null && qubit.name != "QBlocks")
                Object.DestroyImmediate(qubit.gameObject);
        }

        Editor editor = GameObject.Find("Editor").GetComponent<Editor>();
        editor.UpdateLevel();
        GameObject TransTool = GameObject.Find("TransformTool_unbaked_(Clone)");
        if (TransTool != null)
            GameObject.Destroy(TransTool.gameObject);
        if (File.Exists(Application.dataPath + "/levels/" + num.ToString()))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/levels/" + num.ToString(), FileMode.Open);
            Serialize loadData = (Serialize) bf.Deserialize(file);
            file.Close();
            for (int i = 0; i < loadData.Transform.Length; i++)
            {
                Vector3 pos = new Vector3(loadData.Transform[i][0] / 10.0f, loadData.Transform[i][1] / 10.0f,
                    loadData.Transform[i][2] / 10.0f);
                int type = loadData.Block_Type[i];
                float rotate = loadData.Rotation[i];
                if (type == 0)
                {
                    editor.SwitchQubit("QEmitter");
                }
                else if (type == 1)
                {
                    editor.SwitchQubit("QRails");
                }
                else if (type == 2)
                {
                    editor.SwitchQubit("QTurns");
                }
                else if (type == 3)
                {
                    editor.SwitchQubit("QSlants");
                }
                else if (type == 4)
                {
                    editor.SwitchQubit("QFunnel");
                }
                else if (type == 5)
                {
                    editor.SwitchQubit("QCube");
                }
                    

                editor.PlaceQubitByIndex(pos);
            }

            Qubit[] Qubits_Transform = GameObject.Find("QBlocks").GetComponentsInChildren<Qubit>();
            for (int i = 0; i < Qubits_Transform.Length; i++)
            {
                //Qubits_Transform[i].transform.Rotate(new Vector3(0, loadData.Rotation[i], 0));
                Qubits_Transform[i].Rotate((int) loadData.Rotation[i] / 90, true);
                Qubits_Transform[i].SetEditable(true);
                MeshRenderer mesh = Qubits_Transform[i].GetComponentInChildren<MeshRenderer>();
                mesh.material = LevelBlock;
            }

            return loadData.Resource;
        }

        return null;
    }

    public int[] LoadLevel(int num, Material LevelBlock, Material LevelBlockFade)
    {
        TextAsset t = Resources.Load<TextAsset>(num.ToString());
        MemoryStream stream = new MemoryStream(t.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        Serialize loadData = (Serialize) bf.Deserialize(stream);
        stream.Close();
        Level _level = GameObject.FindObjectOfType<Level>();
        
        //you need a monobehavior to do this so why not the level obj
        _level.StartCoroutine(TransitionQubits(loadData, LevelBlock, LevelBlockFade));

        return loadData.Resource;
    }

    IEnumerator TransitionQubits(Serialize loadData, Material LevelBlock, Material LevelBlockFade)
    {
        var transitionTime = 3f;
        Editor editor = GameObject.Find("Editor").GetComponent<Editor>();
        editor.UpdateLevel();
        GameObject QBlocksObj = GameObject.Find("QBlocks");
        Transform[] oldQubitsTransform = QBlocksObj.GetComponentsInChildren<Transform>();
        MeshRenderer[] oldQubitsMR = GetQubitMeshRenderers(QBlocksObj.transform);
        foreach (var mr in oldQubitsMR)
        {
            mr.material = LevelBlockFade;
        }
        var timeSinceStart = 0f;
        if (QBlocksObj.transform.childCount > 0)
        {
            while (true)
            {
                timeSinceStart += Time.deltaTime;
                var alpha = Mathf.Lerp(1f, 0f, 2 * timeSinceStart / transitionTime);
                foreach (var mr in oldQubitsMR)
                {
                    Color oldCol = mr.material.color;
                    var newCol = new Color(oldCol.r, oldCol.g, oldCol.b, alpha);
                    mr.material.color = newCol;
                }

                if (timeSinceStart > transitionTime / 2)
                {
                    foreach (Transform qubit in oldQubitsTransform)
                    {
                        if (qubit != null && qubit.name != "QBlocks")
                        {
                            // this MUST be DestroyImmediate
                            Object.DestroyImmediate(qubit.gameObject);
                        }
                    }

                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(0.2f);
        
        GameObject TransTool = GameObject.Find("TransformTool_unbaked_(Clone)");
        if (TransTool != null)
        {
            GameObject.DestroyImmediate(TransTool.gameObject);
        }
        
        if (QBlocksObj.transform.childCount > 0)
        {
            Debug.LogError("Old Qubits were not destroyed");
        }
        else
        {
            for (int i = 0; i < loadData.Transform.Length; i++)
            {
                Vector3 pos = new Vector3(loadData.Transform[i][0] / 10.0f, loadData.Transform[i][1] / 10.0f,
                    loadData.Transform[i][2] / 10.0f);
                int type = loadData.Block_Type[i];
                float rotate = loadData.Rotation[i];
                if (type == 0)
                {
                    editor.SwitchQubit("QEmitter");
                }
                else if (type == 1)
                {
                    editor.SwitchQubit("QRails");
                }
                else if (type == 2)
                {
                    editor.SwitchQubit("QTurns");
                }
                else if (type == 3)
                {
                    editor.SwitchQubit("QSlants");
                }
                else if (type == 4)
                {
                    editor.SwitchQubit("QFunnel");
                }
                else if (type == 5)
                {
                    editor.SwitchQubit("QBottleNeck");
                }
                else if (type == 6)
                {
                    editor.SwitchQubit("QCube");
                }

                editor.PlaceQubitByIndex(pos);
            }

            Qubit[] newQubits = GameObject.Find("QBlocks").GetComponentsInChildren<Qubit>();
            for (int i = 0; i < newQubits.Length; i++)
            {
                //Qubits_Transform[i].transform.Rotate(new Vector3(0, loadData.Rotation[i], 0));
                newQubits[i].Rotate((int) loadData.Rotation[i] / 90, true);
                newQubits[i].SetEditable(true);
                MeshRenderer mesh = newQubits[i].GetComponentInChildren<MeshRenderer>();
                mesh.material = LevelBlock;
            }

            MeshRenderer[] newQubitsMR = GetQubitMeshRenderers(QBlocksObj.transform);
            foreach (var mr in newQubitsMR)
            {
                mr.material = LevelBlockFade;
            }

            //fade in here
            timeSinceStart = 0f;
            while (true)
            {
                timeSinceStart += Time.deltaTime;
                var alpha = Mathf.Lerp(0f, 1f, 2 * timeSinceStart / transitionTime);
                foreach (var mr in newQubitsMR)
                {
                    Color oldCol = mr.material.color;
                    var newCol = new Color(oldCol.r, oldCol.g, oldCol.b, alpha);
                    mr.material.color = newCol;
                }

                if (timeSinceStart > transitionTime / 2)
                {
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
            foreach (var mr in newQubitsMR)
            {
                mr.material = LevelBlock;
            }

        }

        Level _level = GameObject.FindObjectOfType<Level>();
        _level.OnFinishLoading();
        editor.UpdateLevel(_level);
    }


    private MeshRenderer[] GetQubitMeshRenderers(Transform parent)
    {
        Qubit[] qs = parent.GetComponentsInChildren<Qubit>();
        var result = new MeshRenderer[qs.Length];
        for (int i = 0; i < qs.Length; i++)
        {
            result[i] = qs[i].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>(); //phew!
            if (result[i] == null)
            {
                Debug.LogError("No Mesh Renderer found on Qubit at index: " + i);
            }
        }

        return result;
    }
}