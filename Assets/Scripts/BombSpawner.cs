using TMPro;
using UnityEngine;

public class BombSpawner : GenericSpawner<BombStats>
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private TMP_Text _bombsCreated;
    [SerializeField] private TMP_Text _bombsOnScene;

    private Vector3 _bombPosition;
    private float _createdBombsCount = 0;

    private void Awake()
    {
        DeclarePool();
    }

    private void OnEnable()
    {
        _cubeSpawner.CubeReleased += GetBomb;
    }

    private void Update()
    {
        _bombsOnScene.text = Pool.CountActive.ToString();
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
    }

    private void GetBomb(CubeStats cube)
    {
        CountBombs();

        _bombPosition = cube.transform.position;

        Pool.Get();
    }

    private void ReleaseBomb(BombStats bomb)
    {
        bomb.BombExploded -= ReleaseBomb;

        Pool.Release(bomb);
    }

    private void CountBombs() 
    {
        _createdBombsCount++;

        _bombsCreated.text = _createdBombsCount.ToString();
    }
}
