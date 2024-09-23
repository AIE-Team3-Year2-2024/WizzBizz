using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject firstMenu;

    public List<GameObject> menus = new List<GameObject>();

    private GameObject _activeMenu;
    private GameObject _targetMenu;
    private GameObject _lastActiveMenu;

    void Start()
    {
        if (firstMenu != null && menus.Contains(firstMenu))
        {
            foreach (GameObject m in menus)
                m.SetActive(false);
            firstMenu.SetActive(true);

            _activeMenu = firstMenu;
        }
    }

    void Update()
    {
        
    }
}
