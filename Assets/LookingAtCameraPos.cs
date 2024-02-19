using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtCameraPos : MonoBehaviour
{

    //[SerializeField]
    Transform targetToLook; //which target are we gonna be looking at

    // Start is called before the first frame update
    void Start()
    {
        //automatically set up the main camera as the target to look at
        //main camera is based on which one is Tagged "Main Camera"
        targetToLook = Camera.main.transform;

    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(targetToLook);//changes rotation of transform to keep looking at target
    }
}
