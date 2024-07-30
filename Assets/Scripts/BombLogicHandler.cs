using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class BombLogicHandler : MonoBehaviour
{
    [SerializeField] private float _alphaChangeDelay;
    [SerializeField] private float _alphaChangeSpeed;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _upwardsModifier;

    private Color _color = Color.black;
    private Renderer _renderer;
    private float _minAlpha = 0;

    public event Action<BombLogicHandler> BombExploded;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _renderer.material.color = _color;
        StartCoroutine(StartAlphaChange());
    }

    private IEnumerator StartAlphaChange()
    {
        WaitForSeconds wait = new WaitForSeconds(_alphaChangeDelay);
        float alpha = 1;

        while (_renderer.material.color.a > _minAlpha)
        {
            alpha = Mathf.MoveTowards(alpha, _minAlpha, _alphaChangeSpeed);

            _renderer.material.color = new Color(_renderer.material.color.r, _renderer.material.color.g,
                                                     _renderer.material.color.b, alpha);

            yield return wait;
        }

        Explode();
    }

    private void Explode()
    {
        List<Rigidbody> affectedObjects = GetAffectedObjectsList();

        foreach (Rigidbody affectedObject in affectedObjects)
        {
            affectedObject.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _upwardsModifier, ForceMode.Impulse);
        }

        BombExploded?.Invoke(this);
    }

    private List<Rigidbody> GetAffectedObjectsList()
    {
        List<Rigidbody> affectedObjects = new List<Rigidbody>();

        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<BombLogicHandler>(out _) || hit.TryGetComponent<CubeLogicHandler>(out _))
            {
                affectedObjects.Add(hit.GetComponent<Rigidbody>());
            }
        }

        return affectedObjects;
    }
}
