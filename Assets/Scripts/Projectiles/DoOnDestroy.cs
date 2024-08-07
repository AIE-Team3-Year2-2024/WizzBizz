using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoOnDestroy : MonoBehaviour
{
    public UnityEvent onDestroy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        Debug.Log("on destroy");
        onDestroy.Invoke();

    }
}
