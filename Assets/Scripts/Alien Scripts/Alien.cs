using UnityEngine;

public class Alien : MonoBehaviour
{
    void Start()
    {
        AlienManager.instance.AddAlien(this);
    }

    void OnDestroy()
    {
        AlienManager.instance.RemoveAlien(this);
    }
}
