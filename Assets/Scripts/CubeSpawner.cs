using System;
using System.Collections;
using UnityEngine;

public class CubeSpawner : GenericSpawner<CubeStats>
{
    [SerializeField] private float _spawnPointY = 20;
    [SerializeField] private float _minSpawnPointX = -9;
    [SerializeField] private float _maxSpawnPointX = 9;
    [SerializeField] private float _minSpawnPointZ = -9;
    [SerializeField] private float _maxSpawnPointZ = 9;
    [SerializeField] private float _spawnDelay = 1;

    public event Action<CubeStats> CubeReleased;

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    protected override void OnGet(CubeStats cube)
    {
        cube.transform.position = new Vector3(UnityEngine.Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY,
                                                   UnityEngine.Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        cube.CubeRigidbody.velocity = Vector3.zero;
        cube.LifeSpanEnded += ReleaseObject;
        cube.gameObject.SetActive(true);
    }

    protected override void ReleaseObject(CubeStats cube)
    {
        CubeReleased?.Invoke(cube);

        base.ReleaseObject(cube);

        cube.LifeSpanEnded -= ReleaseObject;
    }

    private IEnumerator StartSpawning()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        while (true)
        {
            GetObject();

            yield return wait;
        }
    }
}
