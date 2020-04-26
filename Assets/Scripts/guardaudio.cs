using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guardaudio : MonoBehaviour
{
    AudioSource source;
    public AudioClip[] seeRobot;
    public AudioClip[] loseRobot;
    public AudioClip[] hearWhistle;
    public AudioClip[] stopWhistle;
    bool playAgain;
    bool playAgainWhistle;
    public guardmove guardmove;
    //public GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        playAgain = true;
        playAgainWhistle = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool isSeen = guardmove.seen;
        if (isSeen && !source.isPlaying && playAgain)
        {
            //camera.BroadcastMessage("intenseMusic");
            source.clip = seeRobot[Random.Range(0, seeRobot.Length)];
            source.Play();
            playAgain = false;
        }
        if (!isSeen && !source.isPlaying && playAgain==false)
        {
            source.clip = loseRobot[Random.Range(0, seeRobot.Length)];
            source.Play();
            playAgain = true;
        }
        //print("chasing");

        if (playAgain)
        {
            //print("whistleSearach");
            bool trackWhistle = guardmove.trackWhistle;
            if (trackWhistle && !source.isPlaying && playAgainWhistle)
            {
                source.clip = hearWhistle[Random.Range(0, seeRobot.Length)];
                source.Play();
                playAgainWhistle = false;
            }



            if (!trackWhistle && !source.isPlaying && playAgainWhistle == false)
            {
                source.clip = stopWhistle[Random.Range(0, seeRobot.Length)];
                source.Play();
                playAgainWhistle = true;
            }
        }
    }
}
