using System;
using UnityEngine;

public class WhooshSfxEmitter : SfxEmitter
{
    Transform _playerTransform;
    private bool _hasPlayedWhoosh;
    [SerializeField] private float _maxLaneDistance = 3.2f;
    private void Start()
    {
        _playerTransform = GameManager.Instance.Player.transform;
    }
    void OnEnable()
    {
        _hasPlayedWhoosh = false;
    }

    void Update()
    {
        if(_playerTransform == null)
        {
            return;
        }

        if (_hasPlayedWhoosh)
        {
            return;
        }

        if (transform.position.z <= _playerTransform.position.z &&
            Mathf.Abs(transform.position.x - _playerTransform.position.x) <= _maxLaneDistance)
        {
            _hasPlayedWhoosh = true;
            PlaySfx();
        }
    }

    void OnDisable()
    {

    }
}
