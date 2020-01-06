using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScript : MonoBehaviour
{
    // Start is called before the first frame update
    /*public StoreTransform[] targets;
    public StoreTransform[] targetFiltered;
    int targetTotalLength = 0;
    int targetTracker = 0;
    public AnimationCurve lerpCurve;
    public Vector3 lerpOffset;
    public float frameLength = 0;
    public float lerpTime = 0;
    //public int playState = 0;
    //public int subSetLength = 0;
    //public float timer = 0;
    public BasicPlayScript basicTest;
    void Start()
    {
        basicTest = GetComponent<BasicPlayScript>();
        targets = GetComponent<SerialAdd>().transformStorage;
    }
    public void FilterTargets()
    {
        int lengthTemp = targets.Length;
        for(int i = 0;i<lengthTemp;i++)
        {
            if (targets[i]!=null)//check
            {
                targetTotalLength++;
            }
        }
        targetFiltered = new StoreTransform[targetTotalLength];
        for (int i = 0; i < lengthTemp; i++)
        {
            if (targets[i]!=null)
            {
                targetFiltered[targetTracker] = targets[i];
                targetTracker++;
            }
        }
        targetTracker = 0;
    }
    //public void calculateCurve(
    public float CalcFrameLength(int fps)
    {
        float tempTime = 1;
        frameLength = (float)(tempTime / fps);
        return frameLength;
    }//calculates how long a frame would be in seconds  
    public float CalcTimeTotal(int fps, int totalFrames)
    {
        lerpTime = (float)(totalFrames / fps);
        return lerpTime;
    }//calculates the time needed to execute.
    // Update is called once per frame
    public void PlayerPlayer()
    {
        FilterTargets();
        PlayerCalc(1);
    }
    public void PlayerCalc(float timer1)
    {
        if (targetTracker < targetTotalLength - 1)
        {
            basicTest.targetA = targetFiltered[targetTracker].transforming;
            basicTest.targetB = targetFiltered[targetTracker + 1].transforming;
            int frameDelta = targetFiltered[targetTracker + 1].storeTime - targetFiltered[targetTracker].storeTime;
            basicTest.frameLength = CalcFrameLength(24);
            basicTest.lerpTime = (float)CalcTimeTotal(24, frameDelta);
            targetTracker++; 
            basicTest.PlayerStart();
        }
    }
    public void PlayerRewind()
    {
        targetTracker = 0;
    }
    void Update()
    {
        /*if (playState != 0)
        {
            if (targetTracker == targetTotalLength-1)
            {
                timer += Time.deltaTime;
                if(timer>=)
            }
        }
    }*/
}
