using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayer : Player
{
    void Start()
    {
        audioManager.Play("OnMainPlayerTurn");
    }

    private void OnEnable()
    {
        UIController.rolled += PlayerRoll;
    }

    private void OnDisable()
    {
        UIController.rolled -= PlayerRoll;
    }

    public void PlayerRoll()
    {
        Roll(true);
    }

}
