using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Tooltip("The curve that dictates the smoothing of the transition.")]
    public AnimationCurve transitionCurve;
    [Tooltip("How long it will take for the menus to fully transition.")]
    public float transitionDuration = 1.5f;

    [Tooltip("A reference to the canvas group that holds the fade effect.")]
    public CanvasGroup fadeCanvas;
    [Tooltip("How long it will take to fade.")]
    public float fadeDuration = 3.0f;

    private Menu _activeMenu = null;
    private Menu _targetMenu = null;
    private Menu _lastActiveMenu = null;

    private List<Menu> menus = new List<Menu>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializeManager();

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0.0f;
        }
    }

    void InitializeManager()
    {
        PopulateMenus();

        Menu firstMenu = null;
        SceneInfo info = FindObjectOfType<SceneInfo>();
        if (info != null)
        {
            firstMenu = info.firstMenu;
        }

        foreach (Menu m in menus)
        {
            if (m.gameObject.activeInHierarchy == false)
            {
                m.gameObject.SetActive(true);
                m.Awake(); // Make sure this is called so that the component gets its required references since awake isn't called when the game object itself is disabled.
            }

            m._menuManager = this;
            m._canvasGroup.interactable = false;
            m._canvasGroup.blocksRaycasts = true;

            m.gameObject.SetActive(false);
        }

        if (firstMenu != null && menus.Contains(firstMenu))
        {
            _activeMenu = firstMenu;
            _activeMenu._canvasGroup.interactable = true;
            _activeMenu._canvasGroup.blocksRaycasts = false;
            _activeMenu.gameObject.SetActive(true);

            if (_activeMenu.firstSelected != null)
                EventSystem.current.SetSelectedGameObject(_activeMenu.firstSelected.gameObject);
        }
    }

    public void SetTargetMenu(Menu menuObj)
    {
        if (menuObj == null || menuObj == _activeMenu)
            return;

        _targetMenu = menuObj;
        _lastActiveMenu = _activeMenu;

        _activeMenu._lastSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();

        switch (_activeMenu.transitionDirection)
        {
            case Menu.MenuTransitionDirection.MENU_MOVE_LEFT:
            case Menu.MenuTransitionDirection.MENU_MOVE_RIGHT:
            {
                int direction = _activeMenu.transitionDirection == Menu.MenuTransitionDirection.MENU_MOVE_LEFT ? -1 : 1;
                TransitionHorizontalToTarget(_activeMenu, _targetMenu, direction);
            } break;
            // TODO: Vertical transitions.
        }
    }

    public void GoBack()
    {
        SetTargetMenu(_lastActiveMenu);
    }

    public void FadeToScene(string sceneName)
    {
        if (fadeCanvas == null || sceneName.Length <= 0)
            return;

        _activeMenu._canvasGroup.interactable = false;
        _activeMenu._canvasGroup.blocksRaycasts = true;

        Tween.CanvasGroupAlpha(fadeCanvas, 0.0f, 1.0f,
            fadeDuration, 0.0f, transitionCurve, Tween.LoopType.None, 
            () => { /* Unused. */ }, // Start callback.
            () => { 
                _activeMenu.gameObject.SetActive(false);
                _activeMenu = null;
                _lastActiveMenu = null;
                _targetMenu = null;

                StartCoroutine(LoadSpecifiedScene(sceneName));
            }, // Complete callback. 
            false);
    }

    private void TransitionHorizontalToTarget(Menu currentMenu, Menu nextMenu, int direction)
    {
        if (currentMenu == null || nextMenu == null)
            return;

        currentMenu._canvasGroup.interactable = false;
        currentMenu._canvasGroup.blocksRaycasts = true;
        
        float tweenDuration = transitionDuration * 0.5f; // Since it'll do two tweens for each menu technically.
        Tween.LocalPosition(currentMenu.anchorObject, Vector3.zero, new Vector3(currentMenu._canvasReferenceResolution.x * direction, 0, 0),
            tweenDuration, 0.0f, transitionCurve, Tween.LoopType.None, 
            () => { /* Unused. */ }, // Start callback.
            () => { 
                currentMenu.gameObject.SetActive(false);
                nextMenu.gameObject.SetActive(true);

                Tween.LocalPosition(nextMenu.anchorObject, new Vector3(nextMenu._canvasReferenceResolution.x * direction, 0, 0), Vector3.zero,
                    tweenDuration, 0.0f, transitionCurve, Tween.LoopType.None, 
                    () => { /* Unused. */ }, // Start callback.
                    () => {
                        _activeMenu = _targetMenu;
                        _targetMenu = null;

                        nextMenu._canvasGroup.interactable = true;
                        nextMenu._canvasGroup.blocksRaycasts = false;

                        Selectable selectedObject = nextMenu._lastSelected == null ? nextMenu.firstSelected : nextMenu._lastSelected;
                        EventSystem.current.SetSelectedGameObject(selectedObject.gameObject);
                    }, // Complete callback. 
                    false);
            }, // Complete callback.
            false);
    }

    private void PopulateMenus()
    {
        menus.Clear();
        Menu[] arr = FindObjectsByType<Menu>(FindObjectsSortMode.None);
        menus.AddRange(arr);
    }

    IEnumerator LoadSpecifiedScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene Loaded: " + sceneName);

        InitializeManager();

        if (fadeCanvas.alpha >= 1.0f)
        {
            Tween.CanvasGroupAlpha(fadeCanvas, 1.0f, 0.0f,
                fadeDuration, 0.0f, transitionCurve, Tween.LoopType.None, 
                () => { /* Unused. */ }, // Start callback.
                () => { /* Unused. */ }, // Complete callback.
                false);
        }
    }
}
