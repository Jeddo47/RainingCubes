using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericSpawner<Type> : MonoBehaviour where Type : MonoBehaviour
{
    [SerializeField] private Type _prefabType;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    private float _objectsCreated = 0;
    private float _objectsOnScene = 0;
    private ObjectPool<Type> _pool;

    public event Action<float> ObjectsOnSceneCountChanged;
    public event Action<float> ObjectsCountChanged;

    private void Awake() 
    {
        _pool = new ObjectPool<Type>(
            createFunc: () => Instantiate(_prefabType),
            actionOnGet: OnGet,
            actionOnRelease: (prefabType) => prefabType.gameObject.SetActive(false),
            actionOnDestroy: (prefabType) => Destroy(prefabType),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    protected abstract void OnGet(Type prefabType);

    protected virtual void ReleaseObject(Type prefabType) 
    {
        _pool.Release(prefabType);
        
        CountActiveObjects();
    }

    protected virtual void GetObject() 
    {
        _pool.Get();

        CountActiveObjects();
        CountObjects();
    }

    protected void CountObjects() 
    {
        _objectsCreated++;

        ObjectsCountChanged?.Invoke(_objectsCreated);
    }

    protected void CountActiveObjects() 
    {
        _objectsOnScene = _pool.CountActive;

        ObjectsOnSceneCountChanged?.Invoke(_objectsOnScene);            
    }
}
