using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private bool _isMoving;
    private Vector2 _originalPos, _nextPos;
    [SerializeField] private float _moveTime = 0.2f;

    private Vector2 _inputDir;
    
    private void Awake()
    {
        // stuff happens here
    }

    private void FixedUpdate()
    {
        if (_inputDir != Vector2.zero && !_isMoving)
        {
            StartCoroutine(MovePlayer(_inputDir));
        }
    }

    public void ProcessMovement(Vector2 direction)
    {
        _inputDir = direction;
    }

    IEnumerator MovePlayer(Vector2 direction)
    {
        _isMoving = true;

        float elapsedTime = 0f;

        _originalPos = transform.position;
        _nextPos = _originalPos + direction;

        while (elapsedTime <= _moveTime)
        {
            transform.position = Vector2.Lerp(_originalPos, _nextPos, (elapsedTime / _moveTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = _nextPos;

        _isMoving = false;
    }
}
