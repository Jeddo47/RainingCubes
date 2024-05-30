using System.Collections;
using UnityEngine;

public class CubeStats : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private float _minLifespan = 2;
    [SerializeField] private float _maxLifespan = 5;

    public bool DidCollisionHappen {  get; private set; }

    public void ChangeCollisionStatus() 
    {
        DidCollisionHappen = true;    
    }

    public void StartCoroutine()
    {
        StartCoroutine(nameof(CountLifeSpan));
    }

    private void OnEnable()
    {
        DidCollisionHappen = false;
    }

    private IEnumerator CountLifeSpan() 
    {
        yield return new WaitForSeconds(Random.Range(_minLifespan, _maxLifespan));

        _spawner.ReleaseCube(gameObject);
    }
}
