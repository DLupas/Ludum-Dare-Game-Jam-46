using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    Vector3 movement;
    Rigidbody2D playerRB;
    public Animator animator;
    public Vector2 lookFacing;
    public eventsystem eventsystem;


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;


        if (movement.x > 0 && movement.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (movement.x > 0 && movement.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }
        if (movement.x == 0 && movement.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (movement.x < 0 && movement.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 135);
        }
        if (movement.x < 0 && movement.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (movement.x < 0 && movement.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 225);
        }
        if (movement.x == 0 && movement.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        if (movement.x > 0 && movement.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 315);
        }






        movement *= Time.deltaTime * moveSpeed; //move multiplied by fration of a second since last update
        transform.position += movement;





        animator.SetBool("moving", movement.x != 0 || movement.y != 0);


    }

}
