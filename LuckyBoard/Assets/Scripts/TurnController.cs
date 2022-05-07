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

    private bool isBattling = false;

    private string[] mainPlayerTurnPhrases = {"OnMainPlayerTurn", "OnMainPlayerTurn2" };

    public delegate void SetRollButtonActive(bool active);
    public static event SetRollButtonActive setRollButton;

    public delegate void SetCameraPosition(bool isMainPlayer);
    public static event SetCameraPosition setMainCamera;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void EndTurn(bool isMainPlayer)
    {
        if(isMainPlayer)
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

        if (setRollButton != null)
        {
            setRollButton(!isMainPlayer);
            Debug.Log("Set roll button: " + !isMainPlayer);
        }

        if(setMainCamera != null)
        {
            setMainCamera(!isMainPlayer);
        }

        if(isBattling)
        {
            mainPlayer.battleMode = true;
            enemyPlayer.battleMode = true;
        }else
        {
            mainPlayer.battleMode = false;
            enemyPlayer.battleMode = false;
        }
    }

    private void SetIsBattling(bool isBattling)
    {
        this.isBattling = isBattling;
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
