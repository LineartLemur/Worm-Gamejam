using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class S_ScarTrigger : MonoBehaviour
{
    
    public enum Result {
        Event,
        Story
    }

    public Result result;

    public bool isStory => result == Result.Story;
    [HideIf(nameof(isStory))]
    public UnityEvent enterEvent;
    
    [ShowIf(nameof(isStory)), AssetsOnly]
    public S_RunStory prefab;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("s");
        if (other.tag == "worm") {

            Debug.Log("xx");
            if (isStory) {
                Instantiate(prefab);
            } else {
                
                enterEvent.Invoke();
            }
        }
    }


}
