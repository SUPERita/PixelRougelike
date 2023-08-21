using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _player = null;
    [SerializeField] RectTransform _rectTransform = null;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.anchoredPosition = _player.position;
    }
}
