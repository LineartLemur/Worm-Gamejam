using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
public class S_WormMovement : MonoBehaviour
{
    public SplineAnimate splineAnimate;
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
    }
    public void StartMovement()
    {
        splineAnimate.Play();
    }
}

