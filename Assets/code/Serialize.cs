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

    public void Save_Level()
    {
        Qubit[] Qubits_Transform = GameObject.Find("QBlocks").GetComponentsInChildren<Qubit>();
        Block_Type = new int[Qubits_Transform.Length];
        Transform = new float[Qubits_Transform.Length][];
        Rotation = new float[Qubits_Transform.Length];
        Resource = new int[] { 15, 4, 2 };
        for (int i = 0; i < Qubits_Transform.Length; i++)
        {
            Transform body = Qubits_Transform[i].GetComponentsInChildren<Transform>()[1];
            string name = body.name;
            if (name == "emiterCube")
            {
                Block_Type[i] = 0;
                Rotation[i] = body.rotation.eulerAngles.y + 90;
                if (Rotation[i] == 360)
                    Rotation[i] = 0;
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
        for(int i = 1; i < Old_Qubits_Transform.Length; i++)
        {
            if(Old_Qubits_Transform[i] != null)
                GameObject.DestroyImmediate(Old_Qubits_Transform[i].gameObject);
        }
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
                Editor editor = GameObject.Find("Editor").GetComponent<Editor>();
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
                editor.PlaceQubitByIndex(pos);
            }
            Qubit[] Qubits_Transform = GameObject.Find("QBlocks").GetComponentsInChildren<Qubit>();
            for (int i = 0; i < Qubits_Transform.Length; i++)
            {
                if(loadData.Rotation[i] == 90)
                {
                    Qubits_Transform[i].Rotate(1);
                }
                else if(loadData.Rotation[i] == 270)
                {
                    Qubits_Transform[i].Rotate(3);
                }
                else if (loadData.Rotation[i] == 180)
                {
                    Qubits_Transform[i].Rotate(2);
                }
            }
            return loadData.Resource;
        }
        return null;
    }
}
