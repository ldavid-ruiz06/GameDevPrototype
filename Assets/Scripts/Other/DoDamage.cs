using UnityEngine;

public class DoDamage : MonoBehaviour
{
    public float damage;

    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        
    }
}
