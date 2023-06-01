using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedCanvas : StaticInstance<SharedCanvas>
{
    public Camera _mainCameraRef = null;

    //overrides the StaticInstance awake?!
    //private void Awake()
    //{
    //    //_mainCameraRef = Camera.main;
    //}

   
}
