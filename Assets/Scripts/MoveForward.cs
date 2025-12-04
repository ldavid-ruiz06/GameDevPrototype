using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    float bulletSpeed;
    
    void Update()
    {
        transform.Translate(0, 0, bulletSpeed * Time.deltaTime);
    }
}
