using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : Menu
{
    public List<PlayerSlot> playerSlots = new List<PlayerSlot>();

    [HideInInspector] public int _joinedPlayers = 0;
    [HideInInspector] public int _readyPlayers = 0;

    public void JoinPlayer()
    {
        Debug.Log("JOINED TEST");

        Debug.Log(playerSlots.Count);

        PlayerSlot slot = getNextAvailableSlot(playerSlots);
        if (slot != null)
        {
            _joinedPlayers++;
            slot.JoinPlayer();
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
            available = slots[i];
        }

        return available;
    }
}
