using UnityEngine;

public class BombSpawner : GenericSpawner<BombStats>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private Vector3 _bombPosition;

    private void OnEnable()
    {
        _cubeSpawner.CubeReleased += SetBombPosition;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeReleased -= SetBombPosition;
    }

    protected override void OnGet(BombStats bomb)
    {
        bomb.gameObject.SetActive(true);
        bomb.transform.position = _bombPosition;
        bomb.BombExploded += ReleaseObject;
    }

    protected override void ReleaseObject(BombStats bomb)
    {
        bomb.BombExploded -= ReleaseObject;

        base.ReleaseObject(bomb);        
    }

    private void SetBombPosition(CubeStats cube)
    {
        _bombPosition = cube.transform.position;

        GetObject();
    }
}
