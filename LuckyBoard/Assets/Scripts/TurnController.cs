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

    private AudioManager audioManager;

    private string[] mainPlayerTurnPhrases = {"OnMainPlayerTurn", "OnMainPlayerTurn2" };

    public delegate void SetRollButtonActive(bool active);
    public static event SetRollButtonActive setRollButton;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void EndTurn(bool isMainPlayer)
    {
        if(isMainPlayer)
        {
            player.animator.SetBool("IsTurn", false);
            enemyPlayer.animator.SetBool("IsTurn", true);

            enemyPlayer.Roll(false);
        }
        else
        {
            player.animator.SetBool("IsTurn", true);
            enemyPlayer.animator.SetBool("IsTurn", false);
            Debug.Log("Play");
            audioManager.Play(mainPlayerTurnPhrases[Random.Range(0, 2)]);
        }

        if (setRollButton != null)
        {
            setRollButton(!isMainPlayer);
        }
    }

    private void OnEnable()
    {
        Player.endPlayerTurn += EndTurn;
    }

    private void OnDisable()
    {
        Player.endPlayerTurn -= EndTurn;
    }

}
