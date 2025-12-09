using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    public float life;
    public UnityEvent onDeath;

    void Update()
    {
        if(life <= 0)
        {
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }

}
