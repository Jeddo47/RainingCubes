using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericSpawner<Type> : MonoBehaviour where Type : MonoBehaviour
{
    [SerializeField] private Type _prefabType;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private TMP_Text _objectsCreatedText;
    [SerializeField] private TMP_Text _objectsOnSceneText;

    private float _objectsCreated = 0;
    private float _objectsOnScene = 0;

    protected ObjectPool<Type> Pool;

    public event Action<TMP_Text, float> ObjectsOnSceneCountChanged;
    public event Action<TMP_Text, float> ObjectsCountChanged;

    private void Awake() 
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

    protected virtual void ReleaseObject(Type prefabType) 
    {
        Pool.Release(prefabType);
        
        CountActiveObjects();
    }

    protected virtual void GetObject() 
    {
        Pool.Get();

        CountActiveObjects();
        CountObjects();
    }

    protected void CountObjects() 
    {
        _objectsCreated++;

        ObjectsCountChanged?.Invoke(_objectsCreatedText, _objectsCreated);
    }

    protected void CountActiveObjects() 
    {
        _objectsOnScene = Pool.CountActive;

        ObjectsOnSceneCountChanged?.Invoke(_objectsOnSceneText, _objectsOnScene);            
    }
}
