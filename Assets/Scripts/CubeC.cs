using UnityEngine;

public class CubeC : MonoBehaviour
{
    // cube
    protected Vector3Int directionToMove;
    protected Quaternion rotationToMake;
    protected Vector3Int[] directions = new Vector3Int[5];
    protected bool isMoving;

    // platform
    protected Vector3 platformDown = new Vector3(0, -16, 0);
    protected Vector3 platformUp = new Vector3(0, -6, 0);

    private void Awake()
    {
        // чаще идём вперёд чем в любую другую сторону
        directions[0] = Vector3Int.forward;
        directions[1] = Vector3Int.left;
        directions[2] = Vector3Int.right;
        directions[3] = Vector3Int.back;
        directions[4] = Vector3Int.forward;
    }

    protected Quaternion SetRotation(int r)
    {
        return r switch
        {
            0 => Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z),
            1 => Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90),
            2 => Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90),
            3 => Quaternion.Euler(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z),
            4 => Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z),
            _ => Quaternion.identity,
        };
    }
}
