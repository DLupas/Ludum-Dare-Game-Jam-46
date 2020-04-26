using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMusic : MonoBehaviour
{
    AudioSource source;
    public AudioClip normal;
    public AudioClip intense;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    public void normalMusic()
    {
        source.clip = normal;
        source.Play();
    }
    public void intenseMusic()
    {
        source.clip = intense;
        source.Play();
    }
}
