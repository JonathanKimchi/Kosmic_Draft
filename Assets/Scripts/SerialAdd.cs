using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialAdd : MonoBehaviour
{
    // Start is called before the first frame update.  Not sure if this is called upon object initialization or not.
    public SerialStore storageObject;
    public float posX;
    public float posY;
    public float posZ;
    //public Transform TemporaryStorageTest;
    public float rotX;
    public float rotY;
    public float rotZ;
    public float posXOld;
    public float posYOld;
    public float posZOld;
    public float rotXOld;
    public float rotYOld;//All of these variables are not yet being used.  They will be used once Serialization is implemented.
    public float rotZOld;//stores the old location, rotation of current object.  Helpful for Keyframing.
    public int currentTime;
    public StoreTransform[] transformStorage = new StoreTransform[10000];
    public int recordState = 0;
    public float keyTime;//The amount of time it takes for one frame to pass.
    public float passedTime;//Records how much time has passed since the last time a frame was recorded.  
    public BVHRecorder recorder;//used to store the BVHRecorder object.
    //GameObject currentObject;//I'm not even sure if this is necessary.
    public int currentRecordLiveFrame = 0;//Tracks the keyframe for live recording.  
    void Start()
    {
        GameObject storageTemp = GameObject.Find("StoreObject");
        storageObject = (SerialStore)storageTemp.GetComponent(typeof(SerialStore));
        storageObject.addBone(gameObject.name);//not sure if you can pass by object.  You probably can, but just to be safe, ya know?
        GameObject temp = GameObject.Find("Save");
        recorder = temp.GetComponent<BVHRecorder>();

    }

    // Update is called once per frame
    void Update()
    {
        if (recordState != 0)
        {
            passedTime += Time.deltaTime;
            if (passedTime >= keyTime)
            {
                currentRecordLiveFrame++;
                //currentRecordLiveFrame++;
                this.SaveLocRotLive(currentRecordLiveFrame);//saves a keyframe every keyTime seconds.
                passedTime = 0;//This is a flawed system.  In the future, see if you can implement a live record system that relies on solely the Current Time counter.  
            }
        }
    }
    public void LiveRecord()
    {
        keyTime = 1 / storageObject.fps;
        recorder.capturing = true;
        recordState = 1;
    }
    public void LiveStop()
    {
        recorder.capturing = false;
        recordState = 0;
    }
    public void SaveLocation()
    {
        //SimpleAttach test = new SimpleAttach();
        //StoreTransform currentLocation = new StoreTransform();
        posX = gameObject.transform.position.x;
        posY = gameObject.transform.position.y;
        posZ = gameObject.transform.position.z;
        currentTime = storageObject.currentTime;
        //TemporaryStorageTest = transform;
        transformStorage[currentTime] = GenerateTransform();
        //maybe don't create a new object, just because it would be harder to edit the variables in the long run?
        //nah.
    }
    public void SaveLocRot()
    {
        //StoreTransform currentLocation = new StoreTransform();
        posX = this.transform.position.x;
        posY = this.transform.position.y;
        posZ = this.transform.position.z;
        rotX = this.transform.rotation.x;
        rotY = this.transform.rotation.y;
        rotZ = this.transform.rotation.z;
        currentTime = storageObject.currentTime;
        GameObject baseLoc = GameObject.Find("BodyBase");//change this later to adapt to dynamic targeting
        StoreTransform currentTransform = new StoreTransform
        {
            /*currentTransform.posX = posX;
            currentTransform.posY = posY;
            currentTransform.posZ = posZ;
            currentTransform.rotX = rotX;
            currentTransform.rotY = rotY;
            currentTransform.rotZ = rotZ;*/
            storeFrame = currentTime,
            //position = gameObject.transform.localPosition
            position = baseLoc.transform.InverseTransformPoint(transform.position)
        };
        currentTransform.rotation = gameObject.transform.localRotation;
        currentTransform.localScale = gameObject.transform.localScale;
        currentTransform.convertFrame();
        transformStorage[currentTime] = currentTransform;
        //transformStorage[currentTime] = GenerateTransform();

    }
    public void SaveLocRotLive(int input1)//used to record SaveLocRot at a specific location at a specific time.  May need to develop a masking feature in the future.  
    {
        
        posX = this.transform.position.x;
        posY = this.transform.position.y;
        posZ = this.transform.position.z;
        rotX = this.transform.rotation.x;
        rotY = this.transform.rotation.y;
        rotZ = this.transform.rotation.z;
        GameObject baseLoc = GameObject.Find("BodyBase");//change this later to adapt to dynamic targeting.
        StoreTransform currentTransform = new StoreTransform
        {
            /*currentTransform.posX = posX;
            currentTransform.posY = posY;
            currentTransform.posZ = posZ;
            currentTransform.rotX = rotX;
            currentTransform.rotY = rotY;
            currentTransform.rotZ = rotZ;*/
            storeFrame = input1,
            //position = gameObject.transform.position,//this doesn't work because localPosition is calculated using the parenting formula.  The LocalPosition shifts to the position of the hand upon grabbing.  
            //position = transform.position-baseLoc.transform.position
            position = baseLoc.transform.InverseTransformPoint(transform.position)
            //position = transform.position - baseLoc.transform.position


        };
        //record the distance between the two objects as well as the distance as two separate things, then combine them to form a vector.  
        //float distanceTemp = Vector3.Distance(baseLoc.transform.position, transform.position);
        //Vector3 directionTemp = baseLoc.transform.InverseTransformDirection(transform.position);
        //currentTransform.position = directionTemp * distanceTemp;
        currentTransform.rotation = gameObject.transform.localRotation;
        currentTransform.localScale = gameObject.transform.localScale;
        currentTransform.convertFrame();
        transformStorage[input1] = currentTransform;
        //transformStorage[currentTime] = GenerateTransform();

    }
    public StoreTransform GenerateTransform()
    {
        StoreTransform currentTransform = new StoreTransform();
       /* currentTransform.posX = posX;
        currentTransform.posY = posY;
        currentTransform.posZ = posZ;
        currentTransform.rotX = rotX;
        currentTransform.rotY = rotY;
        currentTransform.rotZ = rotZ;
        currentTransform.storeTime = currentTime;
        currentTransform.transforming = transform;*/
        return currentTransform;
    }
}
