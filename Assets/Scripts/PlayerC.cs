using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerC : MonoBehaviour
{
    [SerializeField] GameObject showCube, platformPrefab, gamePlayUi, gameOverUi, menuUi;
    [SerializeField] float cubeSpeed = 1.7f, cubeRotationSpeed = 150f, platformSpeed = 21f;
    [SerializeField] Transform platform, platformOld;
    [SerializeField] Text scoreText;

    // cube
    private Vector3Int directionToMove;
    private Quaternion rotationToMake;
    // platforms
    private Vector3 platformDown, platformUp, platformOldDown;
    private Vector3 pl, plOld;
    private Dictionary<Vector3Int, GameObject> platforms = new Dictionary<Vector3Int, GameObject>();

    private bool isMoving, moveBackwards; // do we move the MainCube now, direction 
    private ChaseCubeC chaseCube;
    private int score;


    void Start()
    {
        directionToMove = new Vector3Int(1, 1, 1);
        rotationToMake = transform.rotation;
        platformDown = new Vector3(0, -16, 0);
        platformUp = new Vector3(0, -6, 0);
        platformOldDown = new Vector3(0, -0.5f, 0);
        pl = platform.position;
        platforms.Add(directionToMove, platform.gameObject);
        chaseCube = showCube.GetComponent<ChaseCubeC>();
    }

    public void MoveCube(int direction)
    {
        if (!isMoving)
        {
            Vector3Int dir = new Vector3Int();
            switch (direction)
            {
                case 0:
                    dir = Vector3Int.forward;
                    break;
                case 1:
                    dir = Vector3Int.right;
                    break;
                case 2:
                    dir = Vector3Int.back;
                    break;
                case 3:
                    dir = Vector3Int.left;
                    break;
            }
            Vector3Int vector = MainManager.Instance.VectorMine(new Vector3Int(System.Convert.ToInt32(transform.position.x), 1, System.Convert.ToInt32(transform.position.z)) + dir);

            if (vector != Vector3Int.zero)
            {
                chaseCube.ShowMoves();
                directionToMove = vector;
                SetRotation(direction);
                score++;
                scoreText.text = score.ToString();
                ShowMoves();
            }
            else
                GameOver();
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

    void SetRotation(int r)
    {
        switch (r)
        {
            case 0:
                rotationToMake = Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                break;
            case 1:
                rotationToMake = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90);
                break;
            case 2:
                rotationToMake = Quaternion.Euler(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                break;
            case 3:
                rotationToMake = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90);
                break;
        }
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
