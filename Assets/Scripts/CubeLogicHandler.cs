using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]

public class CubeLogicHandler : MonoBehaviour
{
    [SerializeField] private float _minLifespan = 2;
    [SerializeField] private float _maxLifespan = 5;

    private Color _color = Color.white;
    private Renderer _renderer;
    private bool _didCollisionHappen;

    public event Action<CubeLogicHandler> LifeSpanEnded;

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        Rigidbody = GetComponent<Rigidbody>();        
    }

    private void OnEnable()
    {
        _renderer.material.color = _color;
        _didCollisionHappen = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_didCollisionHappen == false && collision.gameObject.TryGetComponent<Platform>(out _))
        {
            _didCollisionHappen = true;
            _renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            StartCoroutine(nameof(CountLifeSpan));
        }
    }

    private IEnumerator CountLifeSpan()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_minLifespan, _maxLifespan));

        LifeSpanEnded?.Invoke(this);
    }
}
