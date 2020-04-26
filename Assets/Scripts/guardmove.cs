using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class guardmove : MonoBehaviour
{
    public static float speed = 1.2f;
    public static float rotationSpeed = 10f;
    public static float startWaitTime = 1f;
    float waitTime;
    public static float FOV = 6f;
    public static float maxWhistleDistance = 10f;

    public Transform[] patrolSpots;
    int choosePatrolSpot;
    Transform playerTransform;
    public Transform robotTransform;
    public bool seen;

    public static float nextWaypointDist = 0.3f;
    Path path;
    Seeker seeker;
    private int currentWaypoint;
    bool reachedEndOfPath;
    float slowdownDistance;
    float endReachedDistance = 0.1f;

    public bool trackWhistle = false;
    Vector3 whistleSpot;
    Vector3 faceThisPoint;

    public GameObject alert;
    private float alertLvl = 0;



    // Start is called before the first frame update
    void Start()
    {
        //robotTransform = GameObject.FindGameObjectWithTag("Robot").transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //choosePatrolSpot = Random.Range(0, patrolSpots.Length);
        seeker = GetComponent<Seeker>();

        InvokeRepeating("CalculatePath", 0f, 0.5f);

    }

    private void CalculatePath()
    {
        if (seeker.IsDone())
        {
            if (seen)
            {
                seeker.StartPath(transform.position, robotTransform.position, onPathComplete);
            }
            else if (trackWhistle)
            {
                seeker.StartPath(transform.position, whistleSpot, onPathComplete);
            }
            else
            {
                seeker.StartPath(transform.position, patrolSpots[choosePatrolSpot].position, onPathComplete);
            }
            

        }
    }

    private void onPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool checkIfWhistling = playeraudio.isWhistling;
        if (Input.GetKeyDown("space") && !checkIfWhistling && Vector3.Distance(transform.position, playerTransform.position) < maxWhistleDistance) //when the player whistles, track them
        {
            trackWhistle = true;
            whistleSpot = playerTransform.position;
            
        }
    }   

    private void OnDrawGizmos()
    {
        if (seen)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position, robotTransform.position);
        }
        else if (trackWhistle)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, whistleSpot);
        }


    }

    void FixedUpdate()
	{
        float guardBotDistance = Vector3.Distance(transform.position, robotTransform.position);
        if (guardBotDistance < 30f)
        {
            Color col = alert.GetComponent<Image>().color;
            col.a = alertLvl;
            alert.GetComponent<Image>().color = col;


            if (guardBotDistance < FOV)
            {
                Vector3 movement = (robotTransform.position - transform.position).normalized;
                float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                float guardRotation = transform.rotation.z * Mathf.Rad2Deg;


                //- to +
                //reduce
                if (angle < 0f)
                {
                    angle += Mathf.Ceil(Mathf.Abs(angle / 360)) * 360;
                }
                if (angle > 360f)
                {
                    angle %= 360;
                }
                if (guardRotation < 0f)
                {
                    guardRotation += Mathf.Ceil(Mathf.Abs(angle / 360)) * 360;
                }
                if (guardRotation > 360f)
                {
                    guardRotation %= 360;
                }


                float widenAngle = Mathf.Abs(FOV - guardBotDistance) * 45f / FOV; //if the bot is closer, the guard can see in a wider angle
                if (Mathf.Abs(angle - guardRotation) <= widenAngle + 45f)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, robotTransform.position - transform.position); //if player is hidden behind an object

                    if (hit.transform.position == robotTransform.position)
                    {
                        seen = true;
                        trackWhistle = false; //stop tracking whistle
                    }
                    else if (waitTime <= -3)
                    {
                        print("I Am done");
                        seen = false;
                        waitTime = startWaitTime;
                    }

                    else
                    {
                        waitTime -= Time.deltaTime;
                    }
                }

            }
            else if (waitTime <= -3) //copy pasted code I know
            {
                seen = false;
                waitTime = startWaitTime;
            }

            else
            {
                waitTime -= Time.deltaTime;
            }

            if (seen) //pathfinding part
            {
                MoveAlongPath();

                if (alertLvl < 0.4f)
                {
                    alertLvl += 0.001f;
                }
            }
            else if (trackWhistle)
            {

                MoveAlongPath();

                RaycastHit2D hit = Physics2D.Raycast(transform.position, whistleSpot - transform.position, FOV);

                if (hit.collider == null)
                {
                    trackWhistle = false;

                }
            }

            else
            {

                if (Vector3.Distance(transform.position, patrolSpots[choosePatrolSpot].position) <= 7f * nextWaypointDist)
                {
                    if (choosePatrolSpot >= patrolSpots.Length - 1)
                    {
                        choosePatrolSpot = 0;
                    }
                    else
                    {
                        choosePatrolSpot++;
                    }


                }
                MoveAlongPath();

                alertLvl = 0;

            }//moves between checkpoints

        }

    }

    public void MoveAlongPath()
    {
        if (path != null)
        {
            reachedEndOfPath = currentWaypoint >= path.vectorPath.Count - 1;

            if (!reachedEndOfPath)
            {
                if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) <= nextWaypointDist)
                {
                    currentWaypoint++;
                }

                
                faceThisPoint = path.vectorPath[currentWaypoint];
                Vector3 movement = (faceThisPoint - transform.position).normalized;
                float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                angle = Mathf.Floor((angle + 22.5f) / 45) * 45;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);

                float speedFactor = seen ? 1.5f : 1f;
                transform.position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWaypoint], speed * speedFactor * Time.deltaTime);
            }
        }
    }
}
