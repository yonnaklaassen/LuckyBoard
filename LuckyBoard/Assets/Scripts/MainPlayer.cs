using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayer : Player
{
    [HideInInspector]
    private int steps;

    [HideInInspector]
    public bool isTurn = false;

    void Start()
    {
        audioManager.Play("OnMainPlayerTurn");
    }

    private void OnEnable()
    {
        UIController.rolled += RollButtonClicked;
    }

    private void OnDisable()
    {
        UIController.rolled -= RollButtonClicked;
    }

    private void RollButtonClicked()
    {
        if(isTurn && !isMoving)
        {
            audioManager.Play("DiceRoll");
            steps = Random.Range(1, 7);

            if(routePosition + steps < currentRoute.tiles.Count)
            {
                StartCoroutine(Move(steps, true));
            }
        }
    }
}
