using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WinScreen : Menu
{
    [Header("Win Screen stuff - ")]
    [Tooltip("the array of images where the winners will be displayed MUST BE IN ORDER FROM WINNER TO FOURTH PLACE"), SerializeField]
    private RawImage[] _winnerImages;

    [Tooltip("the render texture of the raccoon"), SerializeField]
    private RenderTexture _raccoonRenderTexture;

    [Tooltip("the render texture of the penguin"), SerializeField]
    private RenderTexture _penguinRenderTexture;

    [Tooltip("the render texture of the lizard"), SerializeField]
    private RenderTexture _lizardRenderTexture;

    [Tooltip("the render texture of the frog"), SerializeField]
    private RenderTexture _frogRenderTexture;

    [Tooltip("how long this scene will show up for"), SerializeField]
    private float _sceneTime;

    [Tooltip("the scene to be loaded after this scene is done"), SerializeField]
    private string _afterWinScene;

    [Tooltip("the slider component showing visualaly how far the button has been held")]
    [SerializeField]
    private Slider _buttonSlider;

    [Tooltip("how long the button has to be held before this scene is skipped")]
    [SerializeField]
    private float _buttonHoldTime;

    [Tooltip("how long the button has currently been held for")]
    private float _currentButtonHoldTime = 0;

    private bool _shouldReturn = false;

    // Start is called before the first frame update
    public void OnEnable()
    {
        List<PlayerData> _gameWonData = GameManager.Instance.GetSortedPlayerData();
        int count = 0;
        for(int i = 0; i < _gameWonData.Count; i++)
        {
            if (_winnerImages[i] == null)
            {
                i += 999;
                break;
            }
            count++;
            switch(_gameWonData[i].characterSelect.name)
            {
                case ("Raccoon"):
                    {
                        _winnerImages[i].texture = _raccoonRenderTexture;
                        break;
                    }
                case ("Penguin"):
                    {
                        _winnerImages[i].texture = _penguinRenderTexture;
                        break;
                    }
                case ("Lizard"):
                    {
                        _winnerImages[i].texture = _lizardRenderTexture;
                        break;
                    }
                case ("Frog"):
                    {
                        _winnerImages[i].texture = _frogRenderTexture;
                        break;
                    }
            }
        }

        for (int i = count; i < 4; i++)
        {
            Destroy(_winnerImages[i].gameObject);
        }

        _buttonSlider.maxValue = _buttonHoldTime;
        _buttonSlider.minValue = 0;
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

        if (_menuManager) _menuManager._controllerCancelCallback += OnControllerCancel;

        _menuManager._primaryController.currentActionMap.FindAction("Submit").started += ControlButtonHold;
        _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled += ControlButtonHold;

        StartCoroutine(LoadScene());
    }

    public void ControlButtonHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _shouldReturn = true;
            _buttonSlider.gameObject.SetActive(true);
        }
        if (context.canceled)
        {
            _buttonSlider.gameObject.SetActive(false);
            _shouldReturn = false;
            _currentButtonHoldTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_buttonSlider.gameObject.activeInHierarchy && _shouldReturn)
        {
            _currentButtonHoldTime += Time.unscaledDeltaTime;
            _buttonSlider.value = _currentButtonHoldTime;

            if(_currentButtonHoldTime >= _buttonHoldTime)
            {
                _menuManager.FadeToScene(_afterWinScene);
                _menuManager._controllerCancelCallback -= OnControllerCancel;
                _menuManager._primaryController.currentActionMap.FindAction("Submit").started -= ControlButtonHold;
                _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled -= ControlButtonHold;
                _shouldReturn = false;
            }
        }
    }

    public void OnControllerCancel(PlayerInput controller)
    {
        // Override controller cancel.
        return;
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(_sceneTime);
        _menuManager.FadeToScene(_afterWinScene);
        _menuManager._controllerCancelCallback -= OnControllerCancel;

        _menuManager._primaryController.currentActionMap.FindAction("Submit").started -= ControlButtonHold;
        _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled -= ControlButtonHold;
    }
}
