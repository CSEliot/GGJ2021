using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource movingSoundSource;
    public float maxVolume = 1;
    public float maxVeloxity = 10;
    Rigidbody body;

    void Start()
    {
        if(movingSoundSource == null)
        {
            movingSoundSource = GetComponentInChildren<AudioSource>();
        }

        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(body.angularVelocity.magnitude > 0 && movingSoundSource.isPlaying == false)
        {
            movingSoundSource.Play();
        }
        else if (body.angularVelocity.magnitude <= 0)
        {
            movingSoundSource.Stop();
        }

        //Debug.Log(body.angularVelocity.magnitude);
        float volumeLevel;
        volumeLevel = body.angularVelocity.magnitude / maxVeloxity;
        if(volumeLevel > 1)
        {
            volumeLevel = 1;
        }

        volumeLevel = maxVolume * volumeLevel;
        movingSoundSource.volume = volumeLevel;

    }
}
