using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointIndicator : MonoBehaviour
{
    private WaypointTarget target;
    private Camera _main;
    private RectTransform _rect;
    private Transform followTransform;
    private Transform relativeTo;
    private bool started = false;
    [SerializeField] private AnimationCurve angleToXCurve;
    [SerializeField] private AnimationCurve angleToYCurve;
    //[SerializeField] private GameObject testImage;
    //[SerializeField] private Transform testTarget;
    //[SerializeField] float testAngle = 0;
    //[SerializeField] private RectTransform testRoot;
    private void Start()
    {
        _main = Camera.main;
        _rect = GetComponent<RectTransform>();
    }
    public void Initialize(WaypointTarget _target, Transform _relativeTo)
    {
        target = _target;
        relativeTo = _relativeTo;
        followTransform = target.GetTargetTransform();



        started = true;
    }

    //private void Update()
    //{
    //    if (!started) { return; }
    //    if (followTransform == null) { Destroy(gameObject); return; }

    //    Vector3 _direction = followTransform.position - _main.ScreenToWorldPoint(transform.parent.position) /*_rect.position*/;
    //    float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
    //    _rect.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    //    //Debug.Log(_angle + "  " + angleToXCurve.Evaluate(_angle));

    //    _rect.position = GetScreenEdgeFromAngle(_angle);

    //}

    private void Update()
    {
        if (!started) { return; }
        if (followTransform == null) { Destroy(gameObject); return; }

        //Vector3 _direction = _main.WorldToScreenPoint(followTransform.position) - transform.parent.position/*_main.WorldToScreenPoint(relativeTo.position)*/;
        Vector3 _direction = followTransform.position - _main.ScreenToWorldPoint(transform.parent.position)/*_main.WorldToScreenPoint(relativeTo.position)*/;
        float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        //float _angle = testAngle;
        //float _angle = Vector3.SignedAngle(_main.WorldToScreenPoint(testTarget.position), transform.parent.position, Vector3.up);
        _rect.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);

        _rect.position = GetScreenEdgeFromAngle(_angle);

        //_direction = _main.WorldToScreenPoint(followTransform.position) - _main.WorldToScreenPoint(relativeTo.position);
        _direction = _main.WorldToScreenPoint(followTransform.position) - transform.parent.position;
        Debug.Log(_direction);
        _rect.localScale = Mathf.Abs(_direction.x)>960 * Helpers.CurrentScreenSizeRelativeTo1920() || Mathf.Abs(_direction.y)>590 * Helpers.CurrentScreenSizeRelativeTo1920() ?/*_direction.sqrMagnitude > 600 ?*/
            Vector3.one:
            Vector3.one*.25f;
    }

    private Vector2 GetScreenEdgeFromAngle(float _angle)
    {
        return
            Vector3.right * (angleToXCurve.Evaluate(_angle) * 925 + 960) * Helpers.CurrentScreenSizeRelativeTo1920() +
            Vector3.up * (angleToYCurve.Evaluate(_angle) * 500 + 540) * Helpers.CurrentScreenSizeRelativeTo1920();
        //Vector3.right * (angleToXCurve.Evaluate(_angle) * 1920) +
        //Vector3.up * (angleToYCurve.Evaluate(_angle) * 1080);
    }

    //[Button]

    //private void SpawnWithCurves()
    //{
    //    for (int _i = -180; _i < 180; _i++)
    //    {
    //        GameObject _g = Instantiate(testImage, transform);
    //        _g.GetComponent<RectTransform>().SetParent(transform);
    //        _g.GetComponent<RectTransform>().localPosition = GetScreenEdgeFromAngle(_i);
    //    }
    //}

    //[Button]
    //private void DestroySpawns()
    //{
    //    foreach(RectTransform _r in transform.GetComponentsInChildren<RectTransform>())
    //    {
    //        if (_r.gameObject.name != "Image(Clone)") continue;
    //        DestroyImmediate(_r.gameObject);
    //    }
    //}
}
