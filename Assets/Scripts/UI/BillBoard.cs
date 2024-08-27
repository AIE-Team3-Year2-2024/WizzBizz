using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillBoard : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] private Vector2 maxSize;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void Start()
    {
        // This gets the main camera from the scene
        if(Camera.main != null)
        {
            mainCamera = Camera.main;
            // This enables main camera
            mainCamera.enabled = true;
        }
    }

    
    void Update()
    {
        // Adjusts UI rotation and position by world position.
        transform.rotation = mainCamera.transform.rotation;
        transform.position = target.position + offset;

        // Adjusts UI size by distance from main camera.
        Vector3 uiPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 uiSize = new Vector2(maxSize.x * uiPos.z, maxSize.y * uiPos.y);
        uiSize.x = Mathf.Clamp(uiSize.x, 0.0f, maxSize.x);
        uiSize.y = Mathf.Clamp(uiSize.y, 0.0f, maxSize.y);
        transform.localScale = uiSize;
    }
}
