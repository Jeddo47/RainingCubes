using TMPro;
using UnityEngine;

public abstract class GenericObjectsCountDisplayer<Type> : MonoBehaviour where Type : MonoBehaviour
{
    [SerializeField] private GenericSpawner<Type> _spawner;
    [SerializeField] private TMP_Text _objectsCreatedText;
    [SerializeField] private TMP_Text _objectsOnSceneText;

    private void OnEnable()
    {
        _spawner.ObjectsCountChanged += ChangeObjectsCreatedText;
        _spawner.ObjectsOnSceneCountChanged += ChangeObjectsOnSceneText;
    }

    private void OnDisable()
    {
        _spawner.ObjectsCountChanged -= ChangeObjectsCreatedText;
        _spawner.ObjectsOnSceneCountChanged -= ChangeObjectsOnSceneText;
    }

    private void ChangeObjectsCreatedText(float value)
    {
        ChangeText(_objectsCreatedText, value);
    }

    private void ChangeObjectsOnSceneText(float value)
    {
        ChangeText(_objectsOnSceneText, value);
    }

    private void ChangeText(TMP_Text text, float value)
    {
        text.text = value.ToString();
    }
}
