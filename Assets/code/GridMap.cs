using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    /// Line width
    public float gridLine = 0.02f;

    /// Line color
    public Color gridColor = Color.white;

    /// Grid lines
    private GameObject[,] m_lines;

    /// coefficient, maybe useless
    public int coefficient = 2;

    /// Number of rows
    private int m_arrRow = 10;

    /// Number of coloums
    private int m_arrCol = 10;

    /// Cross position
    private Vector3[,] m_array;
    // Start is called before the first frame update
    void Start()
    {
        this.LoadMap();
    }

    public void LoadMap()
    {

        this.m_array = new Vector3[this.m_arrRow, this.m_arrCol];

        for (int i = 0; i < this.m_arrRow; ++i)
        {
            for (int j = 0; j < this.m_arrCol; ++j)
            {
                this.m_array[i, j] = new Vector3(i-4,0,j-5);
            }
        }

        this.ShowGrid();

    }

    private void ShowGrid()
    {
        Vector3[] pos;
        int rn = this.m_arrRow / (this.coefficient - 1);
        int cn = this.m_arrCol / (this.coefficient - 1);
        if (this.m_arrRow % (this.coefficient - 1) > 0)
            ++rn;
        if (this.m_arrCol % (this.coefficient - 1) > 0)
            ++cn;
        this.m_lines = new GameObject[rn, cn];

        for (int i = 0; i < this.m_arrRow - 1;)
        {
            int lastr = i + this.coefficient - 1;
            if (lastr >= this.m_arrRow)
            {
                lastr = this.m_arrRow - 1;
            }

            for (int j = 0; j < this.m_arrCol - 1;)
            {
                int lastc = j + this.coefficient - 1;
                if (lastc >= this.m_arrCol)
                {
                    lastc = this.m_arrCol - 1;
                }

                if (lastr < this.m_arrRow - 1 && lastc < this.m_arrCol - 1)
                {
                    pos = new Vector3[this.coefficient * 4];
                    for (int k = 0; k < this.coefficient; ++k)
                    {
                        pos[0 * this.coefficient + k] = this.m_array[i, j + k];
                        pos[1 * this.coefficient + k] = this.m_array[i + k, lastc];
                        pos[2 * this.coefficient + k] = this.m_array[lastr, lastc - k];
                        pos[3 * this.coefficient + k] = this.m_array[lastr - k, j];
                    }
                    this.CreatLine(i / (this.coefficient - 1), j / (this.coefficient - 1), pos);
                }
                else
                {
                    int cr = lastr - i + 1;
                    int cl = lastc - j + 1;
                    pos = new Vector3[(cr + cl) * 2];
                    for (int k = 0; k < cr; ++k)
                    {
                        pos[cl + k] = this.m_array[i + k, lastc];
                        pos[cr + 2 * cl + k] = this.m_array[lastr - k, j];
                    }
                    for (int k = 0; k < cl; ++k)
                    {
                        pos[k] = this.m_array[i, j + k];
                        pos[cr + cl + k] = this.m_array[lastr, lastc - k];
                    }
                    this.CreatLine(i / (this.coefficient - 1), j / (this.coefficient - 1), pos);
                }

                j = lastc;
            }
            i = lastr;
        }
    }

    private void CreatLine(int row, int col, Vector3[] pos)
    {
        if (this.m_lines[row, col] != null)
        {
            GameObject.Destroy(this.m_lines[row, col]);
        }
        this.m_lines[row, col] = new GameObject();

        LineRenderer _lineRenderer = this.m_lines[row, col].AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.SetColors(this.gridColor, this.gridColor);
        _lineRenderer.SetWidth(this.gridLine, this.gridLine);
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.SetVertexCount(pos.Length);
        for (int i = 0; i < pos.Length; ++i)
        {
            _lineRenderer.SetPosition(i, pos[i]);
        }
        this.m_lines[row, col].name = "CreateLine " + row + "  " + col;
        this.m_lines[row, col].GetComponent<LineRenderer>().sortingOrder = 0;
        Transform transform = this.m_lines[row, col].transform;
        transform.SetParent(GameObject.Find("GridMap").transform);
    }
}