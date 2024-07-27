using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericSpawner<Type> : MonoBehaviour where Type : MonoBehaviour
{
    [SerializeField] private Type _prefabType;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    protected ObjectPool<Type> Pool;

    protected void DeclarePool() 
    {
        Pool = new ObjectPool<Type>(
            createFunc: () => Instantiate(_prefabType),
            actionOnGet: OnGet,
            actionOnRelease: (prefabType) => prefabType.gameObject.SetActive(false),
            actionOnDestroy: (prefabType) => Destroy(prefabType),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    protected abstract void OnGet(Type prefabType);
}
