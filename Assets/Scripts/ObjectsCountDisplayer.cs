using TMPro;
using UnityEngine;

public class ObjectsCountDisplayer : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;

    private void OnEnable()
    {
        _cubeSpawner.ObjectsCountChanged += ChangeText;
        _cubeSpawner.ObjectsOnSceneCountChanged += ChangeText;
        _bombSpawner.ObjectsCountChanged += ChangeText;
        _bombSpawner.ObjectsOnSceneCountChanged += ChangeText;
    }

    private void OnDisable()
    {
        _cubeSpawner.ObjectsCountChanged -= ChangeText;
        _cubeSpawner.ObjectsOnSceneCountChanged -= ChangeText;
        _bombSpawner.ObjectsCountChanged -= ChangeText;
        _bombSpawner.ObjectsOnSceneCountChanged -= ChangeText;
    }

    private void ChangeText(TMP_Text text, float value) 
    {
        text.text = value.ToString();                            
    }
}
