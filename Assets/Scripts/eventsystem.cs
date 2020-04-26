using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class eventsystem : MonoBehaviour
{
    public GameObject player;
    GameObject robot;
    public GameObject[] guards;
    public Transform[] winPoints;
    public bool win;
    public bool lose;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        robot = GameObject.FindGameObjectWithTag("Robot");

    }

    // Update is called once per frame
    void Update()
    {

        if (!win)
        {

            for (int i = 0; i < winPoints.Length; i++)
            {

                if (Vector2.Distance(robot.transform.position, winPoints[i].position) <= 5f)
                {
                    youWin();
                }
            }
        }
        if (!lose)
        {
            for (int i = 0; i < guards.Length; i++)
            {
                if (Vector2.Distance(guards[i].transform.position, robot.transform.position) <= 1f)
                {
                    youLose();
                }
            }
        }
        
    }

    public void youWin()
    {
        win = true;
        //play cool victory sound
        SceneManager.LoadSceneAsync("WinScreen");

    }

    public void youLose()
    {
        lose = true;
        //play cool lost sound
        SceneManager.LoadSceneAsync("LoseScreen");


    }
}
