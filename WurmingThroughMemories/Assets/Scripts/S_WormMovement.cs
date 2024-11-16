using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
public class S_WormMovement : MonoBehaviour
{
    public SplineAnimate splineAnimate;

    public AudioSource wormSound;
    private void Start()
    {
        StartMovement();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseMovement();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartMovement();
        }
       
    }

    public void PauseMovement()
    {
        splineAnimate.Pause();
        wormSound.Stop();
    }
    public void StartMovement()
    {
        splineAnimate.Play();
        wormSound.Play();
    }
}

