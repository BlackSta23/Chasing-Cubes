
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCubeC : MonoBehaviour
{
    [SerializeField] Transform platform1, platform2;
    [SerializeField] int maxCount = 5; // максимальное количество движений за один ход
    [SerializeField] float cubeSpeed = 1.7f, cubeRotationSpeed = 120f, platformSpeed = 21f;

    // cube
    private Vector3Int[] directions = new Vector3Int[4];
    private Vector3Int directionToMove;
    private Quaternion rotationToMake;
    // platforms
    private Vector3 platformDown, platformUp, pl1dir, pl2dir;
    private bool platformDirection; // false = 1 up, 2 down; true = 1 down, 2 up
    private bool isMoving; // do we move the ShowCube now
    void Start()
    {
        directions[0] = Vector3Int.forward;
        directions[1] = Vector3Int.left;
        directions[2] = Vector3Int.right;
        directions[3] = Vector3Int.back;
        pl1dir = platform1.transform.position;
        pl2dir = platform2.transform.position;
        directionToMove = new Vector3Int(1, 1, 3);
        rotationToMake = transform.rotation;
        platformDown = new Vector3(0, -16, 0);
        platformUp = new Vector3(0, -6, 0);
        //StartCoroutine(MovingPlatforms());
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
            
    //        ShowMoves();
    //    }
    //}

    Vector3Int RandomVector()
    {
        int r = UnityEngine.Random.Range(0, 3);
        Vector3Int vec = MainManager.Instance.VectorShow(transform.position + directions[r]);
        if (vec == Vector3Int.zero) 
        {
            return RandomVector();                      // если нули, повторяем ещё раз
        }
        MainManager.Instance.AddPos(vec);
        SetRotation(r);
        return vec;
    }

    void SetRotation(int r)
    {
        switch (r)
        {
            case 0:
                rotationToMake = Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                break;
            case 1:
                rotationToMake = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90);
                break;
            case 2:
                rotationToMake = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90);
                break;
            case 3:
                rotationToMake = Quaternion.Euler(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                break;
        }
        //Debug.Log(r);
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
