using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTile : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private float healthLoss = 5f;

    [SerializeField]
    private EnemyPlayer enemyPlayer;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            player.loseHealth(healthLoss);
        }
        else if (collision.collider.CompareTag("EnemyPlayer"))
        {
            enemyPlayer.loseHealth(healthLoss);
        }
    }
}
