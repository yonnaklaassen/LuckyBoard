using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTile : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private EnemyPlayer enemyPlayer;

    [SerializeField]
    private float healthGain = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player.gainHealth(healthGain);
        }
        else if(collision.collider.CompareTag("EnemyPlayer"))
        {
            enemyPlayer.gainHealth(healthGain);
        }
    }
}
