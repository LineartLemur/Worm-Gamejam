using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_WormCamera : MonoBehaviour
{
    public GameObject worm;
    private Vector3 cameraoffset;
    // Start is called before the first frame update
    void Start()
    {
        cameraoffset = transform.position - worm.GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = worm.GetComponent<Transform>().position + cameraoffset;
    }
}
