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
        selectArrows.alpha = 0.0f;
        selectArrows.interactable = false;
        options.gameObject.SetActive(false);

        playerSelect.GetComponentInChildren<PortraitsAnchor>()._playerSelect = playerSelect;
    }

    public void JoinPlayer(MultiplayerEventSystem mm)
    {
        if (_playerJoined == true)
            return;
        
        Debug.Log("ANOTHER JOINED TEST");
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

}
