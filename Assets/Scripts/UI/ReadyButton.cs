using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButton : MonoBehaviour
{
    private List<CursorController> _acceptedPlayers = new List<CursorController>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// player cursors call this when they  press the accept button wich will take their ability to move 
    /// </summary>
    /// <param name="player"></param>
    public void PlayerInteract(CursorController player)
    {
        if(_acceptedPlayers.Contains(player))
        {
            player.canMove = true;
            _acceptedPlayers.Remove(player);
        } else
        {
            _acceptedPlayers.Add(player);
            player.canMove = false;
            if(_acceptedPlayers.Count == GameManager.Instance.GetCurrntPlayerCount())
            {
                AllPlayersReady();
            }
        }
    }

    /// <summary>
    /// called when the amount of ready players equl the amout of players to start the game
    /// </summary>
    public void AllPlayersReady()
    {
        GameManager.Instance.StartGame();
    }
}
