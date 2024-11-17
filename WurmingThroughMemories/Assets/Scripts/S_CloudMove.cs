using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_CloudMove : MonoBehaviour
{
    public Vector3 dir;
    public float speed;
    public float max_Dist;
    private Vector3 og_pos;
    // Start is called before the first frame update
    void Start()
    {
        og_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(og_pos, transform.position) > max_Dist)
        {
            transform .position = og_pos + -dir * speed*5;
        }
    }
}
