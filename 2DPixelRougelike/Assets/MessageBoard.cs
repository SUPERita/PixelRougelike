using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MessageBoard : StaticInstance<MessageBoard>
{
    [SerializeField] private GameObject msgPrefab = null;
    [SerializeField] private RectTransform msgRoot = null;
    private List<RectTransform> msgList = new List<RectTransform>();

    [Header("design")]
    [SerializeField] private int msgMargin = 50;
    [SerializeField] private float indentSpeed = .5f;
    [SerializeField] private float msgLifeTime = 2f;

    [Button]
    public void SpawnMessage(string _msg)
    {
        IndentMsgs();
        RectTransform _g = Instantiate(msgPrefab, msgRoot).GetComponent<RectTransform>();
        _g.anchoredPosition = new Vector2(-_g.rect.width, 0);  // .ChangeStartValue(-_g.rect.width)
        _g.DOAnchorPosX(0, indentSpeed);

        if(_msg == null) _msg = "N/A";
        _g.GetComponentInChildren<TextMeshProUGUI>().SetText(_msg);

        msgList.Add(_g);
        RemoveMessage(_g, msgLifeTime);

    }
    private async void RemoveMessage(RectTransform _g, float _InTimeSeconds)
    {
        await Task.Delay((int)(_InTimeSeconds*1000f));

        _g.DOAnchorPosX(-_g.rect.width, indentSpeed).OnComplete(() => {
            _g.transform.DOKill();
            msgList.Remove(_g);
            Destroy(_g.gameObject);
        }).SetEase(Ease.InExpo);
    }


    //utils
    private void IndentMsgs()
    {
        int _i = msgList.Count;
        foreach(RectTransform _g in msgList)
        {
            _g.DOAnchorPosY( - msgMargin*_i, indentSpeed);
            _i--;
        }
    }
}
