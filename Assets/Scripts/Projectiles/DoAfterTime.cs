using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoAfterTime : MonoBehaviour
{
    [Tooltip("how much much time will pass between the creation of this object and the inkovation of the event")]
    [SerializeField]
    private float _time;

    [Tooltip("the event to be invoked after the time has passed")]
    [SerializeField]
    private UnityEvent _afterTimeEvent;

    [Tooltip("whether this event will be reinvoked with the same time after its completion")]
    [SerializeField]
    private bool _looping;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InvokeAfterTime());
    }

    public IEnumerator InvokeAfterTime()
    {
        yield return new WaitForSeconds(_time);
        _afterTimeEvent.Invoke();
        if(_looping)
        {
            RestartEvent();
        }
    }

    public void RestartEvent()
    {
        StartCoroutine(InvokeAfterTime());
    }
}
