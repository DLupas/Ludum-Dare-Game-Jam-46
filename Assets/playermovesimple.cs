using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovesimple : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    Vector3 movement;
    public Animator animator;
    public Vector2 lookFacing;
    public Transform[] moveAroundHouse;
    int currentGoToPosition;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        
        if (Vector2.Distance(transform.position, moveAroundHouse[currentGoToPosition].position) <= 1f)
        {
            if (currentGoToPosition >= moveAroundHouse.Length - 1)
            {
                currentGoToPosition = 0;
            }
            else
            {
                currentGoToPosition++;
            }
            
        }
        Vector3 aimTowards = moveAroundHouse[currentGoToPosition].position;
        

        movement = (aimTowards - transform.position).normalized;
        Debug.Log(movement);


        if (movement.x > 0f && movement.y == 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (movement.x > 0f && movement.y > 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }
        if (movement.x == 0f && movement.y > 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (movement.x < 0f && movement.y > 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 135);
        }
        if (movement.x < 0f && movement.y == 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (movement.x < 0f && movement.y < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 225);
        }
        if (movement.x == 0f && movement.y < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        if (movement.x > 0f && movement.y < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 315);
        }



        transform.position = Vector2.MoveTowards(transform.position, aimTowards, moveSpeed * Time.deltaTime);


        animator.SetBool("moving", movement.x != 0 || movement.y != 0);


    }
}
