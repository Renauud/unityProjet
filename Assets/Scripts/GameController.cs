using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    FreeRoam,
    Dialog,
    //Battle
}
public class GameController : MonoBehaviour
{
    [SerializeField]PlayerController playerController;

    GameState state;

    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }else if (state == GameState.Dialog){}
    }
}
