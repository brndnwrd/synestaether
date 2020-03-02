    using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
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
        for (int i = 0; i < Qubits_Transform.Length; i++,j++)
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
            Transform[i] = new float[] { Qubits_Transform[i].transform.position.x, Qubits_Transform[i].transform.position.y, Qubits_Transform[i].transform.position.z };
        }
        BinaryFormatter bf = new BinaryFormatter();
        int Level_index = 1;
        for (; ; Level_index++)
        {
            if (!File.Exists("Assets/levels/" + Level_index.ToString()))
            {
                break;
            }
        }
        FileStream file = File.Create("Assets/levels/" + Level_index.ToString());
        bf.Serialize(file, this);
        file.Close();
    }

    public int[] Load_Level(int num)
    {
        Transform[] Old_Qubits_Transform = GameObject.Find("QBlocks").GetComponentsInChildren<Transform>();
        foreach(Transform qubit in Old_Qubits_Transform)
        {
            if(qubit != null && qubit.name != "QBlocks")
                Object.DestroyImmediate(qubit.gameObject);
        }
        Editor editor = GameObject.Find("Editor").GetComponent<Editor>();
        editor.UpdateLevel();
        GameObject TransTool = GameObject.Find("TransformTool_unbaked_(Clone)");
        if(TransTool != null)
            GameObject.Destroy(TransTool.gameObject);
        if (File.Exists("Assets/levels/" + num.ToString()))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open("Assets/levels/" + num.ToString(), FileMode.Open);
            Serialize loadData = (Serialize)bf.Deserialize(file);
            file.Close();
            for (int i = 0; i < loadData.Transform.Length; i++)
            {
                Vector3 pos = new Vector3(loadData.Transform[i][0] / 10.0f, loadData.Transform[i][1] / 10.0f, loadData.Transform[i][2] / 10.0f);
                int type = loadData.Block_Type[i];
                float rotate = loadData.Rotation[i];
                if(type == 0)
                {
                    editor.SwitchQubit("QEmitter");
                }
                else if(type == 1)
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
                editor.PlaceQubitByIndex(pos);
            }
            Qubit[] Qubits_Transform = GameObject.Find("QBlocks").GetComponentsInChildren<Qubit>();
            for (int i = 0; i < Qubits_Transform.Length; i++)
            {
                //Qubits_Transform[i].transform.Rotate(new Vector3(0, loadData.Rotation[i], 0));
                Qubits_Transform[i].Rotate((int)loadData.Rotation[i] / 90, true);
                Qubits_Transform[i].SetEditable(true);
            }
            return loadData.Resource;
        }
        return null;
    }
}
