using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class CharacterMenu : Menu
{
    public GameObject controllerPrefab;
    
    public List<PlayerSlot> playerSlots = new List<PlayerSlot>();
    public List<GameObject> characterPrefabs = new List<GameObject>();

    [HideInInspector] public int _joinedPlayers = 0;
    [HideInInspector] public int _readyPlayers = 0;

    private bool _addingControllers = true;
    private List<int> _addedGamepadIDs = new List<int>();

    // TODO: Player leave lobby on controller button.
    // TODO: Handle going back to character select after everything has already been selected.
    
    public override void Start()
    {
        base.Start();
        _menuManager._controllerDisconnectCallback += ControllerDisconnect;
        _menuManager._controllerReconnectCallback += ControllerReconnect;
    }
    
    public void Update()
    {
        if (_addingControllers == false)
            return;
        
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (_addedGamepadIDs.Contains(i))
                continue;
            
            if (Gamepad.all[i].allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic))
            {   
                Debug.Log("TEST THING: " + i);
                _addedGamepadIDs.Add(i);
                JoinPlayer(i);
            }
        }
    }

    public void JoinPlayer(int gamepadID = 0)
    {
        Debug.Log("JOINED TEST");

        Debug.Log(playerSlots.Count);

        PlayerSlot slot = getNextAvailableSlot(playerSlots);
        if (slot != null)
        {
            Gamepad gamepad = null;
            MultiplayerEventSystem mm = null;
            if (gamepadID <= 0 && _menuManager.primaryController != null)
            {
                PlayerInput p = _menuManager.primaryController.GetComponent<PlayerInput>();
                gamepad = (Gamepad)p.devices[0];
                mm = _menuManager.primaryController.GetComponentInChildren<MultiplayerEventSystem>();
                mm.playerRoot = slot.gameObject;
            }
            else if (controllerPrefab != null)
            {
                gamepad = Gamepad.all[gamepadID]; // TODO: Not sure if this is reliable.
                if (gamepad == null)
                {
                    Debug.Log("No suitable gamepad found. " + gameObject.name);
                    return;
                }

                PlayerInput controller =
                    PlayerInput.Instantiate(controllerPrefab, controlScheme: "Gamepad", pairWithDevice: gamepad);
                controller.transform.SetParent(_menuManager.transform);
                controller.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                controller.onDeviceLost += _menuManager.ControllerDisconnect;
                controller.onDeviceRegained += _menuManager.ControllerReconnect;
                _menuManager.AddController(controller);
                mm = controller.GetComponentInChildren<MultiplayerEventSystem>();
                mm.playerRoot = slot.gameObject;
            }

            if (mm != null && gamepad != null)
            {
                slot._playerID = _joinedPlayers;
                _joinedPlayers++;
                
                GameManager.Instance.AddController(gamepad);
                slot.JoinPlayer(mm);
            }
            else
            {
                Debug.LogError("Could not find multiplayer event system or an available gamepad for player slot: " + slot.gameObject.name);
            }
        }
    }

    public void ReadyPlayer(PlayerSlot slot)
    {
        if (slot == null)
            return;
        if (playerSlots.Contains(slot) == false)
        {
            Debug.LogError("Slot " + slot.gameObject.name + " is not in the PlayerSlots list!");
            return;
        }
        
        if (slot._playerJoined == true && slot._playerReady == false)
        {
            slot.ReadyPlayer();
            _readyPlayers++;

            if (characterPrefabs != null && characterPrefabs.Count > 0 && 
                slot._selectedCharacterIndex > -1 && slot._selectedCharacterIndex < characterPrefabs.Count)
            {
                GameObject selectedCharacter = characterPrefabs[slot._selectedCharacterIndex];
                Debug.Log(selectedCharacter.name + ", " + slot._selectedCharacterIndex);
                GameManager.Instance.SetSelectedCharacter(slot._playerID, selectedCharacter);
            }
            else
            {
                Debug.LogError("Character Prefabs list has not been setup or the selected character is incorrect.");
                return;
            }

            // TODO: Check all players ready and start countdown instead of immediately starting the game.
            if (_readyPlayers >= _joinedPlayers)
            {
                _addingControllers = false;
                GameManager.Instance.StartGame();
            }
        }
    }

    public void UnreadyPlayer(PlayerSlot slot)
    {
        if (slot == null)
            return;
        if (playerSlots.Contains(slot) == false)
        {
            Debug.LogError("Slot " + slot.gameObject.name + " is not in the PlayerSlots list!");
            return;
        }

        if (slot._playerJoined == true && slot._playerReady == true)
        {
            slot.ReadyPlayer(false);
            _readyPlayers--;
            
            // TODO: Cancel countdown.
        }
    }

    public void ControllerDisconnect(PlayerInput controller)
    {
        PlayerSlot associatedSlot = null;
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i]._controllerEventSystem == controller.GetComponentInChildren<MultiplayerEventSystem>())
            {
                associatedSlot = playerSlots[i];
                break;
            }
        }

        if (associatedSlot != null)
        {
            GameManager.Instance.RemoveController(controller);
            UnreadyPlayer(associatedSlot);
            associatedSlot.LeavePlayer();
            _joinedPlayers--;
        }
    }
    
    public void ControllerReconnect(PlayerInput controller)
    {
        PlayerSlot slot = getNextAvailableSlot(playerSlots);
        if (slot != null)
        {
            Gamepad gamepad = Gamepad.all.Last(); // Really wish the event gave the device directly.
            MultiplayerEventSystem mm = controller.GetComponentInChildren<MultiplayerEventSystem>();

            if (mm != null && gamepad != null)
            {
                slot._playerID = _joinedPlayers;
                _joinedPlayers++;

                GameManager.Instance.AddController(gamepad);
                slot.JoinPlayer(mm);
            }
            else
            {
                Debug.LogError("Either gamepad or eventsystem returned null!");
            }
        }
        else
        {
            Debug.LogError("Slot returned null!");
        }
    }

    PlayerSlot getNextAvailableSlot(List<PlayerSlot> slots)
    {
        if (slots == null || slots.Count <= 0)
            return null;

        PlayerSlot available = null;

        for (int i = 0; i < slots.Count; i++)
        {
            Debug.Log(slots[i].name + ", " + slots[i]._playerJoined);
            if (slots[i] == null || slots[i]._playerJoined == true)
                continue;
            if (slots[i] != null && slots[i]._playerJoined == false)
            {
                available = slots[i];
                break;
            }
        }

        return available;
    }
}
