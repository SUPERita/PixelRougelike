using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject mysteryBox;
    [SerializeField] private float spawnSpeed = 1f;
    [SerializeField] private float spawnRange = 10f;
    private bool working = false;

    private void SpawnObject()
    {
        if (!working) { return; }

        Vector2 foundPosition = Random.insideUnitSphere*spawnRange;
        Instantiate(mysteryBox, foundPosition, Quaternion.identity, transform);

        Invoke(nameof(SpawnObject), spawnSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

    public void StopSpawning()
    {
        working = false;
    }
    public void StartSpawning()
    {
        working = true;
        SpawnObject();
    }
}
