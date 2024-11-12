using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveInArc : MonoBehaviour
{
    private Vector3 startPos;

    private Vector3 endPos;

    [Tooltip("how fast this object will move along the arc")]
    public float speed;

    private float arcCompletion = 0;

    [Tooltip("decides how high the arc will be")]
    public float heightMultiplyer;

    [Tooltip("event to be invoked when this object is destroyed")]
    [SerializeField]
    private UnityEvent doOnDestroy;

    [Tooltip("another lobbed object that this object will make when it is destroyed(if empty will not make any object on destroy)")]
    [SerializeField]
    private MoveInArc _arcObjectToMake;

    [Tooltip("the distance from the player and this objects end position used to make the next move in arc object")]
    [HideInInspector]
    public float _lifeDistance;

    void Start()
    {
        startPos = transform.position;
    }

    /// <summary>
    /// moves this object by a timer along a curve made by the height multiplyer and destroys it at the end of the curve
    /// </summary>
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
        if(_arcObjectToMake != null)
        {
            MoveInArc madeObject = Instantiate(_arcObjectToMake, transform.position, transform.rotation);
            madeObject.SetEndPos(transform.position + (transform.forward * _lifeDistance));
        }
        doOnDestroy.Invoke();
    }
}
