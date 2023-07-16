using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private RectTransform indicator;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Vector3 offset = Vector2.zero;
    private RectTransform currentTarget = null;
    private Vector3 currentOffset = Vector3.zero;
    private void Start()
    {
        indicator.GetComponent<CanvasGroup>().alpha = 0f;
    }
    public void RequestTooltip(RectTransform _to, string _description, Vector3 _offset)
    {
        currentOffset = _offset;
        text.SetText(_description);
        indicator.GetComponent<CanvasGroup>().DOFade(1, .5f).SetUpdate(true);
        indicator.transform.DOScaleY(1f, .5f);
        currentTarget = _to;
    }
    private void Update()
    {
        if(currentTarget == null) { return; }
        indicator.position = 
            currentTarget.position + 
            offset * Helpers.CurrentScreenSizeRelativeTo1920() +
            currentOffset * Helpers.CurrentScreenSizeRelativeTo1920() +
            (Vector3.up * currentTarget.rect.height * Helpers.CurrentScreenSizeRelativeTo1920() / 2) + 
            (Vector3.up * indicator.rect.height * Helpers.CurrentScreenSizeRelativeTo1920() / 2);
        //Debug.Log(currentTarget.position + ", " + currentTarget.rect.height + ", " + indicator.position);
    }
    public void ReleaseTooltip()
    {
        indicator.GetComponent<CanvasGroup>().DOKill();
        indicator.GetComponent<CanvasGroup>().DOFade(0,.25f).SetUpdate(true);
    }
}
