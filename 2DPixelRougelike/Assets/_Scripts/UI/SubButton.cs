using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubButton : MonoBehaviour
{

    [Header("normal")]
    [SerializeField] private TextMeshProUGUI txtStr1;
    [SerializeField] private TextMeshProUGUI txtVal1;
    [SerializeField] private Image image1;
    [Header("highlight")]
    [SerializeField] private Image highlightImage;
    public int _value1 { get; private set; }
    public string _string1 { get; private set; }

    public ScriptableObject obj1 { get; private set; }
    public MonoBehaviour behav1 { get; private set; }

    private SubButtonListener buttonListener;


    public SubButton InitializeButton(SubButtonListener _listener, Sprite _image1 = null, string _n = "empty", int _v = -1)
    {
        buttonListener = _listener;

        //image
        if(_image1) { image1.sprite = _image1; }
        //int1
        _value1 = _v;
        if(_v <0 && txtVal1) { txtVal1.SetText(_v.ToString()); }
        //string1
        _string1 = _n;
        if (txtStr1) { txtStr1.SetText(_string1); }



        return this;
    }
    public SubButton AddAdditionalData(ScriptableObject _obj)
    {
        obj1 = _obj;

        return this;
    }
    public SubButton AddAdditionalData(MonoBehaviour _behav)
    {
        behav1 = _behav;

        return this;
    }


    public SubButton SetHighlight(bool _state)
    {
        highlightImage.enabled = _state;

        return this;
    }

    public void Button_OnClick()
    {
        buttonListener.OnClicked(this);
    }

}

public interface SubButtonListener
{
    void OnClicked(SubButton _button);
}
