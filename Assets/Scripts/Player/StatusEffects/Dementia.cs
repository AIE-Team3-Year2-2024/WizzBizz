using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dementia : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime = 0;

    [Tooltip("the object wich contains all of the players UI")]
    [SerializeField]
    private GameObject _UIParent;
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
        _UIParent.SetActive(false);
    }

    private void OnDisable()
    {
        _UIParent.SetActive(true);
    }
}
