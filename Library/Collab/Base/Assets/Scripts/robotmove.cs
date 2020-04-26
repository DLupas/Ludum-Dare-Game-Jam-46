using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class robotmove : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float rotationSpeed;
    GameObject player;
    Transform playerTransform;
    public Animator animator;

    public float nextWaypointDist;
    Path path;
    Seeker seeker;
    private int currentWaypoint;
    bool reachedEndOfPath;

    bool randomWalk = false;
    Vector3 wanderDestinationPoint;
    Vector3 faceThisPoint;
    public float startWaitTime;
    float waitTime;

    public eventsystem eventsystem;
    public Transform[] winPoints;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        seeker = GetComponent<Seeker>();
        waitTime = startWaitTime;

        InvokeRepeating("CalculatePath", 0f, 0.5f);


    }
    private void CalculatePath()
    {
        if (seeker.IsDone())
        {
            if (randomWalk)
            {
                seeker.StartPath(transform.position, wanderDestinationPoint, onPathComplete);
            }
            else
            {
                seeker.StartPath(transform.position, playerTransform.position, onPathComplete);
            }

        }
    }

    private void onPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            //for (int i = 0; i < path.vectorPath.Count; i++)
            //{
            //    Debug.Log(path.vectorPath[i]);
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            randomWalk = false;
        }

    }

    void FixedUpdate()
    {
        for (int i = 0; i < winPoints.Length; i++)
        {
            if (Vector2.Distance(transform.position, winPoints[i].position) <= 2f)
            {
                eventsystem.youWin();
            }
        }
        
        if (path != null)
        {
            reachedEndOfPath = currentWaypoint >= path.vectorPath.Count - 1;

            if (!reachedEndOfPath)
            {
                if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) <= nextWaypointDist)
                {
                    currentWaypoint++;
                }

                //float angle = Vector3.SignedAngle(transform.position, playerTransform.position, Vector3.forward);
                faceThisPoint = path.vectorPath[currentWaypoint];

                Vector3 movement = (faceThisPoint - transform.position).normalized; //rotating robot
                float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                angle = Mathf.Floor((angle + 22.5f) / 45) * 45;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);

                transform.position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWaypoint], moveSpeed * Time.deltaTime);

            }

        }


        float playerBotDistance = Vector3.Distance(playerTransform.position, transform.position);
        animator.SetBool("Bump", playerBotDistance < 1f);

        if (playerBotDistance < 1f || Vector2.Distance(wanderDestinationPoint, transform.position) <= 5 * nextWaypointDist) //if the robot gets too close the player, it gets bored
        {
            


            if (waitTime <= 0)
			{
                randomWalk = true;
                waitTime = startWaitTime;
                wanderDestinationPoint = WanderPoint();

            }
            else
			{
                waitTime -= Time.deltaTime;

            }
            
		}
        else if (playerBotDistance > 5)
		{
            randomWalk = false;
		}

    }
    public Vector3 WanderPoint()
    {

        Vector2 randPoint = Random.insideUnitCircle * 5; //returns a Vector2, this turns it into a Vector3
        Vector3 wanderPoint = transform.position + new Vector3(randPoint.x, randPoint.y, 0f);
        return wanderPoint;
    }

    
}
