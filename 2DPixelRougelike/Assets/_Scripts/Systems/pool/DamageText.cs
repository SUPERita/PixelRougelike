using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageText : MonoBehaviour
{
    ObjectPool<DamageText> pool;

    TweenParams tweenParams;
    Tweener moveTween;
    Tween scaleUpTween;
    Tween scaleDownTween;
    private void Awake()
    {
        tweenParams = new TweenParams().SetAutoKill(false).SetRecyclable(true);

        moveTween = transform.DOMove(transform.position + GetRandomRelativePosition(), .5f)
            .SetAs(tweenParams).ChangeEndValue(GetRandomRelativePosition());
        scaleUpTween = transform.DOScale(Vector3.one * 1.1f, .5f).SetEase(Ease.OutBounce)
            .SetAs(tweenParams);
        scaleDownTween = transform.DOScale(Vector3.zero, .4f)
            .SetAs(tweenParams).SetDelay(.5f);
        //_startTween = transform.
        //    DOScale(Vector3.one * 1.1f, .5f).SetEase(Ease.OutBounce)
        //          .OnStart(() => transform.
        //          DOMove(
        //                transform.position + ((Vector3.up * UnityEngine.Random.Range(-1f, 1f)) + (Vector3.right * UnityEngine.Random.Range(-1f, 1f))) * 2f,
        //               .5f))
        //          .OnComplete(() => transform.
        //          DOScale(
        //              Vector3.zero,
        //              .4f)
        //          ).SetAutoKill(false).Pause().SetRecyclable;//??!

    }

    public void DOStartTween(bool _moveRandom = true)
    {
        //transform.position = transform.position - Vector3.up;
        transform.localScale = Vector3.one;
        //transform.position = Vector3.one * 50f;

        //??!>!?!
        if(_moveRandom) { 
            moveTween.ChangeEndValue(GetRandomRelativePosition(), true);
        } else
        {
            moveTween.ChangeEndValue(transform.position+Vector3.up*1.5f, true);
        }

        moveTween.Restart();
        scaleDownTween.Restart();
        scaleUpTween.Restart();

        //// Reset the sequence properties
        //sequence.Rewind();
        //sequence.Kill();

        //// Reuse the existing tweens
        //sequence.Append(moveTween);
        //sequence.Append(scaleUpTween);
        //sequence.Append(scaleDownTween);

        //// Start playing the recycled sequence
        //sequence.Restart();
    }

    private Vector3 GetRandomRelativePosition()
    {
        return transform.position + (Vector3.up * UnityEngine.Random.Range(-1f, 1f) + (Vector3.right * UnityEngine.Random.Range(-1f, 1f))) * 2f;
    }

    /*
    //pool
    public void SetPool(ObjectPool<DamageText> _pool) => pool = _pool;
    public void CallReleaseToPool(float _inSeconds)
    {
        Invoke(nameof(ReleaseToPool), _inSeconds);

    }
    private void ReleaseToPool()
    {
        pool.Release(this);
    }
    */
}
