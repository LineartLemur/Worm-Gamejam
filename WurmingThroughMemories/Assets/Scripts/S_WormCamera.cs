using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

public class S_WormCamera : MonoBehaviour
{
    public GameObject worm;

 

    public float lerpTimer = 0;
    public float lerpMaxTime = 1.0f;
    public bool isLerping = false;



    private Vector3 lerpstartrot;
    private Vector3 lerpendrot;

    private Vector3 lerpstartpos;
    private Vector3 lerpendpos;
    // Start is called before the first frame update
    private Vector3 camerapos;

    // Update is called once per frame

    private void Start()
    {
        camerapos = transform.position - worm.GetComponent<Transform>().position;
    }
    void Update()
    {
        transform.position = worm.GetComponent<Transform>().position + camerapos;
        if(isLerping )
        {
            LerpCameraStates();
            lerpTimer += Time.deltaTime;
        }
        if(lerpTimer > lerpMaxTime )
        {
            isLerping = false;
            lerpTimer = 0;
        }
    }

   
    public void startLerpCamera(Vector3 newcamerapos, Vector3 newcamerarot)
    {
      
        lerpstartrot =transform.eulerAngles;
        lerpendrot = newcamerarot ;
        isLerping = true;
    }
    private void LerpCameraStates()
    {
        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(lerpstartrot.x, lerpendrot.x, Time.deltaTime/lerpMaxTime),
            Mathf.LerpAngle(lerpstartrot.y, lerpendrot.y, Time.deltaTime / lerpMaxTime),
            Mathf.LerpAngle(lerpstartrot.z, lerpendrot.z, Time.deltaTime / lerpMaxTime));
    
        this.transform.eulerAngles = lerpendrot;

    }
}
