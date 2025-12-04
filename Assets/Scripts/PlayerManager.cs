using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject player;
    public GameObject mainHead;
    public GameObject mainArm;
    public GameObject mainLeg;
    

    void Start()
    {
        if(instance == null) instance = this;
        else Destroy(this);


        // assign main members parts
        player = GameObject.Find("Player");
        mainHead = GameObject.Find("Player/Head");
        mainArm = GameObject.Find("Player/ArmsPivot/Arm");
        mainLeg = GameObject.Find("Player/Legs");

        
    }
}
