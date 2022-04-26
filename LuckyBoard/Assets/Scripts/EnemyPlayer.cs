using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : Player
{
    [HideInInspector]
    public bool isTurn = false;

    [HideInInspector]
    private int steps;

    // Update is called once per frame
   private void Update()
    {
        if (isTurn && !isMoving)
        {
            steps = Random.Range(1, 7);
            audioManager.Play("DiceRoll");
            if (routePosition + steps < currentRoute.tiles.Count)
            {
                StartCoroutine(Move(steps, false));
            }
        }
    }
}
