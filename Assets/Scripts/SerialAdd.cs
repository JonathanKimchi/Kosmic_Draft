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
    public float rotYOld;
    public float rotZOld;//stores the old location, rotation of current object.  Helpful for Keyframing.
    public int currentTime;
    public StoreTransform[] transformStorage = new StoreTransform[10000];
    //GameObject currentObject;//I'm not even sure if this is necessary.
    void Start()
    {
        GameObject storageTemp = GameObject.Find("StoreObject");
        storageObject = (SerialStore)storageTemp.GetComponent(typeof(SerialStore));
        storageObject.addBone(gameObject.name);//not sure if you can pass by object.  You probably can, but just to be safe, ya know?

    }

    // Update is called once per frame
    void Update()
    {
        
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
        StoreTransform currentTransform = new StoreTransform
        {
            /*currentTransform.posX = posX;
            currentTransform.posY = posY;
            currentTransform.posZ = posZ;
            currentTransform.rotX = rotX;
            currentTransform.rotY = rotY;
            currentTransform.rotZ = rotZ;*/
            storeFrame = currentTime,
            position = gameObject.transform.localPosition
        };
        currentTransform.rotation = gameObject.transform.localRotation;
        currentTransform.localScale = gameObject.transform.localScale;
        currentTransform.convertFrame();
        transformStorage[currentTime] = currentTransform;
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
