using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStats : MonoBehaviour
{
    [SerializeField] private float _alphaChangeDelay;
    [SerializeField] private float _alphaChangeSpeed;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _upwardsModifier;

    private Color _bombColor = Color.black;
    private Renderer _bombRenderer;
    private float _minAlpha = 0;

    public event Action<BombStats> BombExploded;

    private void Awake()
    {
        _bombRenderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _bombRenderer.material.color = _bombColor;
        StartCoroutine(StartAlphaChange());
    }

    private IEnumerator StartAlphaChange()
    {
        WaitForSeconds wait = new WaitForSeconds(_alphaChangeDelay);
        float alpha = 1;

        while (_bombRenderer.material.color.a > _minAlpha)
        {
            alpha = Mathf.MoveTowards(alpha, _minAlpha, _alphaChangeSpeed);

            _bombRenderer.material.color = new Color(_bombRenderer.material.color.r, _bombRenderer.material.color.g,
                                                     _bombRenderer.material.color.b, alpha);

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
            if (hit.TryGetComponent<BombStats>(out _) || hit.TryGetComponent<CubeStats>(out _))
            {
                affectedObjects.Add(hit.GetComponent<Rigidbody>());
            }
        }

        return affectedObjects;
    }
}
