using UnityEngine;

public class SawSpawner : MonoBehaviour
{

    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private Transform SpawnPosition;
    [SerializeField] private bool giveRigidbody;

    public void SpawnSaw()
    {
        Saw saw = sawPrefab.GetComponent<Saw>().SpawnSaw(SpawnPosition.position);
        if (giveRigidbody)
        {
            saw.gameObject.AddComponent<Rigidbody2D>();
        }
    }
}
