using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInArc : MonoBehaviour
{
    private Vector3 startPos;

    private Vector3 endPos;

    public float speed;

    private float arcCompletion = 0;

    public float heightMultiplyer;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        arcCompletion += Time.deltaTime * speed;
        if (arcCompletion < 1)
        {
            transform.position = Vector3.Slerp(startPos, endPos, arcCompletion);
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(arcCompletion * Mathf.PI) * heightMultiplyer, transform.position.z);
        }
    }

    public void SetEndPos(Vector3 inputTransform)
    {
        endPos = inputTransform;
    }
}
