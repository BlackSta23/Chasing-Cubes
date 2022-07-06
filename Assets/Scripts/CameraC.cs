using UnityEngine;

public class CameraC : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Vector3Int offset;

    void LateUpdate()
    {
        transform.position = _player.position + offset;
    }
}
