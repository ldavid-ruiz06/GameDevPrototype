using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool inPlayerSight(GameObject alien)
    {
        // get angle to the alien
        Vector3 direction = Vector3.Normalize(alien.transform.position - transform.position);
        float angle = Vector3.Angle(transform.forward, direction);
        float anglePlayer = GetComponent<Shoot>().anglePlayer;

        return (angle < anglePlayer);
    }
}
