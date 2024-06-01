using System.Collections;
using UnityEngine;

public class CubeStats : MonoBehaviour
{
    [SerializeField] private float _minLifespan = 2;
    [SerializeField] private float _maxLifespan = 5;
    [SerializeField] private Color _cubeColor = Color.white;
    private Renderer CubeRenderer;

    public Spawner Spawner;

    public bool DidCollisionHappen { get; private set; }    

    public void ChangeCollisionStatus()
    {
        DidCollisionHappen = true;
    }

    public void StartCoroutine()
    {
        StartCoroutine(nameof(CountLifeSpan));
    }

    public void ChangeColor()
    {
        CubeRenderer.material.color = new Color(Random.value, Random.value, Random.value);
    }        

    private void Awake()
    {
        CubeRenderer = gameObject.GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        CubeRenderer.material.color = _cubeColor;
        DidCollisionHappen = false;
    }

    private IEnumerator CountLifeSpan()
    {
        yield return new WaitForSeconds(Random.Range(_minLifespan, _maxLifespan));

        Spawner.ReleaseCube(gameObject);
    }
}
