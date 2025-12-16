using UnityEngine;

public class DoDamage : MonoBehaviour
{
    public float damage;

    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        Life life = other.GetComponent<Life>();
        if(life != null) life.life += -damage;

        
    }
}
