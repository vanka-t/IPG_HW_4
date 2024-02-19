using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtCameraDir : MonoBehaviour
{
    Transform targetToLook;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //keep sanity bar static facing camera
        transform.rotation = Quaternion.LookRotation(-targetToLook.forward, Vector3.up);
        
    }
}
