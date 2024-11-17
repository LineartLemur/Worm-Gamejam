using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class S_NarrationEndGame : MonoBehaviour
{
    public TMP_Text text_block;
    public S_ConsistencyManager consistencyManager;
    public string introtext;
    public string[] CatScratchTexts;
    public string[] KneeScratchTexts;
    public string[] ChinScratchTexts;
    public string[] EyeScratchTexts;
    public string endtext;
    public RectTransform objecttransform;
    private string WholeText;

    public Vector3 BeginPos;
    public Vector3 EndPos;
    public float time = 20;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {

        EndPos = objecttransform.localPosition;
        objecttransform.localPosition = BeginPos;

        // WholeText = introtext + "\n" + CatScratchTexts[consistencyManager.CatScratchMode] + "\n" + KneeScratchTexts[consistencyManager.KneeScratchMode] + "\n" + ChinScratchTexts[consistencyManager.ChinScratchMode] + "\n" + EyeScratchTexts[consistencyManager.EyeScratchMode] + "\n" + endtext;
        WholeText = introtext;
        text_block.text = WholeText;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > timer)
        {
            timer += Time.deltaTime;
            MoveCam();
        }

      
    }

    public void MoveCam()
    {
        objecttransform.localPosition = Lerp3(BeginPos, EndPos, timer / time);
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
