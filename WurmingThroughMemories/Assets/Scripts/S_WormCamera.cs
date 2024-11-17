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
        if (isLerping)
        {
            LerpCameraStates();
            lerpTimer += Time.deltaTime;
        }
        if (lerpTimer > lerpMaxTime)
        {
            isLerping = false;
            lerpTimer = 0;
            transform.eulerAngles=NormalizeRotationAngles(transform.eulerAngles);
        }
    }


    public void startLerpCamera(Vector3 newcamerapos, Vector3 newcamerarot, float lerptime)
    {
        Debug.Log("SWDQ" + newcamerarot + transform.eulerAngles);
        lerpMaxTime = lerptime;

        lerpstartrot = transform.eulerAngles;

        lerpendrot = newcamerarot;

        lerpstartpos = camerapos;
        lerpendpos = newcamerapos;
        isLerping = true;
    }
    private void LerpCameraStates()
    {
        Vector3 currentAngle = Lerp3(lerpstartrot, lerpendrot, lerpTimer / lerpMaxTime);
        Vector3 currentPos = Lerp3(lerpstartpos, lerpendpos, lerpTimer / lerpMaxTime);

        // this.transform.eulerAngles = currentAngle;
        transform.eulerAngles = new Vector3(currentAngle.x, currentAngle.y, currentAngle.z);
        Debug.Log(lerpTimer / lerpMaxTime);
        Debug.Log(currentAngle);


        camerapos = currentPos;

    }
    float Lerp1(float firstFloat, float secondFloat, float by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }
    Vector3 Lerp3(Vector3 firstVector, Vector3 secondVector, float by)
    {
        float retX = Lerp1(firstVector.x, secondVector.x, by);
        float retY = Lerp1(firstVector.y, secondVector.y, by);
        float retZ = Lerp1(firstVector.z, secondVector.z, by);
        return new Vector3(retX, retY, retZ);
    }

    float NormalizeAngle(float angle)
    {
        if(angle < 0)
        {
            angle = Mathf.Abs(angle);
            angle = 360 - angle;
        }
        float normalizedAngle = angle % 360;
        if (normalizedAngle < 0)
        {
            normalizedAngle += 360;
        }
        return normalizedAngle;
    }

    Vector3 NormalizeRotationAngles(Vector3 rotation)
    {
        float rotX = NormalizeAngle(rotation.x);
        float rotY = NormalizeAngle(rotation.y);
        float rotZ = NormalizeAngle(rotation.z);
        //return new Vector3(rotX, rotY, rotZ);
        return rotation;
    }
}
