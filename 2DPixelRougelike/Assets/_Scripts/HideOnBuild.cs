using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnBuild : MonoBehaviour
{
    // Start is called before the first frame update
#if !UNITY_EDITOR
    void Start()
    {
        gameObject.SetActive(false);
    }
#endif


}
