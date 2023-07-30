using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private int healingAmount = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement _out))
        {
            if (!_out.gameObject.GetComponent<Health>().IsFullHealth())
            {
                _out.gameObject.GetComponent<Health>().HealHealth(healingAmount);
                Destroy(gameObject);
            }
        }
    }
}
