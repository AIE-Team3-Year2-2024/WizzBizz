using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveInArc : MonoBehaviour
{
    private Vector3 startPos;

    private Vector3 endPos;

    public float speed;

    private float arcCompletion = 0;

    public float heightMultiplyer;

    [Tooltip("event to be invoked when this object is destroyed")]
    [SerializeField]
    private UnityEvent doOnDestroy;

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
            float t = 1.0f - Mathf.Cos((arcCompletion * Mathf.PI) / 2.0f); // https://easings.net/#easeInSine
            transform.position = Vector3.Slerp(startPos, endPos, t);
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(t * Mathf.PI) * heightMultiplyer, transform.position.z);
        } 
        else
        {
            transform.position = endPos;
            Destroy(gameObject);
        }
    }

    public void SetEndPos(Vector3 inputTransform)
    {
        endPos = inputTransform;
    }

    private void OnDestroy()
    {
        doOnDestroy.Invoke();
    }
}
