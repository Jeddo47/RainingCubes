using System;
using UnityEngine;

public class BombSpawner : GenericSpawner<BombStats>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private Vector3 _bombPosition;

    public event Action CubesOnSceneChanged;

    private void Awake()
    {
        DeclarePool();
    }

    private void OnEnable()
    {
        _cubeSpawner.CubeReleased += GetBomb;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeReleased -= GetBomb;
    }

    protected override void OnGet(BombStats bomb)
    {
        bomb.gameObject.SetActive(true);
        bomb.transform.position = _bombPosition;
        bomb.BombExploded += ReleaseBomb;

        CubesOnSceneChanged?.Invoke();
    }

    private void GetBomb(CubeStats cube)
    {
        _bombPosition = cube.transform.position;

        Pool.Get();

        CountActiveObjects();
        CountObjects();
    }

    private void ReleaseBomb(BombStats bomb)
    {
        bomb.BombExploded -= ReleaseBomb;

        Pool.Release(bomb);

        CountActiveObjects();
    }
}
