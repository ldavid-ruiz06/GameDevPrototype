using UnityEngine;

public class Autodestroy : MonoBehaviour
{
    public float delay = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, delay);
    }

    
}
