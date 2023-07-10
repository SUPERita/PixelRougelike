using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeGroundProp : MonoBehaviour
{
    [SerializeField] private float distance = 10f;
    [SerializeField] private Transform camera = null;
    Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        transform.position = startPos + -Vector3.right*camera.position.x/(distance/10f);
    }
}
