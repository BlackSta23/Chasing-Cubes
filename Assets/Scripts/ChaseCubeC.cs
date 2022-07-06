
using System.Collections;
using UnityEngine;

public class ChaseCubeC : CubeC
{
    [SerializeField] Transform platform1, platform2;
    [SerializeField] int maxCount = 5; // ������������ ���������� �������� �� ���� ���
    [SerializeField] float cubeSpeed = 1.7f, cubeRotationSpeed = 120f, platformSpeed = 21f;

    // platforms
    private Vector3 pl1dir, pl2dir; //platformDown, platformUp
    private bool platformDirection; // false = 1 up, 2 down; true = 1 down, 2 up
    void Start()
    {
        pl1dir = platform1.transform.position;
        pl2dir = platform2.transform.position;
        directionToMove = Vector3Int.FloorToInt(transform.position);
        rotationToMake = transform.rotation;
    }

    Vector3Int RandomVector()
    {
        int r = Random.Range(0, 3);
        Vector3Int vec = Vector3Int.FloorToInt(transform.position + directions[r]);
        if (MainManager.Instance.IsContainsVector(vec)) // ���� ��� ���� ���, ��������� ��� ���
        {
            return RandomVector();                      
        }
        MainManager.Instance.AddPos(vec); // ��������� � ������ - �� ���� ����������� ����� ������
        rotationToMake = SetRotation(r);
        return vec; 
    }
    IEnumerator Move()
    {
        while(isMoving)
        {
            if (transform.position != directionToMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, directionToMove, cubeSpeed * Time.deltaTime);
                platform1.position = Vector3.MoveTowards(platform1.position, pl1dir, platformSpeed * Time.deltaTime);
                platform2.position = Vector3.MoveTowards(platform2.position, pl2dir, platformSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMake, cubeRotationSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                transform.position = directionToMove;
                platform1.position = pl1dir;
                platform2.position = pl2dir;
                transform.rotation = Quaternion.Euler(0,0,0);
                isMoving = false;
            }
            
        }
    }

    public void ShowMoves()
    {
        directionToMove = RandomVector();
        if (!platformDirection)
        {
            platform1.position = directionToMove + platformDown;
            pl1dir = directionToMove + platformUp;
            pl2dir += platformDown;
            platformDirection = true;
        }
        else
        {
            platform2.position = directionToMove + platformDown;
            pl2dir = directionToMove + platformUp;
            pl1dir += platformDown;
            platformDirection = false;
        }
        isMoving = true;
        StartCoroutine(Move());
    }
}
