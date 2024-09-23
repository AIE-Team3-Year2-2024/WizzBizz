using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseScreen : MonoBehaviour
{
    [SerializeField, Tooltip("this will be the defualt button to hover over")]
    private GameObject _firstSelected;

    public void Start()
    {
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }
}
