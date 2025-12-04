using UnityEngine;
using UnityEngine.Events;

public class HeadScript : MonoBehaviour
{
    private bool main = false;
    public Camera cam;
    
    void Start()
    {
        // check if part of the player body
        if(transform.parent != null)
        {
            main = true;
            print("Main head declared.");

        }else
        {
            main = false;
            print("Not main head.");
        }

        // get camera
        cam = GetComponentInChildren<Camera>();
        
        if(!main) cam.enabled = false;

        // add listener
    }

    public void onToggle()
    {
        cam.enabled = !cam.enabled;
    }
}
