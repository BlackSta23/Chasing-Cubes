using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    public List<Vector3Int> positions;

    private void Start()
    {
        positions = new List<Vector3Int>
        {
            new Vector3Int(1, 1, 1),
            new Vector3Int(1, 1, 2),
            new Vector3Int(1, 1, 3),
            new Vector3Int(1, 1, 4)
        };
        //positions.Add(new Vector3Int(1,1,5));
    }

    public void AddPos(Vector3Int pos)
    {
        positions.Add(pos);
    }
    public bool IsContainsVector(Vector3Int vec)
    {
        if (positions.Contains(vec))
            return true;
        else
            return false;
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
