using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float lowJumpFallMultiplier = 2f;
    public float jumpVelocity = 8f;
    public float fallMultiplier = 3f;
    public float turningSpeed = 350;
    public float movementPower = 100;
    public Rigidbody rb;
    public Transform mainCamera;
    float myx;
    float vertical;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1)) //COMBAT CONTROLS
        {
            vertical = Input.GetAxis("Vertical") * movementPower * Time.deltaTime;
            if (Input.GetKey("d"))
            {
                myx = 200;
            }
            if (Input.GetKey("a"))
            {
                myx = -200;
            }
            //Vector3 myMovementV = new Vector3(myx, 0, vertical);  
            //transform.Translate(myMovementV);
            rb.AddRelativeForce(myx, 0, vertical);
        }
        else //3D PLATFORMING CONTROLS
        {
            float horizontal = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
            //transform.Rotate(0, horizontal, 0);
            float vertical = Input.GetAxis("Vertical") * movementPower * Time.deltaTime;
            //transform.Translate(0, 0, vertical);
            rb.AddRelativeForce(horizontal, 0, vertical);
        }
        //SHARED INPUT FOR BOTH TYPES OF MOVEMENT
        //IF I AM FALLING, FALL FASTER
        //if (rb.velocity.y < 0) 
        //{
        //    rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        //}
        //IF I AM FALLING, BUT WANT TO JUMP HIGHER
        //else if (rb.velocity.y > 0 && !Input.GetKeyDown("space")) 
        //{
        //   rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpFallMultiplier - 1) * Time.deltaTime;
        //}
        //JUMP
        if (Input.GetKeyDown("space"))
        {
            rb.velocity = Vector3.up * jumpVelocity;
        }
        //EXIT GAME, NOT WORKING YET
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
