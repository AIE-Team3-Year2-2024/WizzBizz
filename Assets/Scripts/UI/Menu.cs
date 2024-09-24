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
    [HideInInspector] public MenuManager _menuManager = null;

    public enum MenuTransitionDirection
    {
        MENU_MOVE_LEFT,
        MENU_MOVE_RIGHT
    }

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        CanvasScaler cs = GetComponentInChildren<CanvasScaler>();
        if (cs != null)
            _canvasReferenceResolution = cs.referenceResolution;

        _lastSelected = firstSelected;
    }

}
