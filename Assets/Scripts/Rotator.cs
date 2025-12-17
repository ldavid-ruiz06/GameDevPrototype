using UnityEngine;

public class Rotator : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 30 * Time.deltaTime, 0);
    }
}
