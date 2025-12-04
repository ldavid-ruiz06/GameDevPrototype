using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    public GameObject player;

    private PlayerBodyManager body;

    void Start()
    {
        body = player.GetComponent<PlayerBodyManager>();
        
    }

    void Update()
    {
        if(body.stolenHead)
        {
            // calculate the angle to the player and look that direction
            // Vector3 direction = Vector3.Normalize(player.transform.position - transform.position);
            // float angle = Vector3.Angle(new Vector3(0,0,1), direction);
            // print(angle);
            
            // transform.eulerAngles = new Vector3(0, angle, 0);

            transform.LookAt(player.transform);
        }
    }
}
