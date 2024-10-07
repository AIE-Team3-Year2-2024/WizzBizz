using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    public CanvasGroup joinText;
    public CanvasGroup playerSelect;
    public CanvasGroup readyOverlay;
    public CanvasGroup selectArrows;
    public CanvasGroup options;

    [HideInInspector] public int _playerID = -1;
    [HideInInspector] public bool _playerJoined = false;
    [HideInInspector] public bool _playerReady = false;

    [HideInInspector] public int _selectedCharacterIndex = -1;

    [HideInInspector]
    public MultiplayerEventSystem _controllerEventSystem = null;

    public void OnEnable()
    {
        Start();
    }

    public void Start()
    {
        if (!joinText || !playerSelect || !selectArrows || !options)
        {
            Debug.LogError("Player slot has not been setup! (" + gameObject.name + ")");
            return;
        }

        _playerJoined = false;
        joinText.alpha = 1.0f;
        joinText.interactable = true;
        playerSelect.alpha = 0.0f;
        playerSelect.interactable = false;
        readyOverlay.alpha = 0.0f;
        readyOverlay.interactable = false;
        selectArrows.alpha = 0.0f;
        selectArrows.interactable = false;
        options.gameObject.SetActive(false);

        playerSelect.GetComponentInChildren<PortraitsAnchor>()._playerSelect = playerSelect;
    }

    public void JoinPlayer(MultiplayerEventSystem mm)
    {
        if (_playerJoined == true)
            return;
        
        //Debug.Log("ANOTHER JOINED TEST");
        _controllerEventSystem = mm;

        joinText.alpha = 0.0f;
        joinText.interactable = false;
        playerSelect.alpha = 1.0f;
        playerSelect.interactable = true;
        selectArrows.alpha = 1.0f;
        selectArrows.interactable = true;
        options.gameObject.SetActive(true);

        mm.SetSelectedGameObject(playerSelect.gameObject);

        _playerJoined = true;
    }

    public void LeavePlayer()
    {
        _playerJoined = false;
        _playerReady = false;
        _controllerEventSystem = null;
        _playerID = -1;
        
        joinText.alpha = 1.0f;
        joinText.interactable = true;
        playerSelect.alpha = 0.0f;
        playerSelect.interactable = false;
        selectArrows.alpha = 0.0f;
        selectArrows.interactable = false;
        options.gameObject.SetActive(false);
    }

    public void ReadyPlayer(bool isReady = true)
    {
        if (isReady == true)
        {
            _playerReady = true;
            readyOverlay.alpha = 1.0f;
            readyOverlay.interactable = true;
            playerSelect.interactable = false;
            selectArrows.interactable = false;
            
            _controllerEventSystem.SetSelectedGameObject(readyOverlay.GetComponentInChildren<Button>().gameObject); // TODO: Fix this. Make it a parameter or something.
        }
        else
        {
            _playerReady = false;
            readyOverlay.alpha = 0.0f;
            readyOverlay.interactable = false;
            playerSelect.interactable = true;
            selectArrows.interactable = true;
            
            _controllerEventSystem.SetSelectedGameObject(playerSelect.gameObject);
        }
    }

}
