using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanSpin : MonoBehaviour
{
    private List<CharacterBase> _hitPlayers = new List<CharacterBase>();

    [Tooltip("multiplyer for the spinning speed")]
    [SerializeField]
    private float _speed;

    [Tooltip("trigger to find players")]
    [SerializeField]
    private SphereCollider _trigger;

    private float _triggerRadius;

    /// <summary>
    /// get th radius of the collider
    /// </summary>
    void Start()
    {
        _triggerRadius = _trigger.radius;
    }

    /// <summary>
    /// rotate this object by a multiplyer found form the distances of all colliding player objects
    /// </summary>
    void Update()
    {
        float currentMult = 0;

        foreach(CharacterBase player in _hitPlayers)
        {
            currentMult += _triggerRadius - Vector3.Distance(transform.position, player.transform.position);
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (currentMult * _speed), transform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// store any colliding players
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _hitPlayers.Add(other.GetComponent<CharacterBase>());
        }
    }

    /// <summary>
    /// remove any player exiting this objects collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _hitPlayers.Remove(other.GetComponent<CharacterBase>());
        }
    }
}
