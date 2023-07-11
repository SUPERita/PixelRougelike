using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    //[SerializeField] private GameObject mysteryBox;
    //[SerializeField] private float spawnSpeed = 1f;
    [SerializeField] private float spawnRange = 10f;

    public void SpawnObject(GameObject _obj)
    {

        Vector2 foundPosition = Random.insideUnitSphere*spawnRange;
        GameObject _g = Instantiate(_obj, foundPosition, Quaternion.identity, transform);
        //Debug.Log(foundPosition, _g);

        //Invoke(nameof(SpawnObject), spawnSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

 
}
