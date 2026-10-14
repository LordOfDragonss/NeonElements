using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public enum TeleportType
    {
        Position,
        Scene
    }

    public Transform targetPosition;
    public UnityEngine.SceneManagement.Scene targetScene;

    public TeleportType teleportType;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
            Teleport(collision.gameObject);
    }
    public void Teleport(GameObject player)
    {
        if (teleportType == TeleportType.Position)
        {
            player.transform.position = targetPosition.position;
        }
        else if (teleportType == TeleportType.Scene)
        {
            SceneManager.LoadScene(targetScene.name);
        }
    }
}
