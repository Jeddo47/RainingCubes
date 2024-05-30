using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Painter _painter;
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _spawnPointY = 20;
    [SerializeField] private float _minSpawnPointX = -9;
    [SerializeField] private float _maxSpawnPointX = 9;
    [SerializeField] private float _minSpawnPointZ = -9;
    [SerializeField] private float _maxSpawnPointZ = 9;
    [SerializeField] private float _spawnRate = 1;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<GameObject> _pool;

    public void ChangeCubeOnCollision(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<CubeStats>(out CubeStats cubeStats))
        {
            if (cubeStats.DidCollisionHappen == false)
            {
                cubeStats.ChangeCollisionStatus();
                _painter.PaintCube(collision.gameObject);
                cubeStats.StartCoroutine();
            }
        }
    }

    public void ReleaseCube(GameObject gameObject)
    {
        _pool.Release(gameObject);
    }

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject obj)
    {
        _painter.ResetToDefaultColor(obj);
        obj.transform.position = new Vector3(Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY, Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0, _spawnRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }
}
