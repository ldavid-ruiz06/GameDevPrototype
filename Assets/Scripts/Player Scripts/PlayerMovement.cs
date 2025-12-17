using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // public stats
    [SerializeField]
    float speed, jumpHeight, rotationSpeed, gravity;

    // private vars
    private Vector2 movementValue;
    private Vector2 lookValue;
    private bool canJump = false;
    private float velocityY = 0f;
    private Rigidbody rb;
    public GameObject cam;
    public GameObject arm;
    private PlayerBodyManager body;

    public Animator animator;


    Vector3 eulerAngles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
       private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        body = GetComponent<PlayerBodyManager>();

        //rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        movementValue = value.Get<Vector2>() * speed;

        animator.SetBool("isWalking", movementValue != Vector2.zero);
    }

    public void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>() * rotationSpeed;
    }

    public void OnTriggerEnter() { canJump = true;}

    public void OnTriggerExit() { canJump = false;}

    public void OnJump() 
    {
        if (canJump)
        { 
            velocityY = jumpHeight;
            canJump = false;
        }
    }

    void Update()
    {
        // limit camera Y-Axis
        float angleIncrement = lookValue.y * Time.deltaTime;

        eulerAngles = cam.transform.localEulerAngles;
        if(eulerAngles.x >  180f) eulerAngles.x -= 360f;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x - angleIncrement, -30f, 30f);
        cam.transform.localEulerAngles = eulerAngles;
        arm.transform.localEulerAngles = eulerAngles;


        // push and move
        if (!body.stolenLeg)
        {    
            transform.Translate(movementValue.x * Time.deltaTime, 
                            velocityY * Time.deltaTime, 
                            movementValue.y * Time.deltaTime);
        }
        else if (body.stolenLeg && !canJump)
        {
            transform.Translate(movementValue.x * Time.deltaTime,
                            velocityY * Time.deltaTime,
                            movementValue.y * Time.deltaTime);
        }
        transform.Rotate(0, lookValue.x * Time.deltaTime, 0); 

        // gravity (i hate this but whatever) //wdym, it's actually how gravity works irl
        if (velocityY > 0) velocityY += -gravity;
        
    }


}
