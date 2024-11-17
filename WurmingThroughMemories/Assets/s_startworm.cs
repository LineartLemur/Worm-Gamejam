using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class s_startworm : MonoBehaviour
{
    public SplineAnimate sp;
    // Start is called before the first frame update
    void Start()
    {
        sp.Play();
    }

    // Update is called once per frame
    void Update()
    {
        sp.Play();
    }
}
