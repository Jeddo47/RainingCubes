using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;

    private void OnCollisionEnter(Collision collision)
    {
        _spawner.ChangeCubeOnCollision(collision);
    }
}
