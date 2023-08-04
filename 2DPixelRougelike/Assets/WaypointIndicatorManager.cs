using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointIndicatorManager : StaticInstance<WaypointIndicatorManager> 
{
    [SerializeField] private WaypointIndicator waypointIndicatorPrefab;
    private Transform _playerRef = null;

    public void SummonWaypointIndicator(WaypointTarget _target)
    {
        if(_playerRef ==null) { CachePlayerRef(); }
        Instantiate(waypointIndicatorPrefab, transform)
            .GetComponent<WaypointIndicator>().Initialize(_target, _playerRef);
    }
    private void CachePlayerRef()
    {
        _playerRef = FindAnyObjectByType<PlayerMovement>().transform;
    }
}

public interface WaypointTarget
{
    public Transform GetTargetTransform();
}
