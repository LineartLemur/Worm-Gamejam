using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class S_CameraTrigger : MonoBehaviour
{

    public S_WormCamera cam;
    public Vector3 WormCameraOffset;
    public Vector3 WormCameraRotation;

    private void OnTriggerEnter(Collider other)
    {
   
        if (other.tag == "worm") {

            cam.startLerpCamera(WormCameraOffset, WormCameraRotation);

        }
    }


}
