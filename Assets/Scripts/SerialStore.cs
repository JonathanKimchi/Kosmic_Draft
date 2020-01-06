using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SerialStore : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] inputStorage = new GameObject[1000];
    //public string[] inputStorage = new string[1000];
    int inputTracker = 0;
    public int currentTime = 0;
    int previousTime = 0;
    int maxTime = 0;//tells the Max time allowed.
    int maxKeyFrame = 0;//tells max keyframe put in up to this point.  Used to allow scrubbing.  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (previousTime != currentTime && currentTime<=maxKeyFrame)
        {
            previousTime = currentTime;//Time has changed.  A time change function has been called. 
            //call the position update function for scrubbing position.
            for (int i = 0; i < inputTracker; i++)
            {
                inputStorage[i].GetComponent<BasicPlayScript>().PositionUpdate(currentTime,maxKeyFrame);
            }
        }
    }
    public void addBone(string input1)
    {

        //GameObject temp = GameObject.Find(input1);

        inputStorage[inputTracker] = GameObject.Find(input1);
        inputTracker++;
        Debug.Log(inputStorage[inputTracker - 1].name);
            //passing by reference == ref argument.
    }
    public void SaveLocRot()
    {
        for (int i = 0; i < inputTracker; i++)
        {
            inputStorage[i].GetComponent<SerialAdd>().SaveLocRot();
        }
        if (currentTime > maxKeyFrame)
        {
            maxKeyFrame = currentTime;
        }
    }
    public void PlayIt()
    {
        for (int i = 0; i < inputTracker; i++)
        {
            inputStorage[i].GetComponent<BasicPlayScript>().PlayerStart();
        }
    }
    public void PlayRecord()
    {
        for (int i = 0; i < inputTracker; i++)
        {
            inputStorage[i].GetComponent<BasicPlayScript>().RecordStart();
        }
    }
    public void Rewind()
    {
        for (int i = 0; i < inputTracker; i++)
        {
            //inputStorage[i].GetComponent<PlayScript>().PlayerRewind();
        }
    }
}
