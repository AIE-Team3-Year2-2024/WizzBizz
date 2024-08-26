using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillBoard : MonoBehaviour
{
    Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void Start()
    {
        //This gets the main camera from the scene
        if(Camera.main != null)
        {
            camera = Camera.main;
            //This enables main camera
            camera.enabled = true;
        }
    }

    // Setting the UI element's rotation to match the rotation of the camera and the UI's position to above the parent object.
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }
}
