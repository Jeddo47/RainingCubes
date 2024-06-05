using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnPointY = 20;
    [SerializeField] private float _minSpawnPointX = -9;
    [SerializeField] private float _maxSpawnPointX = 9;
    [SerializeField] private float _minSpawnPointZ = -9;
    [SerializeField] private float _maxSpawnPointZ = 9;
    [SerializeField] private float _spawnRate = 1;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private int _delay = 1;
    [SerializeField] private GameObject _cubePrefab;
    
    private ObjectPool<CubeStats> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<CubeStats>(
            createFunc: () => Instantiate(_cubePrefab.GetComponent<CubeStats>()),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);        
    }
       
    private void Start()
    {
        InvokeRepeating(nameof(GetCube), _delay, _spawnRate);
    }
       
    public void ReleaseCube(CubeStats cubeStats)
    {
        _pool.Release(cubeStats);

        cubeStats.LifeSpanEnded -= ReleaseCube;
    }

    private void ActionOnGet(CubeStats cubeStats)
    {
        cubeStats.transform.position = new Vector3(Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY, Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        cubeStats.GetRigidbody().velocity = Vector3.zero;
        cubeStats.LifeSpanEnded += ReleaseCube;
        cubeStats.gameObject.SetActive(true);
    }

    private void GetCube()
    {
        _pool.Get();
    }
}
