using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBodyManager : MonoBehaviour
{
    [SerializeField]
    public bool stolenHead, stolenArm, stolenLeg;
    private bool hasHead = true, hasArm = true, hasLeg = true;

    public GameObject player, head, arm, leg;
    public GameObject fakeHead, fakeArm, fakeLeg;
    public int partStolen = 0;
    public bool[] hasBeenStolen = {false, false, false};

    // events
    public UnityEvent onToggleCamera;

    void Awake()
    {
        // vars stolen
        stolenHead = false;
        stolenArm = false;
        stolenLeg = false;

        // fake parts
        fakeHead.GetComponent<Renderer>().enabled = false;
        fakeHead.GetComponent<Collider>().enabled = false;
        fakeHead.GetComponentInChildren<Camera>().enabled = false;
        fakeArm.GetComponent<Renderer>().enabled = false;
        fakeArm.GetComponent<Collider>().enabled = false;
        fakeLeg.GetComponentInChildren<Renderer>().enabled = false;
        fakeLeg.GetComponentInChildren<Collider>().enabled = false;
    }

    void Update()
    {
        if(stolenHead == hasHead)
        {
            hasHead = !hasHead;
            head.GetComponentInChildren<Camera>().enabled = hasHead;
            head.GetComponent<Renderer>().enabled = hasHead;
            fakeHead.GetComponentInChildren<Camera>().enabled = !hasHead;
            fakeHead.GetComponent<Renderer>().enabled = !hasHead;
            fakeHead.GetComponent<Collider>().enabled = !hasHead;
        }

        if(stolenArm == hasArm)
        {
            hasArm = !hasArm;
            arm.GetComponent<Renderer>().enabled = hasArm;
            arm.GetComponent<Collider>().enabled = hasArm;
            fakeArm.GetComponent<Renderer>().enabled = !hasArm;
            fakeArm.GetComponent<Collider>().enabled = !hasArm;
        }

        if(stolenLeg == hasLeg)
        {
            hasLeg = !hasLeg;
            fakeLeg.GetComponentInChildren<Renderer>().enabled = !hasLeg;
            fakeLeg.GetComponentInChildren<Collider>().enabled = !hasLeg;
        }
    }

    public void StealBody(string part, GameObject alien)
    {
        print("Stole " + part);
        switch(part)
        {
            case "Head":
                    stolenHead = true;
                    fakeHead.GetComponent<PickUpRange>().alien = alien;
                    break;
            case "Arm": 
                stolenArm = true;
                fakeArm.GetComponent<PickUpRange>().alien = alien;
                break;
            case "Legs": 
                stolenLeg = true;
                fakeLeg.GetComponent<PickUpRange>().alien = alien;
                break;
        }
    }

    public bool hasBodyPart(string parte = "")
    {
        if (partStolen < 3) return true;
        
        if(parte == "Head") return hasHead;
        if(parte == "Arm") return hasArm;
        
        return hasLeg;
    }



}
