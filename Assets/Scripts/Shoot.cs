using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject prefab;
    public GameObject shootPoint;
    public float range;
    public float angle;
    public LayerMask enemyLayer;
    

    public void OnFire(InputValue value)
    {
        PlayerBodyManager body = GetComponent<PlayerBodyManager>();

        if (value.isPressed)
        {
            if (!body.stolenArm)
            {
                GameObject clone = Instantiate(prefab);
                clone.transform.position = shootPoint.transform.position;
                clone.transform.rotation = shootPoint.transform.rotation;
            }
            else if(body.stolenArm)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, range, (int)enemyLayer);
            }
        }
    }

}
