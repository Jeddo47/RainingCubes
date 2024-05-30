using UnityEngine;

public class Painter : MonoBehaviour
{
    public void ResetToDefaultColor(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.white;
    }

    public void PaintCube(GameObject obj)
    {        
        obj.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
    }
}
