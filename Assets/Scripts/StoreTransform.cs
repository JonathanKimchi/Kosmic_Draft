using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTransform //: MonoBehaviour
{
    // Start is called before the first frame update
    /*public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;*/
    public int storeFrame;//Stores the frame of the Keyframe.
    //public Transform transforming;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
    public float keySecond;
    public int fps = 24;


    public StoreTransform()
    {

    }
    public void convertFrame()
    {
        keySecond = (float)storeFrame / fps;
    }

    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    /*public static StoreTransform SaveLocal(this Transform aTransform)
    {
       temporary 
    }*/
}
