using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncesBeforeDestroy : MonoBehaviour
{
    [Tooltip("how many bounces this object will take before it destroys itself")]
    [SerializeField]
    private int _deathBounceAmount;

    private int _bounces = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_bounces < _deathBounceAmount || _deathBounceAmount == 0)
        {
            _bounces++;
            Vector3 refection = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            transform.forward = new Vector3(refection.x, 0, refection.z);
        }
        else if(_deathBounceAmount != 0)
        {
            Destroy(gameObject);
        }
    }
}
