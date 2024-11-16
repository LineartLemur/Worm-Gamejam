using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class S_ScarEvent1 : MonoBehaviour
{
    public GameObject worm;
    private bool showingPicture;
    private float maxTimer = 3.0f;
    private float timer = 0.0f;
    public GameObject pictureToShow;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(showingPicture)
        {
            timer += Time.deltaTime;
        }
        if(timer > maxTimer)
        {
            EndEvent();
        }
    }

    public void StartEvent()
    {
       // worm.GetComponent<S_WormMovement>().PauseMovement();
        ShowPicture();
        showingPicture = true;
        worm.GetComponent<S_WormMovement>().PauseMovement();
    }
    private void ShowPicture()
    {
        pictureToShow.gameObject.SetActive(true);
    }
    private void EndEvent()
    {
        pictureToShow.gameObject.SetActive(false);
        worm.GetComponent <S_WormMovement>().StartMovement();
    }
}
