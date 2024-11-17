using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class s_cameraMoveEndGame : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 BeginPos;
    private Vector3 EndPos;
    public float time = 20;
    private float timer = 0;
    void Start()
    {
        EndPos = transform.position;
        transform.position = BeginPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(time > timer)
        {
            timer += Time.deltaTime;
            MoveCam();
        }
        
    }

    public void MoveCam()
    {
        transform.position = Lerp3(BeginPos, EndPos, timer/time);
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
}
