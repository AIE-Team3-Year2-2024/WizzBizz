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
            if (gamepadID <= 0)
            {
                PlayerInput p = _menuManager.primaryController.GetComponent<PlayerInput>();
                gamepad = (Gamepad)p.devices[0];
                mm = _menuManager.primaryController.GetComponentInChildren<MultiplayerEventSystem>();
                mm.playerRoot = slot.gameObject;
            }
            
            if (controllerPrefab != null && gamepadID > 0)
            {
                gamepad = Gamepad.all[gamepadID];
                if (gamepad == null)
                {
                    Debug.Log("No suitable gamepad found. " + gameObject.name);
                    return;
                }

                GameObject controller =
                    PlayerInput.Instantiate(controllerPrefab, controlScheme: "Gamepad", pairWithDevice: gamepad).gameObject;
                controller.transform.SetParent(_menuManager.transform);
                _menuManager.AddController(controller.GetComponent<PlayerInput>());
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
            slot._playerReady = true;
            _readyPlayers++;

            if (characterPrefabs != null && characterPrefabs.Count > 0 && 
                slot._selectedCharacterIndex > -1 && slot._selectedCharacterIndex < characterPrefabs.Count)
            {
                GameObject selectedCharacter = characterPrefabs[slot._selectedCharacterIndex];
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
