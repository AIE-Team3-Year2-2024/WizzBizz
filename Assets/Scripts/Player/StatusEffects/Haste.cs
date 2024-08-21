using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("how much will added to the players speed")]
    [SerializeField]
    private float _speedAdd;

    private CharacterBase _player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_player == null)
        {
            _player = GetComponent<CharacterBase>();
        }
        _player.AddSpeed(_speedAdd);
    }

    private void OnDisable()
    {
        _player.AddSpeed(-_speedAdd);
    }
}
