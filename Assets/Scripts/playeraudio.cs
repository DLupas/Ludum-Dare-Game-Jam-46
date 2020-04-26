using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeraudio : MonoBehaviour
{
    AudioSource whistleSound;
    public static bool isWhistling;

    // Start is called before the first frame update
    void Start()
    {
        whistleSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isWhistling = whistleSound.isPlaying;
        if (Input.GetKeyDown("space") && !isWhistling) //when the player whistles, audio
        {
            whistleSound.Play();
        }
    }
}
