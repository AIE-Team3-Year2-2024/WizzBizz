using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime;

    private CharacterBase _player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0 )
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if(_player == null)
        {
            _player = GetComponent<CharacterBase>();
        }
        _player.confused = true;
    }

    private void OnDisable()
    {
        _player.confused = false;
    }
}
