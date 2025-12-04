using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject prefab;
    public GameObject shootPoint;
    

    public void OnFire(InputValue value)
    {
        PlayerBodyManager body = GetComponent<PlayerBodyManager>();

        if (value.isPressed && !body.stolenArm)
        {
            GameObject clone = Instantiate(prefab);
            clone.transform.position = shootPoint.transform.position;
            clone.transform.rotation = shootPoint.transform.rotation;
        }
    }

}
