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

    public void AllPlayersReady()
    {
        GameManager.Instance.StartGame();
    }
}
