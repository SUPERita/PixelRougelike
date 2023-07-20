using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Lean.Pool.LeanGameObjectPool;

public class MessageBoard : StaticInstance<MessageBoard>
{
    [Header("header")]
    [SerializeField] private GameObject headerPrefab = null;
    [SerializeField] private RectTransform headerRoot = null;
    [SerializeField] private AnimationCurve headerPosCurve = null;
    [SerializeField] private float headerTime = 2f;

    [Header("messages")]
    [SerializeField] private GameObject msgPrefab = null;
    [SerializeField] private RectTransform msgRoot = null;
    private List<RectTransform> msgList = new List<RectTransform>();
    [Header("waves")]
    [SerializeField] private GameObject waveHeaderPrefab = null;
    [SerializeField] private RectTransform waveRoot = null;
    [SerializeField] private RectTransform waveEndPoint = null;
    [SerializeField] private AnimationCurve waveHeaderPosCurve = null;
    private GameObject currentWaveHeader = null;

    [Header("design")]
    [SerializeField] private int msgMargin = 50;
    [SerializeField] private float indentSpeed = .5f;
    [SerializeField] private float msgLifeTime = 2f;

    [Button]
    public void SpawnMessage(string _msg, float _delay = -1f)
    {
        if (_delay > 0) { StartCoroutine(DoInTime(() => SpawnMessage(_msg), _delay)); return; }

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

        Debug.Log("do something about null rect here?");
        if(_g == null) { return; }

        _g.DOAnchorPosX(-_g.rect.width, indentSpeed).OnComplete(() => {
            _g.transform.DOKill();
            msgList.Remove(_g);
            Destroy(_g.gameObject);
        }).SetEase(Ease.InExpo);
    }

    [Button]
    public void SpawnHeader(string _msg, float delay = -1)
    {
        if (delay > 0) { StartCoroutine(DoInTime(() => SpawnHeader(_msg), delay)); return; }
  

        RectTransform _g = Instantiate(headerPrefab, headerRoot).GetComponent<RectTransform>();
        float startPos = 1500;//headerRoot.rect.width + _g.rect.width;
        float endPos = -1500;//-headerRoot.rect.width - _g.rect.width;

        //tweens
        _g.anchoredPosition = new Vector2(startPos, 0);  // .ChangeStartValue(-_g.rect.width)
        Sequence _sequence = DOTween.Sequence();
       // _sequence.Append(_g.DOAnchorPosX(200, .5f)).SetEase(Ease.InOutSine);
       // _sequence.Append(_g.DOAnchorPosX(-200, 2f)).SetEase(Ease.InOutCirc);
        _sequence.Append(_g.DOAnchorPosX(endPos - _g.rect.width, headerTime).SetEase(headerPosCurve)
                           //.SetDelay(.5f)
                           .OnComplete(() => {
                               _g.DOKill();
                                Destroy(_g.gameObject);   
                           }));
        _sequence.Play();

        //messaging
        if (_msg == null) _msg = "N/A";
        _g.GetComponentInChildren<TextMeshProUGUI>().SetText(_msg);



    }

    [Button]
    public void SpawnWaveHeader(string _msg, float delay = -1)
    {
        if (delay > 0) { StartCoroutine(DoInTime(() => SpawnWaveHeader(_msg), delay)); return; }


        RectTransform _g = Instantiate(waveHeaderPrefab, waveRoot).GetComponent<RectTransform>();
        float startPos = 1500;//headerRoot.rect.width + _g.rect.width;

        //tweens
        _g.anchoredPosition = new Vector2(startPos, 0);
        Sequence _sequence = DOTween.Sequence();
        //go to middle
        _sequence.Append(_g.DOAnchorPosX(waveEndPoint.anchoredPosition.x, 2f).SetEase(waveHeaderPosCurve).OnComplete(() => {
            _g.DOScale(.2f, .5f);
        }));
        //go to end
        _sequence.Append(_g.DOAnchorPos3D(waveEndPoint.anchoredPosition, .5f).OnComplete(() => {
            Destroy(currentWaveHeader);
            currentWaveHeader = _g.gameObject;
        }));
        //commit
        _sequence.Play();

        //messaging
        if (_msg == null) _msg = "N/A";
        _g.GetComponentInChildren<TextMeshProUGUI>().SetText(_msg);



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

    private IEnumerator DoInTime(UnityAction _A, float time = 0f)
    {
        yield return new WaitForSecondsRealtime(time);
        _A.Invoke();
    }
}
