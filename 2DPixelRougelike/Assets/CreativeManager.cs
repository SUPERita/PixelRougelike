using DG.Tweening;
using Sirenix.OdinInspector;
//using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreativeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
    [SerializeField] private float startScale = 10f;
    [SerializeField] private float _time = .5f;
    [SerializeField] private float endScale = 1f;
    [SerializeField] private string[] sentences;
    [SerializeField] private Image img1;
    [SerializeField] private Image img2;
    public float _t = .5f;
    int _index = 0;
    [Button]
    public void TweenNext()
    {
        m_TextMeshProUGUI.enabled = true;
        m_TextMeshProUGUI.SetText(sentences[_index]);
        _index++;
        m_TextMeshProUGUI.gameObject.transform.localScale = Vector3.one * startScale;
        m_TextMeshProUGUI.gameObject.transform.DOScale(Vector3.one* endScale, _time).SetUpdate(true);
    }

    private void Start()
    {
        m_TextMeshProUGUI.enabled = false;
        Time.timeScale = 1;
    }

    [Button]
    public void SwapImages()
    {
        img1.rectTransform.DOAnchorPos(img2.rectTransform.anchoredPosition, _t);//.SetEase(Ease.OutElastic);
        img2.rectTransform.DOAnchorPos(img1.rectTransform.anchoredPosition, _t);//.SetEase(Ease.OutElastic);

        img1.transform.DOShakeScale(_t, 1.1f);
        img2.transform.DOShakeScale(_t, 1.1f);
    }
}
