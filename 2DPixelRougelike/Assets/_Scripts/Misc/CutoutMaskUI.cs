using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskUI : Image
{

    //[SerializeField] private bool allowMatChanges = false;
    //cant change mat properties while this active


    public override Material materialForRendering {
        get {
            //if (allowMatChanges) { return base.materialForRendering; }
               Material material = new Material(base.materialForRendering);
               material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
               return material;
            } 
    }

}
