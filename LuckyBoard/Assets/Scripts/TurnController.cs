using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    [SerializeField]
    private MainPlayer player;

    [SerializeField]
    private EnemyPlayer enemyPlayer;

    [SerializeField]
    private UIController uiController;

    private AudioManager audioManager;

    private string[] mainPlayerTurnPhrases = {"OnMainPlayerTurn", "OnMainPlayerTurn2" };

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void EndTurnPlayer()
    {
        player.animator.SetBool("IsTurn", false);
        enemyPlayer.animator.SetBool("IsTurn", true);

        enemyPlayer.Roll(false);
        uiController.SetRollButton(false);
    }

    public void EndTurnEnemy()
    {
        player.animator.SetBool("IsTurn", true);
        enemyPlayer.animator.SetBool("IsTurn", false);

        audioManager.Play(mainPlayerTurnPhrases[Random.Range(0, 2)]);
        uiController.SetRollButton(true);
    }
}
