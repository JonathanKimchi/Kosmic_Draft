using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindButton : MonoBehaviour
{
    // Start is called before the first frame update
    SerialStore storageObject;
    void Start()
    {
        GameObject storageTemp = GameObject.Find("StoreObject");
        storageObject = (SerialStore)storageTemp.GetComponent(typeof(SerialStore));

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        storageObject.Rewind();
    }
}
