using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class S_ScarTrigger : MonoBehaviour
{

    public UnityEvent enterEvent;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("s");
        if (other.tag == "worm") {
            Debug.Log("xx");
            enterEvent.Invoke();
        }
    }


}
