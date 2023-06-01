using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SettingsImplementer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //makes it draw top to bottom and switch bathc whenever it switches material
        //basicly keep it as it ;
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 0.0f, 1.0f);
    }

  
}
