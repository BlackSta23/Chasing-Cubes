using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerC : CubeC
{
    [SerializeField] GameObject showCube, platformPrefab, gamePlayUi, gameOverUi, menuUi;
    [SerializeField] float cubeSpeed = 1.7f, cubeRotationSpeed = 150f, platformSpeed = 21f;
    [SerializeField] Transform platform, platformOld;
    [SerializeField] Text scoreText;

    // platforms
    private Vector3 platformOldDown; //platformDown, platformUp, 
    private Vector3 pl, plOld;
    private Dictionary<Vector3Int, GameObject> platforms = new Dictionary<Vector3Int, GameObject>();

    private bool moveBackwards; // do we move the MainCube now, direction  //private bool isMoving,
    private ChaseCubeC chaseCube;
    private int score;


    void Start()
    {
        directionToMove = Vector3Int.FloorToInt(transform.position);
        rotationToMake = transform.rotation;
        platformOldDown = new Vector3(0, -0.5f, 0);
        pl = platform.position;
        platforms.Add(directionToMove, platform.gameObject);
        chaseCube = showCube.GetComponent<ChaseCubeC>();
    }

    public void MoveCube(int direction)
    {
        if (!isMoving)
        {
            Vector3Int vector = Vector3Int.FloorToInt(transform.position + directions[direction]);

            if(!MainManager.Instance.IsContainsVector(vector)) // если нет нужного вектора, мы не можем туда наступать -> Game Over
                GameOver();
            else                          // если есть, можем идти
            {
                chaseCube.ShowMoves();
                directionToMove = vector;
                rotationToMake = SetRotation(direction);
                score++;
                scoreText.text = score.ToString();
                ShowMoves();
            }
        }
    }
    void GameOver()
    {
        gamePlayUi.SetActive(false);
        gameOverUi.SetActive(true);
    }

    public void GotoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //gamePlayUi.SetActive(false);
        //gameOverUi.SetActive(false);
        //menuUi.SetActive(true);
    }

    public void StartGame()
    {
        gamePlayUi.SetActive(true);
        menuUi.SetActive(false);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ShowMoves()
    {
        if (!platforms.ContainsKey(directionToMove))
        {
            platformOld = platform;
            plOld = pl + platformOldDown;
            var newPlatform = Instantiate(platformPrefab, directionToMove + platformDown, Quaternion.identity);
            platform = newPlatform.transform;
            platforms.Add(directionToMove, newPlatform);
            pl = directionToMove + platformUp;
            moveBackwards = false;
        }
        else
        {
            platformOld = platform;
            plOld = pl + platformOldDown;
            platform = platforms[directionToMove].transform;
            pl = directionToMove + platformUp;
            moveBackwards = true;
        }
        isMoving = true;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (isMoving)
        {
            if (transform.position != directionToMove)
            {
                if (!moveBackwards)
                {
                    transform.position = Vector3.MoveTowards(transform.position, directionToMove, cubeSpeed * Time.deltaTime);
                    platform.position = Vector3.MoveTowards(platform.position, pl, platformSpeed * Time.deltaTime);
                    platformOld.position = Vector3.MoveTowards(platformOld.position, plOld, platformSpeed / 10 * Time.deltaTime);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMake, cubeRotationSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, directionToMove, cubeSpeed * Time.deltaTime);
                    platform.position = Vector3.MoveTowards(platform.position, pl, platformSpeed / 30 * Time.deltaTime);
                    platformOld.position = Vector3.MoveTowards(platformOld.position, plOld, platformSpeed / 10 * Time.deltaTime);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMake, cubeRotationSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                transform.position = directionToMove;
                platform.position = pl;
                platformOld.position = plOld;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isMoving = false;
            }

        }
    }
}
