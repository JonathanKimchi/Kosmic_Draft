using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBVH : MonoBehaviour
{
    // Start is called before the first frame update
    public BVHRecorder recorder;
    public SerialStore storeRecord;
    void Start()
    {
        recorder = this.GetComponent<BVHRecorder>();
        GameObject temp = GameObject.Find("StoreObject");
        storeRecord = temp.GetComponent<SerialStore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        storeRecord.PlayRecord();
    }

}
