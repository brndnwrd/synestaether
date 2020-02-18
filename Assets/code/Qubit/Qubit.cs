using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum directions
{
    // red, w
    North,
    // green, d
    East,
    // blue, s
    South,
    // yellow, a
    West,
    Up,
    Down
}
public class Qubit : MonoBehaviour
{
    public bool editable;
    //public Vector2 orientation; //we don't need this, can just use unity transform
    public Vector3 index;
    public List<availableFace> availableFaces;

    // a general startup method for Qubits
    // You must add this MANUALLY to your startup method
    public void Initialize()
    {
        availableFaces = GetComponents<availableFace>().ToList();
    }

    public void Deselect()
    {
        for (int i = 0; i < this.transform.Find("availableFaces").childCount; i++)
        {
            this.transform.Find("availableFaces").GetChild(i).gameObject.GetComponent<availableFace>().DeselectColor();
        }
    }

    public void Rotate(int input)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform thisChild = this.transform.GetChild(i);
            if (thisChild.gameObject.tag != "no-rotate")
            {
                thisChild.transform.Rotate(new Vector3(0, 90 * input, 0));
            }
        }
    }

    public void Translate(directions direction)
    {
        switch (direction)
        {
            case directions.North:
                if (index.x > 0)
                {
                    index.x -= 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.East:
                if (index.z < FindObjectOfType<Editor>().GetComponent<Editor>().size)
                {
                    index.z += 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.South:
                if (index.x < FindObjectOfType<Editor>().GetComponent<Editor>().size)
                {
                    index.x += 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.West:
                if (index.z > 0)
                {
                    index.z -= 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.Up:
                if (index.y < FindObjectOfType<Editor>().GetComponent<Editor>().size)
                {
                    index.y += 1;
                    transform.position = index * 10.0f;
                }
                break;
            case directions.Down:
                if (index.y > 0)
                {
                    index.y -= 1;
                    transform.position = index * 10.0f;
                }
                break;
            default:
                Debug.LogWarning("Invalid Direction, no movement");
                return;
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }
}
