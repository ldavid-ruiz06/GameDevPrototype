using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject prefab;
    public GameObject shootPoint;
    public float range;
    public float angle;
    public float cooldown;
    public LayerMask enemyLayer;

    
    private float nextShoot = 0; 
    public void OnFire(InputValue value)
    {
        PlayerBodyManager body = GetComponent<PlayerBodyManager>();

        if (value.isPressed)
        {
            if (!body.stolenArm)
            {
                if(nextShoot < Time.time)
                {    
                    GameObject clone = Instantiate(prefab);
                    clone.transform.position = shootPoint.transform.position;
                    clone.transform.rotation = shootPoint.transform.rotation;

                    nextShoot = Time.time + cooldown;
                }
            }
            else if(body.stolenArm)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, range, (int)enemyLayer);
            }
        }
    }

}
