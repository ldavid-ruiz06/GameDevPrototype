using UnityEngine;

public class PickUpRange : MonoBehaviour
{
    private GameObject parent;

    public float range;
    public GameObject player;
    public PlayerBodyManager body;
    public GameObject alien;

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
        if(parent.name == "FakeHead") body.stolenHead = false;
        else if(parent.name == "FakeArm") body.stolenArm = false;
        else if(parent.transform.parent.name == "FakeLeg") body.stolenLeg = false;

        alien.GetComponent<AliensAI>().stoleSomething = false;
    }

    void Update()
    {   
        if(parent.GetComponent<Renderer>().enabled)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < range){ pickUp();}    
            
            if(alien != null)
            {
                Transform pos = alien.GetComponent<AliensAI>().posReference.transform;
                transform.position = pos.position;

            }
        }
    }





}