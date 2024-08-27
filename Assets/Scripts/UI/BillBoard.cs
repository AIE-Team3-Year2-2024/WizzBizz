using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillBoard : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private float _sizeModifier;
    private float _cameraDistance;
    private RectTransform _canvasTransform;
    private Vector3 _tmpLocalScale = new Vector3();

    void Start()
    {
        _canvasTransform = GetComponent<RectTransform>();

        // This gets the main camera from the scene
        if (Camera.main != null)
        {
            mainCamera = Camera.main;
            // This enables main camera
            mainCamera.enabled = true;
        }

        // get distance between camera and 0,0,0. Use this info to calculate size modifier.
        _cameraDistance = Vector3.Distance(Camera.main.transform.position, Vector3.zero);
        _sizeModifier = _cameraDistance / _canvasTransform.localScale.x;
    }

    
    void Update()
    {
        // Adjusts UI rotation and position by world position.
        transform.rotation = mainCamera.transform.rotation;
        transform.position = target.position + offset;

        _cameraDistance = Vector3.Distance(Camera.main.transform.position, this.transform.position);
        _tmpLocalScale.x = _cameraDistance / _sizeModifier;
        _tmpLocalScale.y = _tmpLocalScale.x;
        _tmpLocalScale.z = _tmpLocalScale.x;

        // Apply new scale
        _canvasTransform.localScale = _tmpLocalScale;
        // rotate canvas to be perpendicular to camera regardless of camera rotation.
        _canvasTransform.rotation = Camera.main.transform.rotation;
    }
}
