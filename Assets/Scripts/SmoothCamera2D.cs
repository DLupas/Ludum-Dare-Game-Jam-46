using UnityEngine;

public class SmoothCamera2D : MonoBehaviour
{

    public GameObject target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float peekDistance;
    public Animator animator;

    void FixedUpdate()
    {
        animator.SetBool("checking", false);
        Vector3 desiredPosition = target.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (Input.GetKey("left")) 
        {
            target.transform.rotation = Quaternion.Euler(0, 0, 180);
            animator.SetBool("checking", true);
            target.GetComponent<PlayerMove>().enabled = false;
            transform.position = new Vector3(smoothedPosition.x - peekDistance, smoothedPosition.y, smoothedPosition.z);
        }
        if (Input.GetKey("right")) 
        {
            target.transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool("checking", true);
            target.GetComponent<PlayerMove>().enabled = false;
            transform.position = new Vector3(smoothedPosition.x + peekDistance, smoothedPosition.y, smoothedPosition.z);
        }
        if (Input.GetKey("down"))
        {
            target.transform.rotation = Quaternion.Euler(0, 0, 270);
            animator.SetBool("checking", true);
            target.GetComponent<PlayerMove>().enabled = false;
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y - peekDistance, smoothedPosition.z);
        }
        if (Input.GetKey("up"))
        {
            target.transform.rotation = Quaternion.Euler(0, 0, 90);
            animator.SetBool("checking", true);
            target.GetComponent<PlayerMove>().enabled = false;
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y + peekDistance, smoothedPosition.z);
        }
        else
        {

            target.GetComponent<PlayerMove>().enabled = true;
        }
    }

}