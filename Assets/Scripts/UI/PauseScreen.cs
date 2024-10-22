using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseScreen : Menu
{
    [SerializeField, Tooltip("this will be the defualt button to hover over")]
    private GameObject _firstSelected;

    public override void Start()
    {
        base.Start();
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }
}
