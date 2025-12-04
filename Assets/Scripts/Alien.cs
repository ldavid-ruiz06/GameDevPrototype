using UnityEngine;

public class Alien : MonoBehaviour
{
    void Start()
    {
        AlienManager.instance.aliens.Add(this);
    }

    void OnDestroy()
    {
        AlienManager.instance.aliens.Remove(this);
    }
}
