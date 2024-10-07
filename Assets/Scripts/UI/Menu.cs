using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
    [Tooltip("This is the transform that'll move the whole menu for transitions. (Default position of this object should be set to zero)")]
    public Transform anchorObject;

    [Tooltip("Which way the menu will transition towards when it is being swapped out.")]
    public MenuTransitionDirection transitionDirection = MenuTransitionDirection.MENU_MOVE_LEFT;

    public List<Button> menuButtons = new List<Button>();
    public Selectable firstSelected = null;

    [HideInInspector] public Selectable _lastSelected;

    [HideInInspector] public Vector2 _canvasReferenceResolution = Vector2.zero;
    [HideInInspector] public CanvasGroup _canvasGroup = null;
    [HideInInspector] public MenuManager _menuManager = null; // MenuManager is a singleton but this is probably safer idk.

    public enum MenuTransitionDirection
    {
        MENU_MOVE_LEFT,
        MENU_MOVE_RIGHT
    }

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        CanvasScaler cs = GetComponentInChildren<CanvasScaler>();
        if (cs)
            _canvasReferenceResolution = cs.referenceResolution;

        _lastSelected = firstSelected;
    }

    // Wrapper functions so we don't have to rely on a MenuManager being in the scene.
    public void GoBackScene()
    {
        if (_menuManager == null)
        {
            Debug.LogError("Menu Manager is null on Menu: " + gameObject.name);
            return;
        }
        _menuManager.GoBackScene();
    }

    public void GoBackMenu()
    {
        if (_menuManager == null)
        {
            Debug.LogError("Menu Manager is null on Menu: " + gameObject.name);
            return;
        }
        _menuManager.GoBackMenu();
    }

}
