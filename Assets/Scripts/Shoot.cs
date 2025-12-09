using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject prefab;
    public GameObject shootPoint;
    public float range;
    public float anglePlayer;
    public float cooldown;
    public LayerMask enemyLayer;

    
    private float nextShoot = 0; 
    public void OnFire(InputValue value)
    {
        PlayerBodyManager body = GetComponent<PlayerBodyManager>();

        if (value.isPressed)
        {
            // if it has the arm, shoot the gun if not in cd
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
            // else, punch whoever he has in front
            else if(body.stolenArm)
            {
                // copy-paste from the code from class
                Collider[] colliders = Physics.OverlapSphere(transform.position, range, (int)enemyLayer);

                if (colliders.Length > 0)
                {
                    Collider closest = null;
                    float closestAngle = 0;
                    for(int i = 0; i < colliders.Length; i++)
                    {
                        Collider collider = colliders[i];
                        Vector3 direction = Vector3.Normalize(collider.transform.position - transform.position);
                        float angle = Vector3.Angle(transform.forward, direction);

                        // for each detected alien, check first if they are in reach
                        if(angle < anglePlayer)
                        {   
                            // then pick the closest one to the center (angle 0)
                            if(closest != null)
                            {
                                if(angle < closestAngle)
                                {
                                    closestAngle = angle;
                                    closest = colliders[i];
                                }
                            }else if(closest == null)
                            {
                                closestAngle = angle;
                                closest = colliders[i];
                            }

                            // once found the closest one; hit them
                            hit(closest);
                        }
                    }
                }
            }
        }
    }

    // if weapon is lost, hit aliens
    void hit(Collider alien)
    {
        //alien.getPunch();
        print(alien.name + " got punched");
    }

}
