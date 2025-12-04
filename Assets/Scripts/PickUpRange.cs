using UnityEngine;

public class PickUpRange : MonoBehaviour
{
    private GameObject parent;

    public float range;
    public GameObject player;
    public PlayerBodyManager body;

    void Start()
    {
        parent = this.gameObject;
        parent.layer = LayerMask.NameToLayer("Pickable");
        player = GameObject.Find("Player");
        body = player.GetComponent<PlayerBodyManager>();
        if(parent.name == "FakeLeg") parent = GameObject.Find("FakeLeg/Cylinder");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void pickUp()
    {
        print("In range!");
        if(parent.name == "FakeHead") body.stolenHead = false;
        if(parent.name == "FakeArm") body.stolenArm = false;
        if(parent.transform.parent.name == "FakeLeg") body.stolenLeg = false;
    }

    void Update()
    {   
        if(parent.GetComponent<Renderer>().enabled)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < range){ pickUp();    
            print(this.name + " player is in reach");}
        }
    }





}