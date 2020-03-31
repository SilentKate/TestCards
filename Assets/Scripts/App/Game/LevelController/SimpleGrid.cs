using UnityEngine;

public class SimpleGrid : MonoBehaviour
{
    public Point[] Points { get; private set; }

    private void Awake()
    {
        Points = gameObject.transform.GetComponentsInChildren<Point>();
    }

}