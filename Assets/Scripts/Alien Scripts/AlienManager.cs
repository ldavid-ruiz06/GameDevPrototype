using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class AlienManager : MonoBehaviour
{
    public static AlienManager instance;
    public List<Alien> aliens;
    public UnityEvent onChange;

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this);

        aliens = new List<Alien>();
    }

    public void AddAlien(Alien alien)
    {
        aliens.Add(alien);
        onChange.Invoke();
    }

    public void RemoveAlien(Alien alien)
    {
        aliens.Remove(alien);
        onChange.Invoke();
    }

    
}
