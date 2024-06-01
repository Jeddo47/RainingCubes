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
    [SerializeField] private Material _material;
    [SerializeField] private Mesh _mesh;

    private ObjectPool<GameObject> _pool;

    public void ChangeCubeOnCollision(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<CubeStats>(out CubeStats cubeStats))
        {
            if (cubeStats.DidCollisionHappen == false)
            {
                cubeStats.ChangeCollisionStatus();
                cubeStats.ChangeColor();
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
        GameObject cube = new GameObject("Cube");
        MeshFilter filter = cube.AddComponent<MeshFilter>();
        filter.mesh = _mesh;
        cube.AddComponent<MeshRenderer>();
        cube.GetComponent<Renderer>().material = _material;
        cube.AddComponent<BoxCollider>();
        cube.AddComponent<Rigidbody>();
        CubeStats cubeStats = cube.AddComponent<CubeStats>();
        cubeStats.Spawner = gameObject.GetComponent<Spawner>();
        cube.transform.position = new Vector3(Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY, Random.Range(_minSpawnPointZ, _maxSpawnPointZ));

        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(cube),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = new Vector3(Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY, Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), _delay, _spawnRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }
}
