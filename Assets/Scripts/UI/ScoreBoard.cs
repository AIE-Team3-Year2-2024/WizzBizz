using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScoreBoard : Menu
{
    [SerializeField, Tooltip("the amount of time the players will have to wait before they can skip the scoreboard")]
    private float _beforeButtonWaitTime;

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

        StopCoroutine(AllowButtonAfterTime());

        _buttonSlider.maxValue = _buttonHoldTime;
        _buttonSlider.minValue = 0;

        StartCoroutine(AllowButtonAfterTime());

    }

    public IEnumerator AllowButtonAfterTime()
    {
        yield return new WaitForSecondsRealtime(_beforeButtonWaitTime);

        if (_menuManager) _menuManager._primaryController.currentActionMap.FindAction("Submit").started += ControlButtonHold;
        if (_menuManager) _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled += ControlButtonHold;

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
        if (_buttonSlider.gameObject.activeInHierarchy && _shouldReturn)
        {
            _currentButtonHoldTime += Time.unscaledDeltaTime;
            _buttonSlider.value = _currentButtonHoldTime;

            if (_currentButtonHoldTime >= _buttonHoldTime)
            {
                _menuManager._primaryController.currentActionMap.FindAction("Submit").started -= ControlButtonHold;
                _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled -= ControlButtonHold;
                _shouldReturn = false;
                GameManager.Instance.GoToNextRound();
            }
        }
    }

    private void OnDestroy()
    {
        _menuManager._primaryController.currentActionMap.FindAction("Submit").started -= ControlButtonHold;
        _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled -= ControlButtonHold;
    }

    private void OnDisable()
    {
        _menuManager._primaryController.currentActionMap.FindAction("Submit").started -= ControlButtonHold;
        _menuManager._primaryController.currentActionMap.FindAction("Submit").canceled -= ControlButtonHold;
    }
}
