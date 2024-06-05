using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnPointY = 20;
    [SerializeField] private float _minSpawnPointX = -9;
    [SerializeField] private float _maxSpawnPointX = 9;
    [SerializeField] private float _minSpawnPointZ = -9;
    [SerializeField] private float _maxSpawnPointZ = 9;
    [SerializeField] private float _spawnDelay = 1;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private GameObject _cubePrefab;
    
    private ObjectPool<CubeStats> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<CubeStats>(
            createFunc: () => Instantiate(_cubePrefab.GetComponent<CubeStats>()),
            actionOnGet: (obj) => ActOnGet(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);        
    }
       
    private void Start()
    {
        StartCoroutine(nameof(StartSpawning));        
    }
       
    public void ReleaseCube(CubeStats cubeStats)
    {
        _pool.Release(cubeStats);

        cubeStats.LifeSpanEnded -= ReleaseCube;
    }

    private void ActOnGet(CubeStats cubeStats)
    {
        cubeStats.transform.position = new Vector3(Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY, Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        cubeStats.CubeRigidbody.velocity = Vector3.zero;
        cubeStats.LifeSpanEnded += ReleaseCube;
        cubeStats.gameObject.SetActive(true);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private IEnumerator StartSpawning() 
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);    

        while (true) 
        {
            GetCube();

            yield return wait;
        }
    }
}
