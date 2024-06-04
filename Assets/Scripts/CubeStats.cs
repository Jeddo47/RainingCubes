using System;
using System.Collections;
using UnityEngine;

public class CubeStats : MonoBehaviour
{
    public static Action<CubeStats> LifeSpanEnded;

    [SerializeField] private float _minLifespan = 2;
    [SerializeField] private float _maxLifespan = 5;
    [SerializeField] private Color _cubeColor = Color.white;
    private Renderer _cubeRenderer;
    private Spawner _spawner;
    private bool _didCollisionHappen; 

    private void Awake()
    {
        _cubeRenderer = gameObject.GetComponent<Renderer>();
        _spawner = GameObject.FindObjectOfType<Spawner>();
    }

    private void OnEnable()
    {
        _cubeRenderer.material.color = _cubeColor;
        _didCollisionHappen = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_didCollisionHappen == false)
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
