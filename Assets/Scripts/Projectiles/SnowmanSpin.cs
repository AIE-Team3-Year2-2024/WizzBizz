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

    // Start is called before the first frame update
    void Start()
    {
        _triggerRadius = _trigger.radius;
    }

    // Update is called once per frame
    void Update()
    {
        float currentMult = 0;

        foreach(CharacterBase player in _hitPlayers)
        {
            currentMult += _triggerRadius - Vector3.Distance(transform.position, player.transform.position);
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (currentMult * _speed), transform.rotation.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _hitPlayers.Add(other.GetComponent<CharacterBase>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _hitPlayers.Remove(other.GetComponent<CharacterBase>());
        }
    }
}
