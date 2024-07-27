using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CubeSpawner : GenericSpawner<CubeStats>
{
    [SerializeField] private float _spawnPointY = 20;
    [SerializeField] private float _minSpawnPointX = -9;
    [SerializeField] private float _maxSpawnPointX = 9;
    [SerializeField] private float _minSpawnPointZ = -9;
    [SerializeField] private float _maxSpawnPointZ = 9;
    [SerializeField] private float _spawnDelay = 1;
    [SerializeField] private TMP_Text _cubesCreated;
    [SerializeField] private TMP_Text _cubesOnScene;

    private float _createdCubesCount = 0;

    public event Action<CubeStats> CubeReleased;

    private void Awake()
    {
        DeclarePool();               
    }
       
    private void Start()
    {
        StartCoroutine(StartSpawning());        
    }

    private void Update()
    {
        _cubesOnScene.text = Pool.CountActive.ToString();
    }

    protected override void OnGet(CubeStats cube)
    {
        cube.transform.position = new Vector3(UnityEngine.Random.Range(_minSpawnPointX, _maxSpawnPointX), _spawnPointY,
                                                   UnityEngine.Random.Range(_minSpawnPointZ, _maxSpawnPointZ));
        cube.CubeRigidbody.velocity = Vector3.zero;
        cube.LifeSpanEnded += ReleaseCube;
        cube.gameObject.SetActive(true);
    }

    private void ReleaseCube(CubeStats cubeStats)
    {
        CubeReleased?.Invoke(cubeStats);

        Pool.Release(cubeStats);        

        cubeStats.LifeSpanEnded -= ReleaseCube;
    }

    private void GetCube()
    {
        CountCubes();

        Pool.Get();
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

    private void CountCubes() 
    {
        _createdCubesCount++;

        _cubesCreated.text = _createdCubesCount.ToString(); 
    }
}
