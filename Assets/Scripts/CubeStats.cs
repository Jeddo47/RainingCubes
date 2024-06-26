using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]

public class CubeStats : MonoBehaviour
{
    [SerializeField] private float _minLifespan = 2;
    [SerializeField] private float _maxLifespan = 5;
    [SerializeField] private Color _cubeColor = Color.white;
    private Renderer _cubeRenderer;
    private bool _didCollisionHappen;

    public event Action<CubeStats> LifeSpanEnded;

    public Rigidbody CubeRigidbody { get; private set; }

    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
        CubeRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _cubeRenderer.material.color = _cubeColor;
        _didCollisionHappen = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_didCollisionHappen == false && collision.gameObject.TryGetComponent(out Platform platformDetector))
        {
            _didCollisionHappen = true;
            _cubeRenderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            StartCoroutine(nameof(CountLifeSpan));
        }
    }

    private IEnumerator CountLifeSpan()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_minLifespan, _maxLifespan));

        LifeSpanEnded?.Invoke(this);
    }
}
