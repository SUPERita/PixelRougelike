using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolParticle : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem _particleSystem { get; private set; }
    [field: SerializeField] public Transform _particleTransform { get; private set; }
    private ObjectPool<PoolParticle> pool;
    //pool
    public void SetPool(ObjectPool<PoolParticle> _pool) => pool = _pool;
    public void CallReleaseToPool(float _inSeconds)
    {
        Invoke(nameof(ReleaseToPool), _inSeconds);

    }
    private void ReleaseToPool()
    {
        pool.Release(this);
    }
}
