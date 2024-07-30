using UnityEngine;

public class BombSpawner : GenericSpawner<BombLogicHandler>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private Vector3 _spawnPosition;

    private void OnEnable()
    {
        _cubeSpawner.CubeReleased += SetSpawnPosition;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeReleased -= SetSpawnPosition;
    }

    protected override void OnGet(BombLogicHandler bomb)
    {
        bomb.gameObject.SetActive(true);
        bomb.transform.position = _spawnPosition;
        bomb.BombExploded += ReleaseObject;
    }

    protected override void ReleaseObject(BombLogicHandler bomb)
    {
        bomb.BombExploded -= ReleaseObject;

        base.ReleaseObject(bomb);        
    }

    private void SetSpawnPosition(CubeLogicHandler cube)
    {
        _spawnPosition = cube.transform.position;

        GetObject();
    }
}
