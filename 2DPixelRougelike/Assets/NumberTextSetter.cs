//using Sirenix.OdinInspector.Editor.Internal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberTextSetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string setterTag = null;
    public void SetText(string _text, string _setterTag)
    {
        if(setterTag != _setterTag) { return; }
        text.SetText( _text );
    }
}
