using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintTime : MonoBehaviour
{
    // Start is called before the first frame update
    Text timeUpdate;
    SerialStore timeObjective;
    void Start()
    {
        timeUpdate = GetComponent<Text>();
        GameObject temp = GameObject.Find("StoreObject");
        timeObjective = temp.GetComponent<SerialStore>();
    }

    // Update is called once per frame
    void Update()
    {
        timeUpdate.text = timeObjective.currentTime.ToString(); 
    }
}
