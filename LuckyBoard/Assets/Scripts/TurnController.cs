using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    [SerializeField]
    private MainPlayer mainPlayer;

    [SerializeField]
    private EnemyPlayer enemyPlayer;

    private AudioManager audioManager;

    private string[] mainPlayerTurnPhrases = {"OnMainPlayerTurn", "OnMainPlayerTurn2" };

    public delegate void SetRollButtonActive(bool active);
    public static event SetRollButtonActive setRollButton;

    public delegate void SetCameraPosition(bool isMainPlayer);
    public static event SetCameraPosition setMainCamera;

    public delegate void DisableBattleScores();
    public static event DisableBattleScores disableBattleScores;

    public delegate void UpdateBattleText(BattleStage stage, WinnerTypes winnerTypes);
    public static event UpdateBattleText updateBattleText;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void EndTurn(bool isMainPlayer)
    {
        if(mainPlayer.GetPlayerHealth() != 0 || enemyPlayer.GetPlayerHealth() != 0)
        {
            if (setRollButton != null)
            {
                setRollButton(!isMainPlayer);
            }

            if (setMainCamera != null)
            {
                setMainCamera(!isMainPlayer);
            }

            if (mainPlayer.rollCount == 0 && enemyPlayer.rollCount == 0)
            {
                SetIsBattling(false);
            }

            if (isMainPlayer)
            {
                mainPlayer.animator.SetBool("IsTurn", false);
                enemyPlayer.animator.SetBool("IsTurn", true);

                enemyPlayer.Roll(false);
            }
            else
            {
                mainPlayer.animator.SetBool("IsTurn", true);
                enemyPlayer.animator.SetBool("IsTurn", false);
                audioManager.Play(mainPlayerTurnPhrases[Random.Range(0, 2)]);
            }
        }
    }

    private void SetIsBattling(bool isBattling)
    {

        if(isBattling)
        {
            mainPlayer.SetBattleMode(true);
            enemyPlayer.SetBattleMode(true);
        }
        else
        {
            enemyPlayer.SetBattleMode(false);
            mainPlayer.SetBattleMode(false);
            
            CheckBattleWinner();

            Invoke("DisableScores", 3f);
        }
    }

    private void DisableScores()
    {
        if (disableBattleScores != null)
        {
            disableBattleScores();
        }
    }

    private void CheckBattleWinner()
    {
        if(updateBattleText != null)
        {
            var playerScore = mainPlayer.GetPlayerScore();
            var enemyScore = enemyPlayer.GetPlayerScore();

            if (playerScore > enemyScore)
            {

                updateBattleText(BattleStage.Winner, WinnerTypes.MainPlayer);
                Invoke("PunishMainPlayer", 2.5f);
            }
            else if (playerScore < enemyScore)
            {
                updateBattleText(BattleStage.Winner, WinnerTypes.EnemyPlayer);
                Invoke("PunishEnemyPlayer", 2.5f);

            }
            else
            {
                updateBattleText(BattleStage.Tie, WinnerTypes.None);
            }
        }

        mainPlayer.ResetScore();
        enemyPlayer.ResetScore();
    }

    private void PunishMainPlayer()
    {
        enemyPlayer.LoseHealth(false, true);
    }

    private void PunishEnemyPlayer()
    {
        mainPlayer.LoseHealth(true, true);
    }

    private void OnEnable()
    {
        Player.endPlayerTurn += EndTurn;
        Player.setIsBattling += SetIsBattling;
    }

    private void OnDisable()
    {
        Player.endPlayerTurn -= EndTurn;
        Player.setIsBattling -= SetIsBattling;
    }

}
