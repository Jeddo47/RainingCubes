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
    [SerializeField] private GameObject _cube;

    private ObjectPool<CubeStats> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<CubeStats>(
            createFunc: () => Instantiate(_cube.GetComponent<CubeStats>()),
            actionOnGet: (obj) => ActionOnGet(obj.gameObject),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);        
    }

    private void OnEnable()
    {
        CubeStats.LifeSpanEnded += ReleaseCube;
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), _delay, _spawnRate);
    }

    private void OnDisable()
    {
        CubeStats.LifeSpanEnded -= ReleaseCube;
    }

    public void ReleaseCube(CubeStats cubeStats)
    {
        _pool.Release(cubeStats);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = new Vector3(Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY, Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void GetCube()
    {
        _pool.Get();
    }
}
