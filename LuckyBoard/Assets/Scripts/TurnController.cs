using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private EnemyPlayer enemyPlayer;

    private void Awake()
    {
        player.isTurn = true;
    }

    public void EndTurnPlayer()
    {
        player.isTurn = false;
        player.animator.SetBool("IsTurn", false);

        enemyPlayer.isTurn = true;
        enemyPlayer.animator.SetBool("IsTurn", true);
    }

    public void EndTurnEnemy()
    {
        player.isTurn = true;
        player.animator.SetBool("IsTurn", true);

        enemyPlayer.isTurn = false;
        enemyPlayer.animator.SetBool("IsTurn", false);
    }
}
