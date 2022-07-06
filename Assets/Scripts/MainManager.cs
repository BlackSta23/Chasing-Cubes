using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    public List<Vector3Int> positions;

    private void Start()
    {
        positions = new List<Vector3Int>();
        positions.Add(new Vector3Int(1,1,1));
        positions.Add(new Vector3Int(1,1,2));
        positions.Add(new Vector3Int(1,1,3));
        positions.Add(new Vector3Int(1,1,4));
        //positions.Add(new Vector3Int(1,1,5));
    }

    public void AddPos(Vector3Int pos)
    {
        positions.Add(pos);
    }

    public Vector3Int VectorMine(Vector3 pos)
    {
        Vector3Int vec = new Vector3Int(System.Convert.ToInt32(pos.x), 1, System.Convert.ToInt32(pos.z));
        if (!positions.Contains(vec))
            return Vector3Int.zero;             // если в массиве нет такого вектора, возвращаем нули
        else
            return vec;                         // если есть, возвращаем вектор в виде Vector3Int
    }

    public Vector3Int VectorShow(Vector3 pos)
    {
        Vector3Int vec = new Vector3Int(System.Convert.ToInt32(pos.x), 1, System.Convert.ToInt32(pos.z));
        if (positions.Contains(vec))
            return Vector3Int.zero;             // для другого кубика делаем наоборот
        else
            return vec;        
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
