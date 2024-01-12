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
    private Animator animator;


    GameState state;

    private void Start()
    {
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialog;
        };
        DialogueManager.Instance.OnHideDialogue += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
    }

    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }else if (state == GameState.Dialog)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }
}
